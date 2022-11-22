using System;

namespace CardMaga.MetaData.Collection
{
    public abstract class BaseCollectionItemData<T> where T : IEquatable<T>
    {
        private const string FAILED_MESSAGE = "FAILED add or remove item"; 
        
        public event Action<T> OnTryAddItem; 
        public event Action<T> OnSuccessfullAddItem; 
        public event Action<T> OnTryRemoveItem;
        public event Action<T> OnSuccessfullRemoveItem;
        
        public event Action<string> OnFailedAction; 
        
        protected int _numberOfInstant;
        
        public abstract T ItemReference { get;}
        
        public int NumberOfInstant => _numberOfInstant;
        
        public void TryRemoveItemReference()
        {
            if (_numberOfInstant > 0)
            {
                OnTryAddItem?.Invoke(ItemReference);
                return;
            }
            
            OnFailedAction?.Invoke(FAILED_MESSAGE);
        }

        public void RemoveItemReference(T itemData)
        {
            if (ItemReference.Equals(itemData))
            {
                _numberOfInstant--;
                OnSuccessfullAddItem?.Invoke(ItemReference);
            }
        }

        public void TryAddItemReference()
        {
            OnTryRemoveItem?.Invoke(ItemReference);
        }

        public void AddItemReference(T ItemData)
        {
            if (ItemReference.Equals(ItemData))
            {
                _numberOfInstant++;
                OnSuccessfullRemoveItem?.Invoke(ItemReference);
            }
        }
    }
}