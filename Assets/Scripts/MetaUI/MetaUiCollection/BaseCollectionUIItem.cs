using System;
using CardMaga.UI;

namespace CardMaga.MetaUI.CollectionUI
{
    public abstract class BaseCollectionUIItem<T> : BaseUIElement
    {
        public event Action OnTryAddToDeck; 
        public event Action OnTryRemoveFromDeck;

        public virtual void TryAddToCollection()
        {
            OnTryAddToDeck?.Invoke();
        }

        public virtual void TryRemoveFromCollection()
        {
            OnTryRemoveFromDeck?.Invoke();
        }

        public abstract void SuccessAddToCollection(T itemData);

        public abstract void SuccessRemoveFromCollection(T itemData);
    }
}