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

        [SerializeField] private TransitionPackSO _transition;
        
        [SerializeField] private CraftingSlotsUI_V4[] _craftingSlot;
        [SerializeField] private BodyPartBaseVisualSO _baseVisual;
        [SerializeField] private CraftingSlotDefaultSO _defaultCraftingSlotData;
        
        [Header("CanvasGroups And RectTransforms")]
        [SerializeField] private CanvasGroup _loaderCanvasGroup;
        [SerializeField] private CanvasGroup _unLoadCanvasGroup;
        [SerializeField] private RectTransform _slotsGroup;
        
        private float _xMovement;

        private CraftingSlotData _craftingSlotData;

        #endregion

        #region UnityCallback

        private void Awake()
        {
            for (int i = 0; i < _craftingSlot.Length; i++)
            {
                _craftingSlot[i].Init(_defaultCraftingSlotData);
            }

            HandUI.OnCardSelect += LoadCraftingSlot;
            HandUI.OnCardReturnToHand += StopLoading;
            
            GetDistance();
        }

        private void OnDestroy()
        {
            HandUI.OnCardSelect -= LoadCraftingSlot;
            HandUI.OnCardReturnToHand -= StopLoading;
        }
        
        #endregion

        #region PublicFunctions

        public void LoadCraftingSlot(CardUI cardTypeDataUI)
        {
            LoadCraftingSlot(cardTypeDataUI.CardData.CardTypeData);
        }
        
        public void LoadCraftingSlot(CardTypeData cardTypeData)
        {
            CraftingSlotData craftingSlotData = AssignCraftingSlotData(cardTypeData);

            if (_craftingSlot[1].TryGetCardTypeData(out CraftingSlotData prevCraftingSlotData))
            {
                LoadCraftingSlot(craftingSlotData);
            }
            else
            {
                AddCraftingSlot(craftingSlotData);
            }
        }

        

        public void RestCraftingSlots()
        {
            for (int i = 0; i < _craftingSlot.Length; i++)
            {
                _craftingSlot[i].RestCraftingSlot();
            }
        }

        #endregion

        #region PrivateFunctions

        private void GetDistance()
        {
            _xMovement = -Vector2.Distance(_craftingSlot[0].RectTransform.anchoredPosition,
                _craftingSlot[1].RectTransform.anchoredPosition);
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
        
        private void LoadCraftingSlot(CraftingSlotData craftingSlotData)
        {
            _craftingSlot[0].AssignSlotData(craftingSlotData);
            
            StartCoroutine(LoaderAlpha(craftingSlotData));
        }
        
        private IEnumerator LoaderAlpha(CraftingSlotData craftingSlotData)
        {
            float screenHeight = Screen.height;
            
            while (true)
            {
                float value = InputReciever.TouchPosOnScreen.y / screenHeight;
                
                _loaderCanvasGroup.alpha = value;
                _unLoadCanvasGroup.alpha = 1- value;
                _slotsGroup.anchoredPosition = new Vector3(Mathf.Lerp(0, _xMovement, value), 0,0);
                _craftingSlotData = craftingSlotData;
                if (!InputReciever.IsTouching)
                {
                    FinishAnimation();
                }
                yield return null;
            }
        }

        private void ChangeSlots()
        {
            MoveCraftingSlot();
            AddCraftingSlot(_craftingSlotData);
            ResetPosition();
        }

        private void StopLoading(CardUI cardUI)
        {
            _craftingSlot[0].RestCraftingSlot();
            StopAllCoroutines();
        }

        private void FinishAnimation()
        {
            StopAllCoroutines();
            _loaderCanvasGroup.DOFade(1, _transition.Movement.TimeToTransition);
            _unLoadCanvasGroup.DOFade(0, _transition.Movement.TimeToTransition);
            Vector2 destination = new Vector2(_xMovement, 0);
            _slotsGroup.Transition(destination,_transition,ChangeSlots);
        }

        private void ResetPosition()
        {
            _loaderCanvasGroup.alpha = 0;
            _unLoadCanvasGroup.alpha = 1;
            _slotsGroup.anchoredPosition = Vector2.zero;
        }
        
        private void AddCraftingSlot(CraftingSlotData craftingSlotData)
        {
            for (int i = 1; i < _craftingSlot.Length; i++)
            {
                if (_craftingSlot[i].TryGetCardTypeData(out CraftingSlotData prevCraftingSlotData))
                {
                    if (i == 1)
                    {
                        _craftingSlot[i].AssignSlotData(craftingSlotData);
                        return;
                    }
                    _craftingSlot[i - 1].AssignSlotData(craftingSlotData);
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
        [Sirenix.OdinInspector.Button]
        private void ResetAllCraftingSlots()
        {
            for (int i = 0; i < _craftingSlot.Length; i++)
            {
                if (_craftingSlot[i].TryGetCardTypeData(out CraftingSlotData craftingSlotData))
                {
                    _craftingSlot[i].RestCraftingSlot();
                }
            }
        }

        #endregion
        
        [Sirenix.OdinInspector.Button]
        public void Test()
        {
            LoadCraftingSlot(_card);
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