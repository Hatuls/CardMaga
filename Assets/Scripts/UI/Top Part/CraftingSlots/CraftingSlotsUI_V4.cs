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
        private CraftingSlotData _tempCraftingSlotData;
        private CraftingSlotData _defaultCraftingSlotData;
        private bool _isApply = false;
        
        #endregion
        
        #region prop

        public bool IsApply
        {
            get => _isApply;
        }
        
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
                defaultCraftingSlotData._bgColor,
                false
                );
        }

        public void LoadSlotData(CraftingSlotData craftingSlotData)
        {
            _tempCraftingSlotData = craftingSlotData;
            
            _bgImage.color = craftingSlotData.BgColor;
            _bodyIcon.color = craftingSlotData.BodyPartColor;

            if (craftingSlotData.IsEmpty)
            {
                _bodyIcon.enabled = false;
                return;
            }
                
            _bodyIcon.enabled = true;
            _bodyIcon.sprite = craftingSlotData.BodyPartIcon;
        }

        public void ApplyCraftingData()
        {
            _craftingSlotData = _tempCraftingSlotData;
            _tempCraftingSlotData = null;
            _isApply = true;
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
            LoadSlotData(_defaultCraftingSlotData);
            ApplyCraftingData();
            _isApply = false;
            _craftingSlotData = null;
        }
    }
}