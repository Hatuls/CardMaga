using Account.GeneralData;
using Cards;
using Collections;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;

namespace Battles
{
    [CreateAssetMenu(fileName = "CharacterSO", menuName = "ScriptableObjects/Characters/CharacterSO")]
    public class CharacterSO : ScriptableObject
    {


      
        [SerializeField]
        private Characters.Stats.CharacterStats _characterStats;

        [SerializeField]
        private int _id;
        public int ID { get => _id; private set => _id = value; }

        [SerializeField] byte _unlockAtLevel;
        public byte UnlockAtLevel => _unlockAtLevel;

        [SerializeField]
        private string _characterName;
        public string CharacterName { get => _characterName; private set => _characterName = value; }

        [SerializeField]
        [PreviewField(75f)]
        private GameObject _characterGO;
        public GameObject CharacterAvatar { get => _characterGO; private set => _characterGO = value; }

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
        private Combo.Combo[] _combos;
        public Combo.Combo[] Combos => _combos;
        [SerializeField]
        private CardInstanceID.CardCore[] _deck;
        public CardInstanceID.CardCore[] Deck { get => _deck; }


        [SerializeField]
        private RewardTypeEnum _rewardType;
        public RewardTypeEnum RewardType { get => _rewardType; private set => _rewardType = value; }


        public ref Characters.Stats.CharacterStats CharacterStats { get => ref _characterStats; }

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

                            GameObject characterModel = Resources.Load<GameObject>($"Art/Models/{row[CharacterModelIndex]}");
                            if (row[CharacterModelIndex].Length != 0 && characterModel != null)
                            {
                                CharacterAvatar = characterModel;

                                CharacterStats = new Characters.Stats.CharacterStats()
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
                                string[] Cards = row[CharacterDeckIndex].Split('&');
                                _deck = new CardInstanceID.CardCore[Cards.Length];
                                for (int i = 0; i < Cards.Length; i++)
                                {
                                    string[] data = Cards[i].Split('^');

                                    if (ushort.TryParse(data[iD], out ushort rID))
                                    {
                                        _iD = rID;
                                    }
                                    else
                                        throw new Exception($"ID= {ID} - {CharacterName} : Card has no valid ID! ({data[iD]})");

                                    if (int.TryParse(data[Level], out int lvl))
                                    {
                                        _level = lvl;
                                    }
                                    else
                                        throw new Exception($"ID= {ID} - {CharacterName} : Card has no valid level ({data[Level]}) for Card id: {_id}");
                                    _deck[i] = new CardInstanceID.CardCore(cardCollection.GetAllCards.First(x=>x.ID==_iD).ID, _level,0);

                                }

                                //Recipes / Combos
                                string[] Recipe = row[CharacterRecipeIndex].Split('&');
                                _combos = new Combo.Combo[Recipe.Length];

                                for (int i = 0; i < Recipe.Length; i++)
                                {
                                    string[] data = Recipe[i].Split('^');

                                    if (int.TryParse(data[iD], out int rID))
                                    {
                                        _iD = rID;
                                    }
                                    else
                                        throw new Exception($"ID= {ID} - {CharacterName} : Recipe has no valid ID! ({data[iD]})");


                                    if (int.TryParse(data[Level], out int lvl))
                                    {
                                        _level = lvl;
                                    }
                                    else
                                        throw new Exception($"ID= {ID} - {CharacterName} : Recipe has no valid level ({data[Level]}) for recipe id: {_id}");



                                    _combos[i] = new Combo.Combo(recipeCollections.AllCombos.First(x=>x.ID==_iD), _level);


                                    if (_combos[i] == null)
                                        Debug.LogError("!");
                                }


                                if (int.TryParse(row[RewardTypeIndex], out int RewardInt))
                                {
                                    RewardType = (RewardTypeEnum)RewardInt;
                                }


                                if (byte.TryParse(row[CharacterUnlockLevel], out byte level))
                                    _unlockAtLevel = level;

                                LoadSounds(row);

                                return true;
                            }
                            else
                                Debug.LogError($"Coulmne G : ID= {ID} - Model Name ({row[CharacterModelIndex]}) Was Not correct or wasnt found on resources/Art/Avatars");
                        }
                        else
                            Debug.LogError($"Coulmne E: ID= {ID} Name is Empty!");
                    }
                    else
                        Debug.LogError($"Coulmne D: ID= {ID} Character Difficulty is not valid number! - {row[CharacterDifficultyIndex]} ");
                }
                else
                    Debug.LogError($"Coulmne C: ID={ID} Character enum is not valid! - {row[CharacterEnumIndex]}");
            }
            else
                Debug.LogError($"Coulmne B: ID= {ID} Character type is not a  ENUM!! - {row[CharacterTypeIndex]}");





            return false;
        }


        private void LoadSounds(string[] row)
        {
            string folderPath = string.Concat("Audio/Characters/", _characterName, "/");

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


    [System.Flags]
    public enum RewardTypeEnum
    {
        None = 0,
        Gold = 1 << 0,
        CardReward = 2 << 1,
        Recipe = 3 << 2,
    }


}