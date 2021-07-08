using UnityEngine;
using Keywords;
namespace Cards
{
    [CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/Cards")]
    public class CardSO : ScriptableObject
    {

        #region Fields
        [Header("Card Details:")]

        [Tooltip("Name of the card\n Note: the enum is also used to detect the currect animation")]
        [SerializeField] CardNamesEnum _cardName;

        [Tooltip("Card rarity")]
        public RarityEnum _rarityLevel;

        [Tooltip("What Type Of Card Is It?")]
        [SerializeField] CardTypeEnum _cardType;

        [Tooltip("What Section It Is Targeting")]
        [SerializeField] TargetedPartEnum _targetBodyPart;

        [Tooltip("What Body Part It Uses")]
        [SerializeField] BodyPartEnum _bodyPart;


        [Tooltip("The Image of the card:")]
        [SerializeField] Sprite _cardImage;


        [Tooltip("The Description of the card:")]
        [TextArea()]
        [SerializeField] string _cardDescription;   
        [Tooltip("The Description of the card's LCE:")]
        [SerializeField] string _cardLCEDescription;

        [Space]
        [Header("Mana:")]
        [Tooltip("How much stamina the card cost")]
        [SerializeField] int _staminaCost= 1;
        

        [Tooltip("How Many Times This Card Can Be Upgraded")]
        [SerializeField] int _maxUpgradeLevel = 1;
        [Tooltip("On What Level To Unlock the additional keywords")]
        [SerializeField] int _whenUnlockNewKeywords = 1;

        [Header("Card's Keywords: ")]
        [Header("Card's Regular Keywords: ")]
        [Tooltip("Card's Keywords:")]
        [SerializeField] KeywordData[] _keywords;
        [Header("Card's Additional Keywords: ")]
        [Tooltip("When Card Is Upgraded this keyword is added")]
        [SerializeField] KeywordData[] _upgrateKeywords;
        #endregion

        #region Properties
        public RarityEnum GetCardsRarityLevel => _rarityLevel;
        public CardNamesEnum GetCardName => _cardName;
        public ref Sprite GetCardImage => ref _cardImage;
        public ref string GetCardDescription => ref _cardDescription;
        public ref string GetCardLCEDescription => ref _cardLCEDescription;
        public CardTypeEnum GetCardTypeEnum => _cardType;
        public TargetedPartEnum GetTargetBodyPart => _targetBodyPart;
        public BodyPartEnum GetBodyPartEnum => _bodyPart;
        public  int GetStaminaCost =>  _staminaCost;
        public KeywordData[] GetCardsKeywords =>  _keywords;
        public KeywordData[] GetAdditionalKeywords => _upgrateKeywords;
        public ref int GetKeyWordMaxLevel => ref _maxUpgradeLevel;
        public ref int GetCardLevelToUnlockKeywords => ref _whenUnlockNewKeywords;
        #endregion


    }

    public enum RarityEnum {
    Common,
    Uncommon,
    Rare,
    Epic,
    LegendREI
    };
}
