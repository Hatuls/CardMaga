using System;
using CardMaga.MetaData.AccoutData;

namespace CardMaga.MetaData.Collection
{
    public class MetaCollectionCardData : IEquatable<MetaCollectionCardData>,IEquatable<MetaCardData>
    {
        private const string FAILED_MESSAGE = "FAILED add or remove card"; 
        public event Action<MetaCardData> OnTryAddCard; 
        public event Action<MetaCardData> OnSuccessfullAddCard; 
        public event Action<MetaCardData> OnTryRemoveCard;
        public event Action<MetaCardData> OnSuccessfullRemoveCard;

        public event Action<string> OnFailedAction; 

        private int _numberOfInstant;
        private MetaCardData _cardReference;

        public int CardId => _cardReference.CardInstance.ID;

        public int NumberOfInstant => _numberOfInstant;
        

        public MetaCardData CardReference => _cardReference;


        public MetaCollectionCardData(int numberOfInstant,MetaCardData cardReference)
        {
            _numberOfInstant = numberOfInstant;
            _cardReference = cardReference;
        }

        public void TryRemoveCardReference()
        {
            if (_numberOfInstant > 0)
            {
                OnTryAddCard?.Invoke(_cardReference);
                return;
            }
            
            OnFailedAction?.Invoke(FAILED_MESSAGE);
        }

        public void RemoveCardReference(MetaCardData metaCardData)
        {
            if (_cardReference.Equals(metaCardData))
            {
                _numberOfInstant--;
                OnSuccessfullAddCard?.Invoke(_cardReference);
            }
        }

        public void TryAddCardReference()
        {
            OnTryRemoveCard?.Invoke(_cardReference);
        }

        public void AddCardReference(MetaCardData metaCardData)
        {
            if (_cardReference.Equals(metaCardData))
            {
                _numberOfInstant++;
                OnSuccessfullRemoveCard?.Invoke(_cardReference);
            }
        }

        public bool Equals(MetaCollectionCardData other)
        {
            if (ReferenceEquals(null, other)) return false;
            return CardId == other.CardId;
        }

        public bool Equals(MetaCardData other)
        {
            if (ReferenceEquals(null, other)) return false;
            return CardId == other.CardInstance.ID;
        }
    }
}