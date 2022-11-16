using System;
using System.Collections.Generic;
using CardMaga.MetaData.AccoutData;

namespace CardMaga.MetaData.Collection
{
    public class MetaCollectionCardData : IEquatable<MetaCollectionCardData>
    {
        private const string FAILED_MESSAGE = "FAILED add or remove card"; 
        public event Action<MetaCardData> OnTryAddCard; 
        public event Action<MetaCardData> OnSuccessfullAddCard; 
        public event Action<MetaCardData> OnTryRemoveCard;
        public event Action<MetaCardData> OnSuccessfullRemoveCard;

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

        public void TryAddCardToDeck()
        {
            if (_numberOfInstant > 0)
            {
                OnTryAddCard?.Invoke(_cardReference);
                return;
            }
            
            OnFailedAction?.Invoke(FAILED_MESSAGE);
        }

        public void AddCardToDeck(MetaCardData metaCardData)
        {
            _numberOfInstant--;
            OnSuccessfullAddCard?.Invoke(_cardReference);
            
        }

        public void TryRemoveCardFromDeck()
        {
            OnTryRemoveCard?.Invoke(_cardReference);
        }

        public void RemoveCardFromDeck(MetaCardData metaCardData)
        {
            _numberOfInstant++;
            OnSuccessfullRemoveCard?.Invoke(_cardReference);
        }

        public bool Equals(MetaCollectionCardData other)
        {
            if (ReferenceEquals(null, other)) return false;
            return _cardId == other._cardId;
        }
    }
}