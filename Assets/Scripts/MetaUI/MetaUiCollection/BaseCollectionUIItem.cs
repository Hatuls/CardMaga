using System;
using CardMaga.Input;
using CardMaga.UI;
using UnityEngine;

namespace CardMaga.MetaUI.CollectionUI
{
    public abstract class BaseCollectionUIItem : BaseUIElement
    {
        [SerializeField] private DisableButton _minsButton;
        [SerializeField] private DisableButton _plusButton;

        public virtual void TryAddToCollection()
        {
           
        }

        public virtual void TryRemoveFromCollection()
        {
            
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

        protected abstract void SuccessAddOrRemoveCollection();
        
    }
}