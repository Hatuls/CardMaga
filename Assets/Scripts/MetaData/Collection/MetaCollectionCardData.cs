using System;
using System.Collections.Generic;
using CardMaga.MetaData.AccoutData;

namespace CardMaga.MetaData.Collection
{
    public class MetaCollectionCardData : IEquatable<MetaCollectionCardData>
    {
        private const string FAILED_MESSAGE = "FAILED add or remove card"; 
        public event Action<MetaCardData> OnTryAddCard; 
        public event Action<MetaCardData> OnRemoveCard;

        public event Action<string> OnFailedAction; 

        private int _cardId;
        private int _numberOfInstant;
        private MetaCardData _cardReference;

        public int CardId => _cardId;

        public int NumberOfInstant => _numberOfInstant;
        

        public MetaCardData CardReference => _cardReference;


        public MetaCollectionCardData(int numberOfInstant,MetaCardData cardReference)
        {
            _cardId = cardReference.CardInstance.ID;
            _numberOfInstant = numberOfInstant;
            _cardReference = cardReference;
        }

        public bool TryAddCardToDeck()
        {
            if (_numberOfInstant <= 0)
            {
                OnFailedAction?.Invoke(FAILED_MESSAGE);
                return false;
            }
            OnTryAddCard?.Invoke(_cardReference);
            return true;
        }

        public void AddCardToDeck(MetaCardData metaCardData)
        {
            _numberOfInstant--;
        }

        public void RemoveCardFromDeck()
        {
            _numberOfInstant++;
            OnRemoveCard?.Invoke(_cardReference);
        }

        public bool Equals(MetaCollectionCardData other)
        {
            if (ReferenceEquals(null, other)) return false;
            return _cardId == other._cardId;
        }
    }
}