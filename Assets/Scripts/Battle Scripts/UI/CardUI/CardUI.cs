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
        [SerializeField] private CardGFX _cardGFX;
   
        [SerializeField]
        private CanvasGroup _canvasGroup;
        private CardAnimator _cardAnimator;
        [SerializeField]
        private CardUIInputHandler _inputs;
        [SerializeField]
        private RectTransitionManager _cardTransitionManager;

        #region Enum Selection
        [Flags]
        private enum CardUISettings
        {
            [HideInInspector]
            None = 0,

            Visable = 1 << 0,

            Touchable = 1 << 1,

            Moveable = 1 << 2,

            With_Animations = 1<<3
                /*
                 *  0000 0
                 *  0001 2^0
                 *  0010 2^1
                 *  0100 2^2
                 *  1000 2^3
                 */
               
        };
        [SerializeField]
        [Sirenix.OdinInspector.OnValueChanged("CardUISettingsEnum")]
        [Sirenix.OdinInspector.EnumToggleButtons]

        private CardUISettings Card;

        private void CardUISettingsEnum()
        {
            if ((Card & CardUISettings.Visable) != CardUISettings.Visable)
                Card |= CardUISettings.Visable;
        }


        #endregion
     
        #endregion
        
        private void Start()
        {
            if (_cardAnimator == null &&
               ((Card & CardUISettings.With_Animations) == CardUISettings.With_Animations))
                GetComponent<Animator>().enabled = true;
        }

        public bool Equals(CardUI other)
        {
            return true;
            //other?.RecieveCardReference() == _cardGFX.GetCardReference;
        }

        #region Properties
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
        public CardAnimator CardAnimator
        {
            get
            {
                if (_cardAnimator == null &&
               ((Card & CardUISettings.With_Animations) == CardUISettings.With_Animations))
                    _cardAnimator = new CardAnimator(_cardGFX.GetRectTransform);

                return _cardAnimator;
            }
        }
        #endregion

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


