using CardMaga.Input;
using CardMaga.MetaData.Collection;
using CardMaga.UI;
using UnityEngine;

namespace CardMaga.MetaUI.CollectionUI
{
    public abstract class BaseCollectionUIItem : BaseUIElement
    {
        [SerializeField] private DisableButton _minusButton;
        [SerializeField] private DisableButton _plusButton;

        protected void DisableMinus()
        {
            _minusButton.Disable();
        }

        protected void DisablePlus()
        {
            _plusButton.Disable();
        }

        protected void Enable()
        {
            _minusButton.Enable();
            _plusButton.Enable();
        }

        protected abstract void SuccessAddOrRemoveCollection();
        
    }

    public interface ICollectionButtonsBehavior
    {
        void OnPlusPress(MetaCollectionCardData cardData);
        void OnMinusPress(MetaCollectionCardData cardData);
    }
}