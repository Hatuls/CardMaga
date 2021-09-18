﻿using UnityEngine;
using Keywords;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
namespace Cards
{
    [CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/Cards")]
    public class CardSO : SerializedScriptableObject
    {


        #region Fields
        [TitleGroup("CardData", BoldTitle = true, Alignment = TitleAlignments.Centered)]

        [HorizontalGroup("CardData/Info/Display")]

        [OdinSerialize]
        [ShowInInspector]
        [VerticalGroup("CardData/Info/Display/Coulmn 1")]
        [PreviewField(100, Alignment = ObjectFieldAlignment.Right)]
        public Sprite CardSprite { get; set; }

        [VerticalGroup("CardData/Info/Display/Coulmn 2")]
        [LabelWidth(90f)]
        [SerializeField]
        private string _cardName;
        public string CardName { get=> _cardName; set=> _cardName=value; }

        [TabGroup("CardData/Info", "Animation")]
        [SerializeField]
        private AnimationBundle _animationBundle;
        public AnimationBundle AnimationBundle { get=> _animationBundle; set=> _animationBundle=value; }


        [TabGroup("CardData/Info", "Data")]
        [SerializeField]
        private RarityEnum _rarity;
        public RarityEnum Rarity { get=> _rarity; set=> _rarity=value; }

        [TabGroup("CardData/Info", "Data")]

        [ShowInInspector]
        public CardTypeData CardType => PerLevelUpgrade[0]?.UpgradesPerLevel[0]?.CardTypeData;

        public BodyPartEnum BodyPartEnum => CardType.BodyPart;
        public CardTypeEnum CardTypeEnum => CardType.CardType;





        [VerticalGroup("CardData/Info/Display/Coulmn 2")]
        [SerializeField]
        [LabelWidth(80f)]
        private string _cardDescription;
        public string CardDescription { get=> _cardDescription; set=> _cardDescription= value; }



        [TabGroup("CardData/Info", "Data")]
        [SerializeField]
        private int _stamina;
        public int StaminaCost { get=> _stamina; set=> _stamina=value; }

        [TabGroup("CardData/Info", "Data")]
        [SerializeField]
        private int _purchaseCost;
        public int PurchaseCost { get=> _purchaseCost; set=> _purchaseCost=value; }



        [TabGroup("CardData/Info", "Data")]
        [SerializeField]
        private bool _toExhaust;
        public bool ToExhaust { get=> _toExhaust; set=> _toExhaust=value; }

        [TabGroup("CardData/Info", "Data")]
        [Tooltip("How much coins the card cost")]
        [SerializeField]
        int _salvageCost = 1;

        [TabGroup("CardData/Info", "Data")]
        [SerializeField]
        private int _id;
        public int ID { get => _id; set => _id = value; }

        [TabGroup("CardData/Info", "Keywords")]
        [SerializeField]
        private KeywordData[] _cardKeywordsData;
        public KeywordData[] CardSOKeywords { get=> _cardKeywordsData; set=> _cardKeywordsData=value; }

        [TabGroup("CardData/Info", "Levels")]
        [SerializeField]
        private PerLevelUpgrade[] _perLevelUpgrades;
        public PerLevelUpgrade[] PerLevelUpgrade { get=> _perLevelUpgrades; set=> _perLevelUpgrades=value; }

        [TabGroup("CardData/Info", "Crafting")]
        [SerializeField]
        private int[] _cardsFusesFrom;
        public int[] CardsFusesFrom { get=> _cardsFusesFrom; set=> _cardsFusesFrom=value; }

        #endregion

        #region Properties




        [ShowInInspector]
        public int CardsMaxLevel => PerLevelUpgrade == null ? 0 : PerLevelUpgrade.Length-1;

        #endregion


        #region Methods
        public PerLevelUpgrade GetLevelUpgrade(int level)
        {

            if (level >=0 && level< PerLevelUpgrade.Length)
              return PerLevelUpgrade[level];
            
            return null;
        }
        #endregion

    }

    public enum LevelUpgradeEnum
    {
        None=0,
        Stamina=1,
        KeywordAddition=2,
        ConditionReduction=3,
        ToRemoveExhaust = 4,
        BodyPart = 5,
    }

    public enum RarityEnum
    {
        None = 0,
        Common = 1,
        Uncommon = 2,
        Rare= 3,
        Epic=4,
        LegendREI=5
    };
}