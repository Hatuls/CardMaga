using Account.GeneralData;
using Collections;
using System;
using System.Linq;
using UnityEngine;
using Characters.Stats;
using System.Collections.Generic;

namespace Battle
{
    [CreateAssetMenu(fileName = "CharacterSO", menuName = "ScriptableObjects/Characters/CharacterSO")]
    public class CharacterSO : ScriptableObject
    {
        [SerializeField]
        private CharacterStats _characterStats;

        [SerializeField]
        private int _id;
        public int ID { get => _id; private set => _id = value; }

        [SerializeField] byte _unlockAtLevel;
        public byte UnlockAtLevel => _unlockAtLevel;

        [SerializeField]
        private string _characterName;
        public string CharacterName { get => _characterName; private set => _characterName = value; }

        [SerializeField]
        private ModelSO _characterVisual;
        public ModelSO ModelSO { get => _characterVisual; private set => _characterVisual = value; }

        [SerializeField]
        private Sprite _characterSprite;
        public Sprite CharacterSprite => _characterSprite;

        [SerializeField]

        private CharacterTypeEnum _characterType;
        public CharacterTypeEnum CharacterType { get => _characterType; private set => _characterType = value; }


        [SerializeField]
        private CharacterEnum _characterEnum;
        public CharacterEnum CharacterEnum => _characterEnum;

        [SerializeField]
        private short _characterDifficulty;
        public short CharacterDiffciulty { get => _characterDifficulty; private set => _characterDifficulty = value; }

        [SerializeField]
        private Account.GeneralData.ComboCore[] _combos;
        public Account.GeneralData.ComboCore[] Combos => _combos;
        [SerializeField]
        private CoreID[] _deck;
        public CoreID[] Deck { get => _deck; }



        public ref CharacterStats CharacterStats { get => ref _characterStats; }

        public SoundEventWithParamsSO SoundOnAttack; // parameter "Voice"
        public SoundEventWithParamsSO GetHitSounds;//Parameter "Get Hit"
        public SoundEventSO VictorySound; 
        public SoundEventSO TauntSounds; 
        public SoundEventSO DeathSounds; 
        public SoundEventSO ComboSounds; 



#if UNITY_EDITOR
        public bool Init(ushort id, string[] row, CardsCollectionSO cardCollection, ComboCollectionSO recipeCollections)
        {
            ID = id;

            //CharacterType
            const int CharacterTypeIndex = 1;
            const int CharacterEnumIndex = 2;
            const int CharacterDifficultyIndex = 3;
            const int CharacterNameIndex = 4;
            const int CharacterModelIndex = 5;
            const int CharacterUnlockLevel = 6;
            const int CharacterMaxHpIndex = 7;
            const int CharacterDefenseIndex = 8;
            const int CharacterStaminaIndex = 9;
            const int CharacterCardDrawIndex = 10;
            const int CharacterGoldIndex = 11;
            const int CharacterStrengthPointIndex = 12;
            const int CharacterDexterityPointIndex = 13;
            const int CharacterDeckIndex = 14;
            const int CharacterRecipeIndex = 15;
            const int RewardTypeIndex = 16;
            
            if (Enum.TryParse<CharacterTypeEnum>(row[CharacterTypeIndex].Replace(' ', '_'), out CharacterTypeEnum cte))
            {
                CharacterType = cte;
                if (int.TryParse(row[CharacterEnumIndex], out int ce))
                {
                    _characterEnum = (CharacterEnum)ce;

                    if (short.TryParse(row[CharacterDifficultyIndex], out short cde))
                    {
                        CharacterDiffciulty = cde;

                        //Name
                        if (row[CharacterNameIndex].Length != 0)
                        {
                            CharacterName = row[CharacterNameIndex];
                            Debug.Log(Application.dataPath);
                            
                            //AssetDatabase.LoadAssetAtPath(Application.dataPath, typeof(Material))
                            ModelSO characterModel =  Resources.Load<ModelSO>($"Art/Models/SO/{row[CharacterModelIndex]}");
                            if (row[CharacterModelIndex].Length != 0 && characterModel != null)
                            {
                                ModelSO = characterModel;

                                CharacterStats = new CharacterStats()
                                {
                                    MaxHealth = int.Parse(row[CharacterMaxHpIndex]),
                                    Shield = int.Parse(row[CharacterDefenseIndex]),
                                    StartStamina = int.Parse(row[CharacterStaminaIndex]),
                                    DrawCardsAmount = int.Parse(row[CharacterCardDrawIndex]),
                                    Gold = int.Parse(row[CharacterGoldIndex]),
                                    Bleed = 0,
                                    Strength = int.Parse(row[CharacterStrengthPointIndex]),
                                    Health = int.Parse(row[CharacterMaxHpIndex]),
                                    Dexterity = int.Parse(row[CharacterDexterityPointIndex]),
                                };

                                int _iD = 0;
                                int _level = 0;

                                const int iD = 0, Level = 1;
                                //deck cards
                                string[] Cards = row[CharacterDeckIndex].Trim().Split('&');
                                List<CoreID> cardCores = new List<CoreID>();

                                for (int i = 0; i < Cards.Length; i++)
                                {
                                    string[] data = Cards[i].Split('^');

                                    if (int.TryParse(data[iD], out int rID))
                                    {
                                        if (rID == -1)
                                            break;
                                        _iD = rID;
                                    }
                                    else
                                        throw new Exception($"CoreID= {ID} - {CharacterName} : BattleCard has no valid CoreID! ({data[iD]})");

                                    if (int.TryParse(data[Level], out int lvl))
                                    {
                                        _level = lvl;
                                    }
                                    else
                                        throw new Exception($"CoreID= {ID} - {CharacterName} : BattleCard has no valid level ({data[Level]}) for BattleCard id: {_id}");
                                    cardCores.Add(new CoreID(_iD + _level));

                                }
                                _deck = cardCores.ToArray();


                                //Recipes / Combos
                                string[] Recipe = row[CharacterRecipeIndex].Split('&');
                                List<ComboCore> comboCores = new List<ComboCore>();
                           

                                for (int i = 0; i < Recipe.Length; i++)
                                {
                                    string[] data = Recipe[i].Split('^');

                                    if (int.TryParse(data[iD], out int rID))
                                    {
                                        if (rID == -1)
                                            break;
                                        _iD = rID;
                                    }
                                    else
                                        throw new Exception($"CoreID= {ID} - {CharacterName} : Recipe has no valid CoreID! ({data[iD]})");


                                    if (int.TryParse(data[Level], out int lvl))
                                    {
                                        _level = lvl;
                                    }
                                    else
                                        throw new Exception($"CoreID= {ID} - {CharacterName} : Recipe has no valid level ({data[Level]}) for recipe id: {_id}");



                                    comboCores.Add(new Account.GeneralData.ComboCore(recipeCollections.AllCombos.First(x=>x.ID==_iD), _level));


                                }
                                _combos = comboCores.ToArray();

                          


                                if (byte.TryParse(row[CharacterUnlockLevel], out byte level))
                                    _unlockAtLevel = level;

                                LoadSounds(row);

                                return true;
                            }
                            else
                                Debug.LogError($"Coulmne G : CoreID= {ID} - Model Name ({row[CharacterModelIndex]}) Was Not correct or wasnt found on resources/Art/Avatars");
                        }
                        else
                            Debug.LogError($"Coulmne E: CoreID= {ID} Name is Empty!");
                    }
                    else
                        Debug.LogError($"Coulmne D: CoreID= {ID} Character Difficulty is not valid number! - {row[CharacterDifficultyIndex]} ");
                }
                else
                    Debug.LogError($"Coulmne C: CoreID={ID} Character enum is not valid! - {row[CharacterEnumIndex]}");
            }
            else
                Debug.LogError($"Coulmne B: CoreID= {ID} Character type is not a  ENUM!! - {row[CharacterTypeIndex]}");
            
            return false;
        }


        private void LoadSounds(string[] row)
        {
            string name = "Chiara"; // this is temp please fix the folders of the sounds to be based on the name of the character
            string folderPath = string.Concat("Audio/Characters/", name, "/");

            const int IndexSoundOnAttack = 17;
            const int IndexDamageSound = 18;
            const int IndexDeathSound = 19;
            const int IndexVictorySound = 20;
            const int IndexComboSound = 21;
            const int IndexTauntSound = 22;
            
            string fileName = row[IndexSoundOnAttack];
            string path = string.Concat(folderPath, fileName);
            SoundOnAttack = Resources.Load< SoundEventWithParamsSO>(path);

            fileName = row[IndexDamageSound];
            path = string.Concat(folderPath, fileName);
            GetHitSounds = Resources.Load<SoundEventWithParamsSO>(path);

            fileName = row[IndexDeathSound];
            path = string.Concat(folderPath, fileName);
            DeathSounds = Resources.Load<SoundEventSO>(path);

            fileName = row[IndexVictorySound];
            path = string.Concat(folderPath, fileName);
            VictorySound = Resources.Load<SoundEventSO>(path);

            fileName = row[IndexComboSound];
            path = string.Concat(folderPath, fileName);
            ComboSounds = Resources.Load<SoundEventSO>(path);

            fileName = row[IndexTauntSound];
            path = string.Concat(folderPath,  fileName);
            TauntSounds = Resources.Load<SoundEventSO>(path);
            
        }

#endif
    }
    
    public enum CharacterTypeEnum
    {
        None = 0,
        Player = 1,
        Tutorial = 2,
        Basic_Enemy = 3,
        Elite_Enemy = 4,
        Boss_Enemy = 5,
    }

}