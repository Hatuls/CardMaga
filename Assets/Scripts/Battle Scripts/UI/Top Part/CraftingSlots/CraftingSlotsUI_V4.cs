using System;
using CardMaga.Card;
using CardMaga.UI.Visuals;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.Carfting
{
    [Serializable]
    public class CraftingSlotsUI_V4 : BaseVisualAssigner
    {
        #region Fields
        
        [SerializeField] private RectTransform _transform;
        [SerializeField] private Image _bodyIcon;
        [SerializeField] private Image _bgImage;
        private CraftingSlotData _craftingSlotData;
        private CraftingSlotData _defaultCraftingSlotData;
        
        #endregion


        #region prop

        public CraftingSlotData CraftingSlotData
        {
            get => _craftingSlotData;
        }

        #endregion

        public override void Init()
        {
             
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
            
        }
    }
}