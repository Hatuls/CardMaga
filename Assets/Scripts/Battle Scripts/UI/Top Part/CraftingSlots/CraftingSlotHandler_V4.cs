using System;
using System.Collections;
using Battle.Deck;
using CardMaga.Card;
using CardMaga.UI.Card;
using CardMaga.UI.Visuals;
using UnityEngine;
using DG.Tweening;


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
        [SerializeField] private RectTransform _slotsGroup;

        [Header("Animation Parameters")] [SerializeField]
        private float _xMovement;

        #endregion

        private void Awake()
        {
            for (int i = 0; i < _craftingSlot.Length; i++)
            {
                _craftingSlot[i].Init();
            }

            HandUI.OnCardSelect += LoadCraftingSlot;
            HandUI.OnCardReturnToHand += StopLoading;
        }

        private void OnDestroy()
        {
            HandUI.OnCardSelect -= LoadCraftingSlot;
            HandUI.OnCardReturnToHand -= StopLoading;
        }

        [Sirenix.OdinInspector.Button]
        public void Test()
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
            LoadCraftingSlot(cardTypeDataUI.CardData.CardTypeData);
        }
        
        public void LoadCraftingSlot(CardTypeData cardTypeData)
        {
            CraftingSlotData craftingSlotData = AssignCraftingSlotData(cardTypeData);
            LoadCraftingSlot(craftingSlotData);
        }

        private void LoadCraftingSlot(CraftingSlotData craftingSlotData)
        {
            _craftingSlot[0].AssignSlotData(craftingSlotData);
            StartCoroutine(LoaderAlpha());
        }
        
        private IEnumerator LoaderAlpha()
        {
            float screenHeight = Screen.height;
            
            while (true)
            {
                float value = InputReciever.TouchPosOnScreen.y / screenHeight;
                Debug.Log(InputReciever.TouchPosOnScreen);
                _loaderCanvasGroup.alpha = value;
                _slotsGroup.localPosition = new Vector3(Mathf.Lerp(0, _xMovement, value), 0, 0);
                yield return null;
            }
        }

        public void StopLoading(CardUI cardUI)
        {
            _craftingSlot[0].RestCraftingSlot();
            StopAllCoroutines();
        }
        
        private void AddCraftingSlot(CraftingSlotData craftingSlotData)
        {
            _loaderCanvasGroup.DOFade(1, 1);
            
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