using CardMaga.UI;
using Cards;
using System;
using UnityEngine;

namespace Battles.UI
{
    public class CardUI : MonoBehaviour, IPoolable<CardUI>
    {
        #region Fields

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private BaseCardVisualHandler _cardVisuals;

        [SerializeField] private CardUIInputHandler _inputs;


        private RectTransitionManager _cardTransitionManager;
        private CardAnimator _cardAnimator;
        private Card _cardData;


        #endregion
        public BaseCardVisualHandler CardVisuals => _cardVisuals;
        public CardUIInputHandler Inputs => _inputs;
        
        public RectTransitionManager CardTransitionManager
        {
            get
            {
                if (_cardTransitionManager == null)
                    _cardTransitionManager = new RectTransitionManager(_rectTransform);
                return _cardTransitionManager;
            }
        }

        public Card CardData { get => _cardData; private set => _cardData = value; }


        public void AssignCard(Card card)
        {
            CardData = card;
            CardVisuals.SetCardVisuals(card);
        }

        #region Ipoolable Implementation

        public void Dispose()
        {
            OnDisposed?.Invoke(this);
            gameObject.SetActive(false);
        }
        public event Action<CardUI> OnDisposed;
        public void Init()
        {
            gameObject.SetActive(true);
        }

        #endregion

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
        _animator.SetTrigger((value) ? AnimatorParameters.ZoomOutAnimation : AnimatorParameters.ZoomInAnimation);
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


