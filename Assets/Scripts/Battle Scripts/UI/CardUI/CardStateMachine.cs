﻿using System;
using UnityEngine;
using System.Collections.Generic;

namespace Battles.UI.CardUIAttributes
{
    public class CardStateMachine : ITouchable
    {
        public enum CardUIInput
        {
            Locked = 0,
            Hand = 1,
            Zoomed = 2,
            Hold = 3,
            None = 4
        };
 
        public CardUI CardReference { get; set; }

        private RectTransform _rect;
         Dictionary<CardUIInput, CardUIAbstractState> _statesDictionary;
        private ITouchable _currentState;
        public static Vector2 TouchPos { get; set; }
        public ITouchable CurrentState
        {
            get => _currentState;
            set
            {
                if (_currentState != null)
                    _currentState.OnStateExit();

                _currentState = value;
                CardReference.startState = (_currentState != null ) ? _currentState.State : CardUIInput.None;
                if (_currentState != null)
                    _currentState.OnStateEnter();
            }
        }
        public void MoveToState (CardUIInput cardUIInput)
        {
            Debug.LogWarning($"Current State is: {CurrentState?.State}\nChanging state to: {cardUIInput}");
            CurrentState = _statesDictionary[cardUIInput];
        }

        internal T GetState<T>(CardUIInput cardUIInput) where T : CardUIAbstractState
        {
            return _statesDictionary[cardUIInput] as T;
        }

        public CardStateMachine(CardUI cardUI ,CanvasGroup canvasGroup, CardUIInput firstState)
        {
            CardReference = cardUI;
            _rect = cardUI.GFX.GetRectTransform;

            const int states = 5;
            _statesDictionary = new Dictionary<CardUIInput, CardUIAbstractState>(states)
            {
                { CardUIInput.None, null },
                { CardUIInput.Locked, null },
                { CardUIInput.Hand, new HandState(_rect, this) },
                {CardUIInput.Hold, new HoldState(CardReference,this) },
                {CardUIInput.Zoomed, new ZoomState(_rect,this)}
            };

            _currentState = _statesDictionary[firstState];
        }


        #region Interface Implementation
    

        public RectTransform Rect => _rect;

        public bool IsInteractable => (CurrentState.State == CardUIInput.Locked || CurrentState.State== CardUIInput.None) ? false : CurrentState.IsInteractable;

        public CardUIInput State => CurrentState.State;

        public void OnMouse()
        {
            CurrentState?.OnMouse();
        }

        public void OnStateEnter()
        {
            CurrentState?.OnStateEnter();
        }

        public void OnStateExit()
        {
            CurrentState?.OnStateExit();
        }

        public void OnTick(in Touch touchPos)
        {
            CurrentState?.OnTick(touchPos);
        }

        public void ResetTouch()
        {
            CurrentState?.ResetTouch();
        }

        #endregion

    }



    public abstract class CardUIAbstractState : ITouchable
    {
        protected CardStateMachine _cardStateMachine;
        public abstract CardStateMachine.CardUIInput State { get; }
        public RectTransform Rect { get; private set; }
        public virtual bool IsInteractable => State != CardStateMachine.CardUIInput.None && State != CardStateMachine.CardUIInput.Locked;



        public CardUIAbstractState(RectTransform rectm , CardStateMachine cardStateMachine)
        {
            Rect = rectm;
            _cardStateMachine = cardStateMachine;
        }


        public abstract void OnMouse();
        public virtual void OnStateEnter() { }
        public virtual void ResetTouch() { }
        public virtual void OnStateExit() { }
        public abstract void OnTick(in Touch touchPos);
    }

    public class HandState : CardUIAbstractState
    {
        public HandState(RectTransform rectm, CardStateMachine cardStateMachine) : base(rectm, cardStateMachine)
        {
        }

        public override CardStateMachine.CardUIInput State => CardStateMachine.CardUIInput.Hand;

        public bool HasValue { get; set; }
        public int Index { get; internal set; }

        public override void OnMouse()
        {

        }


        public override void OnTick(in Touch touchPos)
        {
            //   var _touchPosOnScreen = CameraController.Instance.GetTouchPositionOnUIScreen(_playerTouch.Value.position);
            //Debug.Log(touchPos.position);
            //Debug.Log(touchPos.deltaPosition);

            switch (touchPos.phase)
            {
                case TouchPhase.Began:
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    Debug.Log("CardUI - State Hand - Began Touch ");
                    CardStateMachine.TouchPos =touchPos.position;
                    var card = _cardStateMachine.CardReference;
                    CardUIHandler.Instance.CardUITouched(card);
                 //   currentTime += Time.deltaTime;      
                    _cardStateMachine.MoveToState(CardStateMachine.CardUIInput.Hold);
                    break;


                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    Debug.Log("CardUI - State Hand -  End Touch");
                    CardUIHandler.Instance.CardUITouchedReleased(false,_cardStateMachine.CardReference);
                    break;
                default:
                    break;
            }
       
        }

        public override void OnStateExit()
        {
        //currentTime = 0;
        }
        public override void OnStateEnter()
        {
            _cardStateMachine.CardReference.Inputs.GetCanvasGroup.alpha = 1f;
        }
    }












}