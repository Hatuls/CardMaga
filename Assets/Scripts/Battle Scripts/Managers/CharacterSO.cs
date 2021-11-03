﻿using UnityEngine;
using Sirenix.OdinInspector;
using System;
using Collections;

namespace Battles
{
    [CreateAssetMenu(fileName = "CharacterSO", menuName = "ScriptableObjects/Characters/CharacterSO")]
    public  class CharacterSO : ScriptableObject
    {
        [Serializable]
        public class CardInfo
        {
            public CardInfo(Cards.CardSO card , byte level)
            {
                _cardSO = card;
                _level = level;
            }
            
            [SerializeField]
            private Cards.CardSO _cardSO;
            public Cards.CardSO Card { get => _cardSO; }

            [SerializeField]
            private byte _level;
            public byte Level { get => _level; }
        }

        [Serializable]
        public class RecipeInfo
        {
            public RecipeInfo(Combo.ComboSO comboSO, byte level)
            {
                _comboRecipe = comboSO;
                _level = level;
            }
            [SerializeField]
            private Combo.ComboSO _comboRecipe;
            public Combo.ComboSO ComboRecipe { get => _comboRecipe; }
            [SerializeField]
            private byte _level;
            public byte Level { get=> _level; }
        }

        [SerializeField]
        private Characters.Stats.CharacterStats _characterStats;

        [SerializeField]
        private int _id;
        public int ID { get=> _id; private set=> _id = value; }

        [SerializeField] byte _unlockAtLevel;
        public byte UnlockAtLevel => _unlockAtLevel;

        [SerializeField]
        private string _characterName;
        public string CharacterName { get=> _characterName; private set=> _characterName=value; }

        [SerializeField]
        [PreviewField(75f)]
        private GameObject _characterGO;
        public GameObject CharacterAvatar { get=> _characterGO; private set=> _characterGO= value; }

        [SerializeField]
        private Sprite _characterSprite;
        public Sprite CharacterSprite => _characterSprite;

        [SerializeField]

        private CharacterTypeEnum _characterType;
        public CharacterTypeEnum CharacterType { get => _characterType; private set=> _characterType=value; }


        [SerializeField]
        private CharacterEnum _characterEnum;
        public CharacterEnum CharacterEnum => _characterEnum;

        [SerializeField]
        private CharacterDifficultyEnum _characterDifficultyEnum;
        public CharacterDifficultyEnum CharacterDiffciulty { get => _characterDifficultyEnum; private set=> _characterDifficultyEnum=value; }
    
        [SerializeField]
        private RecipeInfo[] _combos;
        public RecipeInfo[] Combos => _combos;
        [SerializeField]
        private CardInfo[] _deck;
        public CardInfo[] Deck { get => _deck; }


        [SerializeField]
        private RewardTypeEnum _rewardType;
        public RewardTypeEnum RewardType { get=> _rewardType; private set=> _rewardType= value; }


        public ref Characters.Stats.CharacterStats CharacterStats { get =>ref _characterStats; }

        public bool Init(ushort id , string[] row, CardsCollectionSO cardCollection, ComboCollectionSO recipeCollections)
        {
            ID = id;


            //CharacterType
            const byte CharacterTypeIndex = 1;
            const byte CharacterEnumIndex = 2;
            const byte CharacterDifficultyIndex = 3;
            const byte CharacterNameIndex = 4;
            const byte CharacterModelIndex = 5;
            const byte CharacterUnlockLevel = 6;
            const byte CharacterMaxHpIndex =7;
            const byte CharacterDefenseIndex = 8;
            const byte CharacterStaminaIndex = 9;
            const byte CharacterCardDrawIndex = 10;
            const byte CharacterGoldIndex =11;
            const byte CharacterStrengthPointIndex = 12;
            const byte CharacterDexterityPointIndex = 13;
            const byte CharacterDeckIndex = 14;
            const byte CharacterRecipeIndex = 15;
            const byte RewardTypeIndex = 16;


            if (Enum.TryParse<CharacterTypeEnum>(row[CharacterTypeIndex].Replace(' ','_'), out CharacterTypeEnum cte))
            {
                CharacterType = cte;
                if (int.TryParse(row[CharacterEnumIndex], out int ce))
                {
                    _characterEnum = (CharacterEnum)ce;

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
                                _deck = new CardInfo[Cards.Length];
                                for (int i = 0; i < Cards.Length; i++)
                                {
                                    string[] data = Cards[i].Split('^');



                                    _deck[i] = new CardInfo(cardCollection.GetCard(ushort.Parse(data[iD])), byte.Parse(data[Level]));

                                }

                                //Recipes / Combos
                                string[] Recipe = row[CharacterRecipeIndex].Split('&');
                                _combos = new RecipeInfo[Recipe.Length];
                                for (int i = 0; i < Recipe.Length; i++)
                                {
                                    string[] data = Recipe[i].Split('^');
                                    _combos[i] = new RecipeInfo(recipeCollections.GetCombo(ushort.Parse(data[iD])), byte.Parse(data[Level]));


                                    if (_combos[i].ComboRecipe == null)
                                        Debug.LogError("!");
                                }


                                if (int.TryParse(row[RewardTypeIndex], out int RewardInt))
                                {
                                    RewardType = (RewardTypeEnum)RewardInt;
                                }


                                if (byte.TryParse(row[CharacterUnlockLevel], out byte level))
                                    _unlockAtLevel = level;



                                return true;
                            }
                            else
                                Debug.LogError($"Coulmne G : ID= {ID} - Model Name ({row[CharacterModelIndex]}) Was Not correct or wasnt found on resources/Art/Avatars");
                        }
                        else
                            Debug.LogError($"Coulmne E: ID= {ID} Name is Empty!");
                    }
                    else
                        Debug.LogError($"Coulmne D: ID= {ID} Character Difficulty is not valid ENUM! - {row[CharacterDifficultyIndex]} ");
                }
                else
                    Debug.LogError($"Coulmne C: ID={ID} Character enum is not valid! - {row[CharacterEnumIndex]}");
            }
            else
                Debug.LogError($"Coulmne B: ID= {ID} Character type is not a  ENUM!! - {row[CharacterTypeIndex]}");


            return false;
        }
    }





    public enum CharacterTypeEnum
    {
        None=0,
        Player =1,
        Tutorial =2,
        Basic_Enemy =3,
        Elite_Enemy =4,
        Boss_Enemy =5,
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