using Account.GeneralData;
using CardMaga.Card;
using CardMaga.Keywords;

namespace CardMaga.Meta.AccountMetaData
{
    public class MetaCardData
    {
        private CardSO _cardSO;
        private CardInstanceID _cardCoreInfo;
        private bool _toExhaust = false;
        private CardTypeData _cardTypeData;
        private KeywordData[] _cardKeyword;
        private int _staminaCost;
        
        
        public CardTypeData CardTypeData => _cardTypeData;
        public bool IsExhausted => _toExhaust;
        public Card.BodyPartEnum BodyPartEnum => _cardTypeData.BodyPart;
        public int CardInstanceID => _cardCoreInfo.InstanceID;
        public int CardLevel => _cardCoreInfo.Level;
        public int CardEXP => _cardCoreInfo.Exp;
        public bool CardsAtMaxLevel => _cardSO.CardsMaxLevel - 1 == CardLevel; 
        public int StaminaCost => _staminaCost; 
    }
}