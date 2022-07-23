using System;
using Battle.Deck;
using CardMaga.Card;
using CardMaga.UI.Visuals;
using UnityEngine;
using UnityEngine.UI;


namespace CardMaga.UI.Carfting
{
    public class CraftingSlotHandler_V4 : MonoBehaviour
    {
        #region Fields

        [SerializeField] private CraftingSlotsUI_V4[] _craftimgSlot;
        [SerializeField] private BodyPartBaseVisualSO _baseVisual;
        [SerializeField] private CraftingSlotDefaultSO _defaultCraftingSlotData;

        #endregion

        private void Awake()
        {
            for (int i = 0; i < _craftimgSlot.Length; i++)
            {
                _craftimgSlot[i].Init();
            }
        }

        private CraftingSlotData AssignCraftingSlotData(CardTypeData cardTypeData)
        {
            CraftingSlotData craftingSlotData = new CraftingSlotData();

            var bpImage = _baseVisual.GetBodyPartSprite(cardTypeData.BodyPart);
            var bpColor = _baseVisual.GetMainColor(cardTypeData.CardType);
            var bgColor = _baseVisual.GetInnerColor(cardTypeData.CardType);

            craftingSlotData.SetCraftingSlotData(bpImage,bpColor,bgColor);
            
            return craftingSlotData;
        }

        public void AddCraftingSlot(CardTypeData cardTypeData)
        {
            CraftingSlotData craftingSlotData = AssignCraftingSlotData(cardTypeData);
            
            MoveCraftingSlot();
            
            _craftimgSlot[1].AssignSlotData(craftingSlotData);
        }

        private void MoveCraftingSlot()
        {
            for (int i = 1; i < _craftimgSlot.Length - 1; i++)
            {
                if (_craftimgSlot[i].TryGetCardTypeData(out CraftingSlotData cardTypeData))
                {
                    _craftimgSlot[i + 1].AssignSlotData(cardTypeData);
                }
                
                break;
            }
        }

        public void RestCraftingSlots()
        {
            for (int i = 0; i < _craftimgSlot.Length; i++)
            {
                _craftimgSlot[i].RestCraftingSlot();
            }
        }
    }
    
    public class CraftingSlotData
    {
        private Sprite _bodyIcon;
        private Color _bodyPartColor;
        private Color _bgColor;

        public Sprite BodyPartIcon
        {
            get => _bodyIcon;
        }

        public Color BodyPartColor
        {
            get => _bodyPartColor;
        }

        public Color BgColor
        {
            get => _bgColor;
        }

        public void SetCraftingSlotData(Sprite bodyIcon, Color bodyPartColor,Color bgColor)
        {
            _bodyIcon = bodyIcon;
            _bodyPartColor = bodyPartColor;
            _bgColor = bgColor;
        }
    }
}