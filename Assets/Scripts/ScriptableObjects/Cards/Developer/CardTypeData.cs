using System;

namespace CardMaga.Card
{
    [Serializable]
    public class CardTypeData
    {
        [UnityEngine.SerializeField]
        private BodyPartEnum _bodyPart;
        public BodyPartEnum BodyPart { get => _bodyPart; set => _bodyPart = value; }

        [UnityEngine.SerializeField]
        private CardTypeEnum _cardType;
        public CardTypeEnum CardType { get => _cardType; set => _cardType = value; }


        public void CopyValues(CardTypeData other)
        {
            BodyPart = other.BodyPart;
            CardType = other.CardType;
        }
    }
}