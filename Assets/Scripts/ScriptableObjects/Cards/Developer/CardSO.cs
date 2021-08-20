using UnityEngine;
using Keywords;
using Sirenix.OdinInspector;
namespace Cards
{
    [CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/Cards")]
    public class CardSO : ScriptableObject
    {

        #region Fields
        [TitleGroup("CardData", BoldTitle =true, Alignment = TitleAlignments.Centered)]  
     
        [HorizontalGroup("CardData/Info/Display")]


        [Tooltip("The Image of the card:")]
        [VerticalGroup("CardData/Info/Display/Coulmn 1")]
        [PreviewField(100, Alignment = ObjectFieldAlignment.Right )]
        [SerializeField] Sprite _cardImage;
     
        [Header("Card Details:")]
        [VerticalGroup("CardData/Info/Display/Coulmn 2")]
        [LabelWidth(90f)]
        [SerializeField] string _cardName;

        [TabGroup("CardData/Info", "Animation")]
        [SerializeField]
        AnimationBundle _animationBundle;

        [TabGroup("CardData/Info", "Data")]
        [Tooltip("What Type Of Card Is It?")]
        [SerializeField]
        CardType _cardData;




        [VerticalGroup("CardData/Info/Display/Coulmn 2")]
      
        [Tooltip("The Description of the card:")]
        [TextArea()]
        [SerializeField] string _cardDescription;


        [Space]
        [TabGroup("CardData/Info", "Data")]
        [Header("Mana:")]
        [Tooltip("How much stamina the card cost")]
        [SerializeField] int _staminaCost = 1;

        [TabGroup("CardData/Info", "Data")]
        [Tooltip("How Many Times This Card Can Be Upgraded")]
        [SerializeField] int _maxUpgradeLevel = 1;
        [TabGroup("CardData/Info", "Data")]
        [Tooltip("On What Level To Unlock the additional keywords")]
        [SerializeField] int _whenUnlockNewKeywords = 1;

        [Header("Card's Keywords: ")]

        [Header("Card's Regular Keywords: ")]
        [Tooltip("Card's Keywords:")]
        [TabGroup("CardData/Info", "Keywords")]
        [SerializeField] KeywordData[] _keywords;

        [TabGroup("CardData/Info", "Keywords")]
        [Header("Card's Additional Keywords: ")]
        [Tooltip("When Card Is Upgraded this keyword is added")]
        [SerializeField] KeywordData[] _upgrateKeywords;
        #endregion

        #region Properties
        public CardType GetCardType => _cardData;
        public RarityEnum GetCardsRarityLevel => _cardData._rarityLevel;
        public string GetCardName => _cardName;
        public ref Sprite GetCardImage => ref _cardImage;
        public ref string GetCardDescription => ref _cardDescription;

        public CardTypeEnum GetCardTypeEnum => _cardData._cardType;
        public AnimationBundle GetAnimationBundle => _animationBundle;
        public BodyPartEnum GetBodyPartEnum => _cardData._bodyPart;
        public int GetStaminaCost => _staminaCost;
        public KeywordData[] GetCardsKeywords => _keywords;
        public KeywordData[] GetAdditionalKeywords => _upgrateKeywords;
        public ref int GetKeyWordMaxLevel => ref _maxUpgradeLevel;
        public ref int GetCardLevelToUnlockKeywords => ref _whenUnlockNewKeywords;
        #endregion


    }

    public enum RarityEnum
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        LegendREI
    };
}