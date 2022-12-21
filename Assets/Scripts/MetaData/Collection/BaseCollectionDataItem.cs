using System;
using System.Collections.Generic;

namespace CardMaga.MetaData.Collection
{
    public abstract class BaseCollectionDataItem<T> where T : IEquatable<T>
    {
        private const string FAILED_MESSAGE = "FAILED add or remove item"; 
        
        public event Action<T> OnTryAddItemToCollection; 
        public event Action<T> OnSuccessfulAddItemToCollection; 
        public event Action<T> OnTryRemoveItemFromCollection;
        public event Action<T> OnSuccessfulRemoveItemFromCollection;
        public event Action<string> OnFailedAction;

        private readonly int _maxInstants;
        private int _numberOfInstant;
        /// <summary>
        /// Associate Deck first number the deck Id, Second number is the number of the Instant in the deck
        /// </summary>
        Dictionary<int, int> _associateDeck;

        public IReadOnlyDictionary<int, int> AssociateDeck => _associateDeck;

        public abstract T ItemReference { get; }

        public bool IsNotMoreInstants => _numberOfInstant <= 0;
        public bool IsMaxInstants => _numberOfInstant == _maxInstants;
        
        public int NumberOfInstant => _numberOfInstant;

        protected BaseCollectionDataItem(int numberOfInstant,int maxInstants)
        {
            _associateDeck = new Dictionary<int, int>();
            _numberOfInstant = numberOfInstant;
            _maxInstants = maxInstants;
        }
        
        public void AddItemToCollection()
        {
            if (_numberOfInstant > 0)
            {
                OnTryAddItemToCollection?.Invoke(ItemReference);
                return;
            }
            
            OnFailedAction?.Invoke(FAILED_MESSAGE);
        }

        public void AddItemToCollection(T itemData)
        {
            if (!ItemReference.Equals(itemData)) return;
           _numberOfInstant--;
            OnSuccessfulAddItemToCollection?.Invoke(ItemReference);
        }

        public void RemoveItemFromCollection()
        {
            OnTryRemoveItemFromCollection?.Invoke(ItemReference);
        }

        public void RemoveItemFromCollection(T itemData)
        {
            if (!ItemReference.Equals(itemData)) return;
            _numberOfInstant++;
            OnSuccessfulRemoveItemFromCollection?.Invoke(ItemReference);
        }

        public void AddItemToAssociateDeck(int deckId)
        {
            if (!_associateDeck.TryGetValue(deckId, out var value))
                _associateDeck.Add(deckId,1);

            value++;
            _associateDeck[deckId] = value;
        }
        
        public void RemoveItemToAssociateDeck(int deckId)
        {
            if (!_associateDeck.TryGetValue(deckId, out var value)) 
                return;
            
            value--;
            
            if (value == 0)
            {
                _associateDeck.Remove(deckId);
                return;
            }
            
            _associateDeck[deckId] = value;
        }
    }
}