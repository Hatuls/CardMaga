using System;
using Unity.Events;
using UnityEngine;
using Battles.UI.CardUIAttributes;
using UnityEngine.EventSystems;

namespace Battles.UI
{
    //[Serializable]
    public class CardUI : MonoBehaviour, IInputAbleObject, IEquatable<CardUI>
    {
        #region Fields
        [SerializeField]
        CardUISO _settings;
        public CardUISO Settings { get => _settings; }

        [SerializeField]
        EventTrigger _eventTrigger;
        [SerializeField]
        CardStateMachine _cardStateMachine;


        [SerializeField]
        internal CardStateMachine.CardUIInput startState = CardStateMachine.CardUIInput.Locked;
        [SerializeField]
   //     [HideInInspector]
        private CardGFX _cardGFX;


        [SerializeField]
        private CanvasGroup _canvasGroup;
        private CardAnimator _cardAnimator;
        [SerializeField]
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
        private void Awake()
        {
            if (((Card & CardUISettings.Touchable) != CardUISettings.Touchable))
                Destroy(GetComponent<CardStateMachine>());
        }

        private void Start()
        {
            if (_cardAnimator == null &&
               ((Card & CardUISettings.With_Animations) == CardUISettings.With_Animations))
                GetComponent<Animator>().enabled = true;
        }

        public bool Equals(CardUI other)
        => other.RecieveCardReference() == _cardGFX.GetCardReference;

        #region Properties
        public  CardGFX GFX =>  _cardGFX;

        public  CardInputs Inputs
        {
            get
            {
                if (_inputs == null)
                    _inputs = gameObject.GetComponent<CardInputs>();
                //if (_inputs == null && (Card & CardUISettings.Touchable) == CardUISettings.Touchable)
                //    _inputs = new CardInputs( _canvasGroup, _eventTrigger, startState, this);
                return  _inputs;
            }
        }

        public  CardTranslations CardTranslations {
            get
            {
                if (_cardTranslations == null &&
                    ((Card & CardUISettings.Moveable) == CardUISettings.Moveable))
                    _cardTranslations = new CardTranslations(_cardGFX.GetRectTransform == null ? GetComponent<RectTransform>() :_cardGFX.GetRectTransform);

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
        public CardStateMachine CardStateMachine =>_cardStateMachine;
        public ITouchable GetTouchAbleInput => ((Card & CardUISettings.Touchable) == CardUISettings.Touchable) ? CardStateMachine.CurrentState : null;
        #endregion

    
    }

   
   
public static class CardUIHelper {
        public static void DisplayCard(this CardUI cardUI, Cards.Card card) => cardUI.GFX.SetCardReference(card);
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


