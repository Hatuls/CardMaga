using System;
using CardMaga.UI;

namespace CardMaga.MetaUI.CollectionUI
{
    public abstract class BaseCollectionItemUI<T> : BaseUIElement
    {
        public event Action OnTryAddToDeck; 
        public event Action OnTryRemoveFromDeck;

        public virtual void TryAddToDeck()
        {
            OnTryAddToDeck?.Invoke();
        }

        public virtual void TryRemoveFromDeck()
        {
            OnTryRemoveFromDeck?.Invoke();
        }

        public abstract void SuccessAddToDeck(T metaCardData);

        public abstract void SuccessRemoveFromDeck(T metaCardData);
    }
}