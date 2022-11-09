using System;
using Account.GeneralData;
using CardMaga.Card;
using CardMaga.Keywords;

namespace CardMaga.Meta.AccountMetaData
{
    public class MetaCardData : IEquatable<MetaCardData>
    {
        private CardSO _cardSO;
        private CardInstance _cardInstance;
        private BattleCardData _battleCardData;
        private bool _toExhaust = false;
        private CardTypeData _cardTypeData;
        private KeywordData[] _cardKeyword;
        private int _staminaCost;


        public BattleCardData BattleCardData => _battleCardData; //need to by remove
        public CardTypeData CardTypeData => _cardTypeData;
        public bool IsExhausted => _toExhaust;
        public Card.BodyPartEnum BodyPartEnum => _cardTypeData.BodyPart;
        public CardInstance CardInstance => _cardInstance;
        public int CardLevel => _cardInstance.Level;
      
        public bool CardsAtMaxLevel => _cardSO.CardsMaxLevel - 1 == CardLevel; 
        public int StaminaCost => _staminaCost;

        public MetaCardData(CardInstance instance, CardSO cardSo,BattleCardData battleCardData)//temp
        {
            _battleCardData = battleCardData;
            _cardSO = cardSo;
            _cardInstance = instance;
        }
        
        public MetaCardData(CardInstance instance, CardSO cardSo)
        {
            _cardSO = cardSo;
            _cardInstance = instance;
        }

        public bool Equals(MetaCardData other)
        {
            if (other == null)
                return false;
            
            return CardInstance == other.CardInstance;
        }
    }
}