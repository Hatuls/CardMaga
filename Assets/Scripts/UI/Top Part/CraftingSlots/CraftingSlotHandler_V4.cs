using System.Collections;
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

        [Header("Test")] 
        [SerializeField] private CardTypeData _card;
        
        [SerializeField] private TransitionPackSO _transition;
        
        [SerializeField] private CraftingSlotsUI_V4[] _craftingSlot;
        [SerializeField] private BodyPartBaseVisualSO _baseVisual;
        [SerializeField] private CraftingSlotDefaultSO _defaultCraftingSlotData;
        
        [Header("RectTransforms")]
        [SerializeField] private RectTransform _slotsGroup;
        
        private float _xMovement;
        private int _currentSlot;

        private CraftingSlotData _craftingSlotData;

        #endregion

        #region UnityCallback

        private void Start()
        {
            for (int i = 0; i < _craftingSlot.Length; i++)
            {
                _craftingSlot[i].Init(_defaultCraftingSlotData);
            }
            
            GetDistance();
        }
        private void OnDestroy()
        {
            StopAllCoroutines();
        }
        #endregion

        #region PublicFunctions

        public void CancelLoadSlot(BattleCardUI battleCardUI)
        {
            if (_craftingSlot[1].TryGetCardTypeData(out CraftingSlotData prevCraftingSlotData))
            {
                FullStopLoading();
            }
            else
            {
                SingleStopLoading();
            }
        }

        public void ApplySlot()
        {
            if (_craftingSlot[1].TryGetCardTypeData(out CraftingSlotData prevCraftingSlotData))
            {
                FinishFullAnimation();
            }
            else
            {
                FinishSingleAnimation();
            }
        }
        
        public void ApplyEnemySlot(BattleCardData battleCardData)
        {
     
            _craftingSlotData = AssignCraftingSlotData(battleCardData.CardTypeData);
            
            AddCraftingSlot();
            
            if (_craftingSlot[1].TryGetCardTypeData(out CraftingSlotData prevCraftingSlotData))
            {
                ApplySlot();
            }
            else
            {
                ApplySlot();
            }
        }
        

        public void LoadCraftingSlot(BattleCardUI battleCardUI)
        {
            _craftingSlotData = AssignCraftingSlotData(battleCardUI.BattleCardData.CardTypeData);

            if (_craftingSlot[1].TryGetCardTypeData(out CraftingSlotData prevCraftingSlotData))
            {
                LoadCraftingSlot();
            }
            else
            {
                AddCraftingSlot();
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
            
            Sprite bpImage;
            Color bpColor;
            Color bgColor;
            bool isEmpty;
            
            if (cardTypeData.BodyPart == CardMaga.Card.BodyPartEnum.Empty)
            {
                bpImage = null;
                isEmpty = true;
            }
            else
            {
                bpImage = _baseVisual.GetBodyPartSprite(cardTypeData.BodyPart);
                isEmpty = false;
            }
            
            
            bpColor = _baseVisual.GetMainColor(cardTypeData.CardType);
            bgColor = _baseVisual.GetInnerColor(cardTypeData.CardType);

            craftingSlotData.SetCraftingSlotData(bpImage,bpColor,bgColor,isEmpty);
            
            return craftingSlotData;
        }
        
        private void LoadCraftingSlot()
        {
            _craftingSlot[0].LoadSlotData(_craftingSlotData);
            
            StartCoroutine(FullLoaderAlpha());
        }
        
        private IEnumerator FullLoaderAlpha()
        {
            float screenHeight = Screen.height * 0.75f;
            
            while (true)
            {
                float value = InputReciever.Instance.TouchPosOnScreen.y / screenHeight;
                
                _craftingSlot[0].CanvasGroup.alpha = value;
                _craftingSlot[_craftingSlot.Length - 1].CanvasGroup.alpha = 1- value;
                _slotsGroup.anchoredPosition = new Vector3(Mathf.Lerp(0, _xMovement, value), 0,0);
                yield return null;
            }
        }

        private IEnumerator SingleLoaderAlpha(CraftingSlotData craftingSlotData)
        {
            float screenHeight = Screen.height * 0.75f;
            
            while (true)
            {
                float value = InputReciever.Instance.TouchPosOnScreen.y / screenHeight;
                
                _craftingSlot[_currentSlot].CanvasGroup.alpha = value;
                yield return null;
                _craftingSlotData = craftingSlotData;
            }
        }



        private void ChangeSlots()
        {
            MoveCraftingSlot();
            ResetPosition();
        }

        private void FullStopLoading()
        {
            _craftingSlot[0].RestCraftingSlot();
            ResetPosition();
            StopAllCoroutines();
        }

        private void SingleStopLoading()
        {
            StopAllCoroutines();
            _craftingSlot[_currentSlot].RestCraftingSlot();
            _craftingSlot[_currentSlot].CanvasGroup.alpha = 1;
        }

        private void FinishFullAnimation()
        {
            StopAllCoroutines();
            _craftingSlot[0].ApplyCraftingData();
            _craftingSlot[0].CanvasGroup.DOFade(1, _transition.Movement.TimeToTransition);
            _craftingSlot[_craftingSlot.Length - 1].CanvasGroup.DOFade(0, _transition.Movement.TimeToTransition);
            Vector2 destination = new Vector2(_xMovement, 0);
            _slotsGroup.Transition(destination,_transition,ChangeSlots);
        }

        private void FinishSingleAnimation()
        {
            StopAllCoroutines();
            _craftingSlot[_currentSlot].ApplyCraftingData();
            _craftingSlot[_currentSlot].CanvasGroup.DOFade(1, _transition.Movement.TimeToTransition);
        }

        private void ResetPosition()
        {
            _craftingSlot[0].CanvasGroup.alpha = 0;
            _craftingSlot[_craftingSlot.Length - 1].CanvasGroup.alpha = 1;
            _slotsGroup.anchoredPosition = Vector2.zero;
            _craftingSlot[0].RestCraftingSlot();
        }
        
        private void AddCraftingSlot()
        {
            for (int i = 0; i < _craftingSlot.Length; i++)
            {
                if (_craftingSlot[i].TryGetCardTypeData(out CraftingSlotData prevCraftingSlotData))
                {
                    _currentSlot = i - 1;
                    LoadSingleSlot(_craftingSlotData);
                    return;
                }
            }
            _currentSlot = _craftingSlot.Length - 1;
            LoadSingleSlot(_craftingSlotData);
        }

        private void LoadSingleSlot(CraftingSlotData craftingSlotData)
        {
            _craftingSlot[_currentSlot].LoadSlotData(craftingSlotData);
            StopCoroutine(SingleLoaderAlpha(craftingSlotData));
            StartCoroutine(SingleLoaderAlpha(craftingSlotData));
        }

        private void MoveCraftingSlot()
        {
            for (int i = _craftingSlot.Length - 2; i >= 0; i--)
            {
                if (_craftingSlot[i].TryGetCardTypeData(out CraftingSlotData craftingSlotData))
                {
                    _craftingSlot[i + 1].LoadSlotData(craftingSlotData);
                    _craftingSlot[i + 1].ApplyCraftingData();
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
        
    }
    
    public class CraftingSlotData
    {
        private Sprite _bodyIcon;
        private Color _bodyPartColor;
        private Color _bgColor;
        private bool _isEmpty;

        public bool IsEmpty
        {
            get => _isEmpty;
        }
        
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

        public void SetCraftingSlotData(Sprite bodyIcon, Color bodyPartColor,Color bgColor,bool isEmpty)
        {
            _bodyIcon = bodyIcon;
            _bodyPartColor = bodyPartColor;
            _bgColor = bgColor;
            _isEmpty = isEmpty;
        }
    }
}