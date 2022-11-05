using Account.GeneralData;
using CardMaga.Animation;
using CardMaga.Keywords;
using Cards;
using Collections;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CardMaga.Card
{
    [CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/Cards")]
    public class CardSO : ScriptableObject
    {
        #region Fields
        [TitleGroup("CardData", BoldTitle = true, Alignment = TitleAlignments.Centered)]

        [HorizontalGroup("CardData/Info/Display")]


        [VerticalGroup("CardData/Info/Display/Coulmn 1")]
        [PreviewField(100, Alignment = ObjectFieldAlignment.Right)]
        [SerializeField]
        private Sprite _cardSprite;


        [VerticalGroup("CardData/Info/Display/Coulmn 2")]
        [LabelWidth(90f)]
        [SerializeField]
        private string _cardName;
  

        [TabGroup("CardData/Info", "Animation")]
        [SerializeField]
        private AnimationBundle _animationBundle;
     

        [TabGroup("CardData/Info", "Camera")]
        [SerializeField]
        private CameraDetails cameraDetails;


        [TabGroup("CardData/Info", "Data")]
        [SerializeField]
        private RarityEnum _rarity;


        [TabGroup("CardData/Info", "Data")]

        [ShowInInspector]
        public CardTypeData CardType => PerLevelUpgrade[0]?.UpgradesPerLevel[0]?.CardTypeData;






        [TabGroup("CardData/Info", "Data")]
        [SerializeField]
        private byte _stamina;

        [TabGroup("CardData/Info", "Data")]
        private bool _isFuseCard;
        [TabGroup("CardData/Info", "Data")]
        private bool _isCombo;



        [TabGroup("CardData/Info", "Data")]
        [Tooltip("How much coins the card cost")]
        [SerializeField]
        ushort _salvageCost = 1;

        [TabGroup("CardData/Info", "Data")]
        [SerializeField]
        private ushort _id;


     
        [TabGroup("CardData/Info", "Data")]
        [SerializeField]
        bool _isPackReward;

        [TabGroup("CardData/Info", "Keywords")]
        [SerializeField]
        private KeywordData[] _cardKeywordsData;
 

        [TabGroup("CardData/Info", "Levels")]
        [SerializeField]
        private PerLevelUpgrade[] _perLevelUpgrades;


        [TabGroup("CardData/Info", "Crafting")]
        [SerializeField]
        private ushort[] _cardsFusesFrom;

        [TabGroup("CardData/Info", "Instances")]
        [SerializeField]
        private CardCoreInfo[] _cardCoreInfo;

        #endregion

        #region Properties
        public bool IsFusedCard => _isFuseCard;
        public bool IsCombo => _isCombo;
        public ushort ID { get => _id; set => _id = value; }
        public PerLevelUpgrade[] PerLevelUpgrade { get => _perLevelUpgrades; set => _perLevelUpgrades = value; }
        public ushort[] CardsFusesFrom { get => _cardsFusesFrom; set => _cardsFusesFrom = value; }
        public Sprite CardSprite { get => _cardSprite; set => _cardSprite = value; }
        public KeywordData[] CardSOKeywords { get => _cardKeywordsData; set => _cardKeywordsData = value; }
        [ShowInInspector]
        public byte CardsMaxLevel => PerLevelUpgrade == null ? (byte)0 : (byte)PerLevelUpgrade.Length;
        public byte StaminaCost { get => _stamina; set => _stamina = value; }
        public BodyPartEnum BodyPartEnum => CardType.BodyPart;
        public CardTypeEnum CardTypeEnum => CardType.CardType;
        public CameraDetails CameraDetails { get => cameraDetails; set => cameraDetails = value; }
        public string CardName { get => _cardName; set => _cardName = value; }
        public bool IsPackReward { get => _isPackReward; set => _isPackReward = value; }
        public RarityEnum Rarity { get => _rarity; set => _rarity = value; }
        public CardCoreInfo[] CardCore => _cardCoreInfo;
        public AnimationBundle AnimationBundle
        {
            get
            {
                _animationBundle.CameraDetails = cameraDetails;
                return _animationBundle;
            }
            set => _animationBundle = value;
        }
        #endregion

        #region Methods

        public int GetCardValue(int level)
            => _perLevelUpgrades[level].CardValue;
        public PerLevelUpgrade GetLevelUpgrade(int level)
        {

            if (level >= 0 && level < PerLevelUpgrade.Length)
                return PerLevelUpgrade[level];

            throw new System.Exception($"CardSO: ID:{ID}\n trying To get level {level} max level is {CardsMaxLevel}");
        }

        //public DescriptionInfo[] CardDescription(int level)
        //   => PerLevelUpgrade[level].Description;

        public List<string[]> CardDescription(int level)
        {
            List<string[]> description = new List<string[]>();

            for (int i = 0; i < PerLevelUpgrade[level].Description.Length; i++)
                description.Add(PerLevelUpgrade[level].Description[i].Description);
            return description;
        }
        public ushort GetCostPerUpgrade(int level)
        {
            return GetLevelUpgrade(level).PurchaseCost;
        }

        public KeywordData[] KeywordsCombin(int lvl)
        {
            var combines = GetLevelUpgrade(lvl);

            var keywordsAddition = combines.UpgradesPerLevel
                .Where((x) => x.UpgradeType == LevelUpgradeEnum.KeywordAddition).Select((X) => X.KeywordUpgrade);

            List<KeywordSO> keywordTypes = new List<KeywordSO>();
            List<KeywordData> _keywords = new List<KeywordData>();
            foreach (var item in keywordsAddition)
            {
                if (!keywordTypes.Contains(item.KeywordSO))
                {
                    keywordTypes.Add(item.KeywordSO);
                    _keywords.Add(new KeywordData(item.KeywordSO,
                        TargetEnum.None,
                        keywordsAddition
                        .Where(x => x.KeywordSO.GetKeywordType == item.KeywordSO.GetKeywordType)
                        .Sum(x => x.GetAmountToApply), 0));
                }
            }




            return _keywords.ToArray();
        }

        #endregion
#if UNITY_EDITOR
        #region Editor
        public bool ContainID (int id)
        {
      
            for (int i = 0; i < _cardCoreInfo.Length; i++)
            {
            
                if (_cardCoreInfo[i].CardCore.ID == id)
                    return true;
            }
            return false;
        }
        public void CreateCardCoreInfo()
        {
            _cardCoreInfo = new CardCoreInfo[CardsMaxLevel];

            for (int i = 0; i < _cardCoreInfo.Length; i++)
            {
                _cardCoreInfo[i] = new CardCoreInfo();
                _cardCoreInfo[i].InitCardData(ID + i, i + 1);
            }
        }
        public void AssignRewardData(int iD, bool isBasicPack, bool isSpecialPack, bool isArenaReward = false)
        {
            CardCoreInfo current = GetCore(iD);
            current.InitRewardData(isSpecialPack, isBasicPack, isArenaReward);



            CardCoreInfo GetCore(int id)
            {
                for (int i = 0; i < _cardCoreInfo.Length; i++)
                {
                    if (_cardCoreInfo[i].CardCore.ID == id)
                        return _cardCoreInfo[i];
                }
                throw new Exception($"CARDSO: Cardcore could not be found\nID - {id}");
            }
            
        }
        #endregion
#endif

    }

    public enum LevelUpgradeEnum
    {
        None = 0,
        Stamina = 1,
        KeywordAddition = 2,
        ConditionReduction = 3,
        ToRemoveExhaust = 4,
        BodyPart = 5,
    }

    public enum RarityEnum
    {
        None = 0,
        Common = 1,
        Uncommon = 2,
        Rare = 3,
        Epic = 4,
        LegendREI = 5
    };



    [Serializable]
    public class CardCoreInfo
    {

        [SerializeField] private CardCore _cardCore;
        [SerializeField] private bool _isSpecialReward;
        [SerializeField] private bool _isBasicReward;
        [SerializeField] private bool _isArenaReward;


        public bool IsSpecialReward => _isSpecialReward;
        public bool IsArenaReward => _isArenaReward; 
        public bool IsBasicReward => _isBasicReward;
        public CardCore CardCore => _cardCore;



        #region UNITY_EDITOR
        public void InitCardData(int id, int level)
        {
            _cardCore = new CardCore(id, level);
        }
        public void InitRewardData(bool isSpecialReward, bool isBasicReward,bool isArenaReward)
        {
            _isArenaReward = isArenaReward;
            _isBasicReward = isBasicReward;
            _isSpecialReward = isSpecialReward;
        }
        #endregion
    }
}