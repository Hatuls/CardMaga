using Account.GeneralData;
using CardMaga.Card;
using CardMaga.Keywords;

namespace CardMaga.Meta.AccountMetaData
{
    public class MetaCardData
    {
        private CardSO _cardSO;
        private CardInstanceID _cardInstanceID;
        private bool _toExhaust = false;
        private CardTypeData _cardTypeData;
        private KeywordData[] _cardKeyword;
        private int _staminaCost;
        
        
        public CardTypeData CardTypeData => _cardTypeData;
        public bool IsExhausted => _toExhaust;
        public Card.BodyPartEnum BodyPartEnum => _cardTypeData.BodyPart;
        public int CardInstanceID => _cardInstanceID.InstanceID;
        public int CardLevel => _cardInstanceID.Level;
        public int CardEXP => _cardInstanceID.Exp;
        public bool CardsAtMaxLevel => _cardSO.CardsMaxLevel - 1 == CardLevel; 
        public int StaminaCost => _staminaCost;

        public MetaCardData(CardInstanceID instanceID, CardSO cardSo)
        {
            _cardSO = cardSo;
            _cardInstanceID = instanceID;
        }
    }
}