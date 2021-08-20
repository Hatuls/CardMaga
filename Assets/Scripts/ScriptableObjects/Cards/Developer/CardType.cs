using UnityEngine;
namespace Cards
{
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
}