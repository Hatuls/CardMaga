using System;

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
        
        protected int _numberOfInstant;
        protected int _maxInstants;
        
        public abstract T ItemReference { get;}

        public bool IsNotMoreInstants => _numberOfInstant <= 0;
        public bool IsMaxInstants => _numberOfInstant == _maxInstants;
        
        public int NumberOfInstant => _numberOfInstant;
        
        public void TryAddItemToCollection()
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

        public void TryRemoveItemFromCollection()
        {
            OnTryRemoveItemFromCollection?.Invoke(ItemReference);
        }

        public void RemoveItemFromCollection(T itemData)
        {
            if (!ItemReference.Equals(itemData)) return;
            _numberOfInstant++;
            OnSuccessfulRemoveItemFromCollection?.Invoke(ItemReference);
        }
    }
}