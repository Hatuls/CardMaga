using CardMaga.MetaData.Collection;
using CardMaga.UI;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace CardMaga.MetaUI.CollectionUI
{
    public abstract class BaseCollectionUIItem : BaseUIElement
    {
        [SerializeField] private Button _minusButton;
        [SerializeField] private Button _plusButton;

        protected void DisableMinus()
        {
            _minusButton.interactable = false; 
        }

        protected void DisablePlus()
        {
            _plusButton.interactable = false;
        }

        protected void Enable()
        {
            _minusButton.interactable = true; 
            _plusButton.interactable = true; 
        }

        protected abstract void SuccessAddOrRemoveCollection();
        
    }

    public interface ICollectionButtonsBehavior
    {
        void OnPlusPress(MetaCollectionCardData cardData);
        void OnMinusPress(MetaCollectionCardData cardData);
    }
}