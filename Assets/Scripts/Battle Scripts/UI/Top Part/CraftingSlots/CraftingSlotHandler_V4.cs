using System;
using System.Collections;
using Battle.Deck;
using CardMaga.Card;
using CardMaga.UI.Card;
using CardMaga.UI.Visuals;
using UnityEngine;
using UnityEngine.UI;


namespace CardMaga.UI.Carfting
{
    public class CraftingSlotHandler_V4 : MonoBehaviour
    {
        #region Fields

        [Header("Test")] [SerializeField] private CardTypeData _card;
        
        [SerializeField] private CraftingSlotsUI_V4[] _craftingSlot;
        [SerializeField] private BodyPartBaseVisualSO _baseVisual;
        [SerializeField] private CraftingSlotDefaultSO _defaultCraftingSlotData;
        [SerializeField] private CanvasGroup _loaderCanvasGroup;

        #endregion

        private void Awake()
        {
            for (int i = 0; i < _craftingSlot.Length; i++)
            {
                _craftingSlot[i].Init();
            }

            HandUI.OnCardSelect += LoadCraftingSlot;
        }
        
        [Sirenix.OdinInspector.Button]
        private void Test()
        {
            LoadCraftingSlot(_card);
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

        public void LoadCraftingSlot(CardUI cardTypeDataUI)
        {
            CraftingSlotData craftingSlotData = AssignCraftingSlotData(cardTypeDataUI.CardData.CardTypeData);
            
            _craftingSlot[0].AssignSlotData(craftingSlotData);
            
            
        }

        private IEnumerator LoaderAlpha()
        {
            while (true)
            {
                
                yield return null;
            }
        }
        
        public void LoadCraftingSlot(CardTypeData cardTypeData)
        {
            CraftingSlotData craftingSlotData = AssignCraftingSlotData(cardTypeData);
            
            _craftingSlot[0].AssignSlotData(craftingSlotData);
        }
        

        private void AddCraftingSlot(CraftingSlotData craftingSlotData)
        {
            for (int i = 0; i < _craftingSlot.Length; i++)
            {
                if (_craftingSlot[i].TryGetCardTypeData(out CraftingSlotData prevCraftingSlotData))
                {
                    if (i == 1)
                    {
                        MoveCraftingSlot();
                        _craftingSlot[i].AssignSlotData(craftingSlotData);
                        return;
                    }
                    _craftingSlot[i-1].AssignSlotData(craftingSlotData);
                    return;
                }
            }
            _craftingSlot[_craftingSlot.Length - 1].AssignSlotData(craftingSlotData);
        }

        private void MoveCraftingSlot()
        {
            for (int i = _craftingSlot.Length -1; i > 0; i--)
            {
                if (_craftingSlot[i].TryGetCardTypeData(out CraftingSlotData craftingSlotData))
                {
                    if (i == _craftingSlot.Length - 1)
                    {
                        _craftingSlot[i].RestCraftingSlot();
                        continue;
                    }
                    _craftingSlot[i + 1].AssignSlotData(craftingSlotData);
                }
            }
        }

        public void RestCraftingSlots()
        {
            for (int i = 0; i < _craftingSlot.Length; i++)
            {
                _craftingSlot[i].RestCraftingSlot();
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