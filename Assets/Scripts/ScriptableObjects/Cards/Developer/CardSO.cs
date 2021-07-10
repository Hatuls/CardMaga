using UnityEngine;
using Keywords;
using System.Collections.Generic;

namespace Cards
{
    [CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/Cards")]
    public class CardSO : ScriptableObject
    {

        #region Fields
        [Header("Card Details:")]

        [Tooltip("Name of the card\n Note: the enum is also used to detect the currect animation")]
        [SerializeField] CardNamesEnum _cardName;

        [Tooltip("What Body Part Is Targeting")]
        [SerializeField] TargetedPartEnum _targetBodyPart;

        [Tooltip("What Type Of Card Is It?")]
        [SerializeField]
        CardType _cardData;


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
        [SerializeField] int _staminaCost = 1;


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
        public CardType GetCardType => _cardData;
        public RarityEnum GetCardsRarityLevel => _cardData._rarityLevel;
        public CardNamesEnum GetCardName => _cardName;
        public ref Sprite GetCardImage => ref _cardImage;
        public ref string GetCardDescription => ref _cardDescription;
        public ref string GetCardLCEDescription => ref _cardLCEDescription;
        public CardTypeEnum GetCardTypeEnum => _cardData._cardType;
        public TargetedPartEnum GetTargetBodyPart => _targetBodyPart;
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

    [System.Serializable]
    public class CardType
    {
        [Tooltip("What Type Of Card Is It?")]
        public CardTypeEnum _cardType;

        [Tooltip("What Body Part It Uses")]
        public BodyPartEnum _bodyPart;

        [Tooltip("Card rarity")]
        public RarityEnum _rarityLevel;
    }

    public class CardTypeComparaer : IEqualityComparer<CardType>
    {

        // Products are equal if their names and product numbers are equal.
        public bool Equals(CardType x, CardType y)
        {


            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y))
                return true;


            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;



            //Check whether the products' properties are equal.
            return x._bodyPart == y._bodyPart && x._cardType == y._cardType;

        }


        // If Equals() returns true for a pair of objects
        // then GetHashCode() must return the same value for these objects.
        public int GetHashCode(CardType obj)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(obj, null))
                return 0;

            //Get hash code for the Name field if it is not null.
            int hashBodyPart = obj._bodyPart.GetHashCode();
            int hashCardType = obj._cardType.GetHashCode();
            int hash_rarityLevel = obj._rarityLevel.GetHashCode();

            return hashBodyPart ^ hashCardType ^ hash_rarityLevel;
        }

    }
}