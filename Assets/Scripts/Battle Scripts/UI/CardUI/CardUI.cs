using System;
using Unity.Events;
using UnityEngine;
using Battles.UI.CardUIAttributes;
using UnityEngine.EventSystems;

namespace Battles.UI
{
    public class CardUI : MonoBehaviour, IEquatable<CardUI> , IPoolable<CardUI>
    {
        #region Fields

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CardGFX _cardGFX;
        [SerializeField] private CanvasGroup _canvasGroup;
        private CardAnimator _cardAnimator;
        
        [SerializeField] private CardUIInputHandler _inputs;
        private RectTransitionManager _cardTransitionManager;
        

        #region Enum Selection
        
        [Sirenix.OdinInspector.OnValueChanged("CardUISettingsEnum")]
        [Sirenix.OdinInspector.EnumToggleButtons]
        
        
        #endregion
     
        #endregion
        
        private void Awake()
        {
            _cardTransitionManager = new RectTransitionManager(_rectTransform);
        }

        
        public bool Equals(CardUI other)
        {
            return other.RecieveCardReference().CardInstanceID == _cardGFX.GetCardReference.CardInstanceID;
        }
        
        public  CardGFX GFX =>  _cardGFX;

        public  CardUIInputHandler Inputs
        {
            get
            {
                return  _inputs;
            }
        }

        public  RectTransitionManager CardTransitionManager {
            get
            {
                return  _cardTransitionManager;
            }
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
    
public static class CardUIHelper {
        public static void AssignData(this CardUI cardUI, Cards.Card card) => cardUI.GFX.SetCardReference(card);
        public static Cards.Card RecieveCardReference(this CardUI cardui) => cardui.GFX.GetCardReference;
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
        _animator.SetTrigger((value) ? AnimatorParameters.ZoomOutAnimation :  AnimatorParameters.ZoomInAnimation);
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


