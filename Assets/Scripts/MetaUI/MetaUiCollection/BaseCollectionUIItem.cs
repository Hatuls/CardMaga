using System;
using CardMaga.UI;
using UnityEngine;

namespace CardMaga.MetaUI.CollectionUI
{
    public abstract class BaseCollectionUIItem<T> : BaseUIElement
    {
        public event Action OnTryAddToDeck;
        public event Action OnTryRemoveFromDeck;

        [SerializeField] private DisableButton _minsButton;
        [SerializeField] private DisableButton _plusButton;

        public virtual void TryAddToCollection()
        {
           OnTryAddToDeck?.Invoke();
        }

        public virtual void TryRemoveFromCollection()
        {
            OnTryRemoveFromDeck?.Invoke();
        }

        protected void DisableMins()
        {
            _minsButton.Disable();
        }

        protected void DisablePlus()
        {
            _plusButton.Disable();
        }

        protected void Enable()
        {
            _minsButton.Enable();
            _plusButton.Enable();
        }

        public abstract void SuccessAddToCollection(T itemData);

        public abstract void SuccessRemoveFromCollection(T itemData);
    }
}