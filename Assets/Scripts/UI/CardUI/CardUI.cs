using System;
using Unity.Events;
using UnityEngine;
using Battles.UI.CardUIAttributes;
using UnityEngine.EventSystems;

namespace Battles.UI
{

    public class CardUI : MonoBehaviour, IInputAbleObject
    {
        #region Fields
        [SerializeField]
        CardUISO _settings;
        public CardUISO Settings { get => _settings; }

        [SerializeField]
        EventTrigger _eventTrigger;
        [SerializeField]

        internal CardStateMachine.CardUIInput startState = CardStateMachine.CardUIInput.Locked;
        [SerializeField]
   //     [HideInInspector]
        private CardGFX _cardGFX;


       [HideInInspector]
        [SerializeField]
        private CanvasGroup _canvasGroup;
        private CardAnimator _cardAnimator;
        private CardInputs _inputs;

        private CardTranslations _cardTranslations;

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
            //Debug.Log(Card);
            //Debug.Log((byte)Card);
            //Debug.Log(Card.HasFlag(CardUISettings.Default));
            //Debug.Log(Card.HasFlag(CardUISettings.Image));

            if ((Card & CardUISettings.Visable) != CardUISettings.Visable)
                Card |= CardUISettings.Visable;


        }


        #endregion
     
        #endregion


        #region Events
        [Space]
        [Header("Events")]
        [SerializeField] CardUIEvent _zoomCardEvent;
        [SerializeField] CardUIEvent _selectCardEvent;
        #endregion

        private void Start()
        {
            if (_cardAnimator == null &&
               ((Card & CardUISettings.With_Animations) == CardUISettings.With_Animations))
                GetComponent<Animator>().enabled = true;
        }

        #region Properties
        public  CardGFX GFX =>  _cardGFX;

        public  CardInputs Inputs
        {
            get
            {
                if (_inputs == null && (Card & CardUISettings.Touchable) == CardUISettings.Touchable)
                    _inputs = new CardInputs( _canvasGroup, _eventTrigger, startState, this);
                return  _inputs;
            }
        }

        public  CardTranslations CardTranslations {
            get
            {
                if (_cardTranslations == null &&
                    ((Card & CardUISettings.Moveable) == CardUISettings.Moveable))
                    _cardTranslations = new CardTranslations( _cardGFX.GetRectTransform);

                return  _cardTranslations;
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
        public ITouchable GetTouchAbleInput => ((Card & CardUISettings.Touchable) == CardUISettings.Touchable) ? Inputs.CardStateMachine.CurrentState : null;
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
    }

    public void ScaleAnimation(bool value)
    {
        _animator.SetBool(AnimatorParameters.ZoomAnimation, value);
    }




    public static class AnimatorParameters
    {
        public static int ZoomAnimation = Animator.StringToHash("ToZoom");
        public static int ResetAllAnimation = Animator.StringToHash("ResetAll");
        public static int NoticeAnimation = Animator.StringToHash("CardUI_Notice");
    }
    internal void PlayNoticeAnimation()
    {
        _animator.Play(AnimatorParameters.NoticeAnimation);
    }


    internal void ResetAllAnimations()
    {
        _animator.SetTrigger(AnimatorParameters.ResetAllAnimation);
    }
}


