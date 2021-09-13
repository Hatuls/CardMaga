using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;

namespace Battles
{
[CreateAssetMenu(fileName = "CharacterSO", menuName = "ScriptableObjects/Characters/CharacterSO")]
    public  class CharacterSO : SerializedScriptableObject
    {
        [Serializable]
        public class CardInfo
        {
            [OdinSerialize]
            [ShowInInspector]
            public Cards.CardSO Card { get; set; }
            [ShowInInspector]
            [OdinSerialize]
            public int Level { get; set; }
        }

        [Serializable]
        public class RecipeInfo
        {
            [OdinSerialize]
            [ShowInInspector]
            public Combo.ComboSO ComboRecipe { get; set; }
            [ShowInInspector]
            [OdinSerialize]
            public int Level { get; set; }
        }

        [ShowInInspector]
        [OdinSerialize]
        public int ID { get; private set; }
        [ShowInInspector]
        [OdinSerialize]
        public string CharacterName { get; private set; }

        [ShowInInspector]
        [OdinSerialize]
        public GameObject CharacterAvatar { get; private set; }
        [ShowInInspector]
        [OdinSerialize]
        public CharacterTypeEnum CharacterType { get; private set; }
        [ShowInInspector]
        [OdinSerialize]
        public CharacterDifficultyEnum CharacterDiffciulty { get; private set; }
        [ShowInInspector]
        [OdinSerialize]
        public RecipeInfo[] Combos { get; private set; }
        [ShowInInspector]
        [OdinSerialize]
        public CardInfo[] Deck { get; private set; }
        [ShowInInspector]
        [OdinSerialize]
        public RewardTypeEnum RewardType { get; private set; }

        [SerializeField] Cards.CardSO[] _characterCards;


       Cards.Card[] _cards;

        public Cards.Card[] GetCharacterCards {
            get
            {
                if (_cards == null || _cards.Length == 0)
                {
                    _cards = new Cards.Card[_characterCards.Length];

                    for (int i = 0; i < _characterCards.Length; i++)
                        _cards[i] = Managers.CardManager.CreateCard(CharacterType == CharacterTypeEnum.Class_1,_characterCards[i].CardName);
                }

                return   _cards; 
            }
        }

        public Characters.Stats.CharacterStats CharacterStats {get;private set;}



        public bool Init(int id , string[] row, CardsCollectionSO cardCollection, Collections.RelicsSO.ComboCollectionSO recipeCollections)
        {
            ID = id;


            //CharacterType
            const int CharacterTypeIndex = 1;
            const int CharacterDifficultyIndex = 2;
            const int CharacterNameIndex = 3;
            const int CharacterModelIndex = 4;
            const int CharacterMaxHpIndex = 5;
            const int CharacterDefenseIndex = 6;
            const int CharacterStaminaIndex = 7;
            const int CharacterCardDrawIndex = 8;
            const int CharacterGoldIndex = 9;
            const int CharacterStrengthPointIndex = 10;
            const int CharacterDexterityPointIndex = 11;
            const int CharacterDeckIndex = 12;
            const int CharacterRecipeIndex = 13;
            const int RewardTypeIndex = 14;


            if (Enum.TryParse<CharacterTypeEnum>(row[CharacterTypeIndex].Replace(' ','_'), out CharacterTypeEnum cte))
            {
                CharacterType = cte;
               
                if (Enum.TryParse<CharacterDifficultyEnum>(row[CharacterDifficultyIndex], out CharacterDifficultyEnum cde))
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


                            const int iD = 0, Level = 1;
                            //deck cards
                            string[] Cards = row[CharacterDeckIndex].Split('&');
                            Deck = new CardInfo[Cards.Length];
                            for (int i = 0; i < Cards.Length; i++)
                            {
                                string[] data = Cards[i].Split('^');



                                Deck[i] = new CardInfo()
                                {
                                    Card = cardCollection.GetCard(int.Parse(data[iD])),
                                    Level = int.Parse(data[Level]),
                                };
                            }

                            //Recipes / Combos
                            string[] Recipe = row[CharacterRecipeIndex].Split('&');
                            Combos = new RecipeInfo[Recipe.Length];
                            for (int i = 0; i < Recipe.Length; i++)
                            {
                                string[] data = Recipe[i].Split('^');
                                Combos[i] = new RecipeInfo()
                                {
                                    ComboRecipe = recipeCollections.GetCombo(int.Parse(data[iD])),
                                    Level = int.Parse(data[Level])
                                };
                            }


                            if (int.TryParse(row[RewardTypeIndex], out int RewardInt))
                            {
                                RewardType = (RewardTypeEnum)RewardInt;
                            }
                            return true;
                        }
                        else
                         Debug.LogError($"Coulmne E : ID= {ID} - Model Name ({row[CharacterModelIndex]}) Was Not correct or wasnt found on resources/Art/Avatars");
                    }
                    else
                        Debug.LogError($"Coulmne D: ID= {ID} Name is Empty!");
                }
                else
                    Debug.LogError($"Coulmne C: ID= {ID} Character Difficulty is not valid ENUM! - {row[CharacterDifficultyIndex]} ");
            }
            else
                Debug.LogError($"Coulmne B: ID= {ID} Character type is not a  ENUM!! - {row[CharacterTypeIndex]}");


            return false;
        }
    }

    public enum CharacterTypeEnum
    {
        None=0,
        Class_1 =1,
        Class_2 =2,
        Class_3 =3,
        Class_4 =4,
        Basic_Enemy =5,
        Elite_Enemy =6,
        Boss_Enemy =7,
    }

    public enum CharacterDifficultyEnum
    {
        None = 0,
        Player = 1,
        Tutorial =2,
        Easy =3,
        Medium =4,
        Hard =5,
    }


     [System.Flags]
     public enum RewardTypeEnum
    {
        None = 0,
        Gold= 1 <<0,
        CardReward= 2<<1,
        Recipe= 3<<2,
    }
}
