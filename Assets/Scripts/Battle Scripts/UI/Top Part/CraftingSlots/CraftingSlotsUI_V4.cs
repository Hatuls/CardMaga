using System;
using CardMaga.Card;
using CardMaga.UI.Visuals;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.Carfting
{
    [Serializable]
    public class CraftingSlotsUI_V4
    {
        #region Fields
        
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _bodyIcon;
        [SerializeField] private Image _bgImage;
        [SerializeField] private CanvasGroup _canvasGroup;
        private CraftingSlotData _craftingSlotData;
        private CraftingSlotData _defaultCraftingSlotData;
        
        #endregion
        
        #region prop

        public CanvasGroup CanvasGroup
        {
            get => _canvasGroup;
        }    
        
        public RectTransform RectTransform
        {
            get => _rectTransform;
        }
        
        public CraftingSlotData CraftingSlotData
        {
            get => _craftingSlotData;
        }

        #endregion

        public void Init(CraftingSlotDefaultSO defaultCraftingSlotData)
        {
            _defaultCraftingSlotData = new CraftingSlotData();
            
            _defaultCraftingSlotData.SetCraftingSlotData(
                defaultCraftingSlotData._bodyIcon,
                defaultCraftingSlotData._bodyPartColor,
                defaultCraftingSlotData._bgColor
                );
        }

        public void AssignSlotData(CraftingSlotData craftingSlotData)
        {
            _craftingSlotData = craftingSlotData;

            _bodyIcon.sprite = craftingSlotData.BodyPartIcon;
            _bodyIcon.color = craftingSlotData.BodyPartColor;
            _bgImage.color = craftingSlotData.BgColor;
        }

        public bool TryGetCardTypeData(out CraftingSlotData cardTypeData)
        {
            if (_craftingSlotData != null)
            {
                cardTypeData = _craftingSlotData;
                return true;
            }
            
            cardTypeData = null;
            return false;
        }

        public void RestCraftingSlot()
        {
            AssignSlotData(_defaultCraftingSlotData);
            _craftingSlotData = null;
        }
    }
}