using System;
using Account.GeneralData;
using CardMaga.Card;
using CardMaga.Keywords;

namespace CardMaga.Meta.AccountMetaData
{
    public class MetaCardData : IEquatable<MetaCardData>
    {
        private CardSO _cardSO;
        private CardInstanceID _cardInstanceID;
        private CardData _cardData;
        private bool _toExhaust = false;
        private CardTypeData _cardTypeData;
        private KeywordData[] _cardKeyword;
        private int _staminaCost;


        public CardData CardData => _cardData; //need to by remove
        public CardTypeData CardTypeData => _cardTypeData;
        public bool IsExhausted => _toExhaust;
        public Card.BodyPartEnum BodyPartEnum => _cardTypeData.BodyPart;
        public int CardInstanceID => _cardInstanceID.InstanceID;
        public int CardLevel => _cardInstanceID.Level;
        public int CardEXP => _cardInstanceID.Exp;
        public bool CardsAtMaxLevel => _cardSO.CardsMaxLevel - 1 == CardLevel; 
        public int StaminaCost => _staminaCost;

        public MetaCardData(CardInstanceID instanceID, CardSO cardSo,CardData cardData)
        {
            _cardData = cardData;
            _cardSO = cardSo;
            _cardInstanceID = instanceID;
        }

        public bool Equals(MetaCardData other)
        {
            if (other == null)
                return false;
            
            return CardInstanceID == other.CardInstanceID;
        }
    }
}