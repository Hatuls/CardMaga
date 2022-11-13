using System;
using CardMaga.Card;
using UnityEngine;
using CardMaga.Input;
using CardMaga.Tools.Pools;

namespace CardMaga.UI.Card
{
    public class BattleCardUI : MonoBehaviour, IPoolableMB<BattleCardUI> , IUIElement, IVisualAssign<BattleCardData>
    {
        public event Action<BattleCardUI> OnDisposed;
        public event Action OnShow;
        public event Action OnHide;
        public event Action OnInitializable;

        #region Fields

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private RectTransform _visualsRectTransform;
        [SerializeField] private BaseCardVisualHandler _cardVisuals;

        [SerializeField] private CardUIInputHandler _inputs;

        private CardAnimator _cardAnimator;
        private BattleCardData _battleCardData;
        
        #endregion
        
        public BaseCardVisualHandler CardVisuals => _cardVisuals;
        public CardUIInputHandler Inputs => _inputs;
        public RectTransform RectTransform => _rectTransform;
        public RectTransform VisualsRectTransform => _visualsRectTransform;

        public BattleCardData BattleCardData { get => _battleCardData; private set => _battleCardData = value; }
        
        
        public void AssignVisual(BattleCardData data)
        {
            BattleCardData = data;
            CardVisuals.Init(data);
        }

        #region Ipoolable Implementation

        public void Dispose()
        {
            Hide();
            OnDisposed?.Invoke(this);
        }
 
        public void Init()
        {
            OnInitializable?.Invoke();
            Show();
        }

        #endregion
        
        public void Show()
        {
            OnShow?.Invoke();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            OnHide?.Invoke();
            if(gameObject.activeSelf)
             gameObject.SetActive(false);
        }
    }
}

public class CardAnimator
{
    Animator _animator;
    RectTransform _rect;

    float _scaleAmount = 1;
    public CardAnimator(RectTransform rect)
    {
        _rect = rect;
        _animator = _rect.GetComponent<Animator>();
        _animator.enabled = true;
    }

    public void ScaleAnimation(bool value)
    {
        _animator.SetTrigger(value ? AnimatorParameters.ZoomOutAnimation : AnimatorParameters.ZoomInAnimation);
    }

    public static class AnimatorParameters
    {
        public static int ZoomInAnimation = Animator.StringToHash("ToZoomIn");
        public static int ZoomOutAnimation = Animator.StringToHash("ToZoomOut");
        public static int ResetAllAnimation = Animator.StringToHash("ResetAll");
        public static int NoticeAnimation = Animator.StringToHash("ToNotice");
    }
    internal void PlayNoticeAnimation()
    {
        _animator.SetTrigger(AnimatorParameters.NoticeAnimation);
    }

    internal void ResetAllAnimations()
    {
        _animator.SetTrigger(AnimatorParameters.ResetAllAnimation);
    }
}


