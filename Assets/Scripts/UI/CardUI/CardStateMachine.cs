using System;
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
        float scaleTimeOffset = 0.35f;
        float currentTime = 0;
        public bool HasValue { get; set; }
        public int Index { get; internal set; }
        private const float StationaryOffset = 30f;
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
                    Debug.Log("CardUI - State Hand - Began Touch ");
                    CardStateMachine.TouchPos =touchPos.position;
                    var card = _cardStateMachine.CardReference;
                    CardUIHandler.Instance.CardUITouched(card);
      
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:

                    if (Vector2.Distance(CardStateMachine.TouchPos, touchPos.position) > StationaryOffset)
                    {
                        _cardStateMachine.MoveToState(CardStateMachine.CardUIInput.Hold);
                        Debug.Log($"CardUI - State Hand - Moved Touch\n First Touch = { CardStateMachine.TouchPos}\nCurrent Touch Position = {touchPos.position} ");
                    }
                    else if(currentTime > scaleTimeOffset)
                    {
                        Debug.Log($"Time Exceeds! {currentTime} / {scaleTimeOffset}");
                        _cardStateMachine.MoveToState(CardStateMachine.CardUIInput.Zoomed);
                    }
                    

                    Debug.Log("CardUI - State Hand - Stationary Touch ");
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    Debug.Log("CardUI - State Hand -  End Touch");
                    CardUIHandler.Instance.CardUITouchedReleased(false,_cardStateMachine.CardReference);
                    break;
                default:
                    break;
            }
            currentTime += Time.deltaTime;
        }

        public override void OnStateExit()
        {
            currentTime = 0;
        }
        public override void OnStateEnter()
        {
            currentTime = 0;
        }
    }




















    //public abstract class InputStateAbst : ITouchable
    //{
    //    protected CardInputs _cardInputHandler;
    //    public InputStateAbst(CardInputs cardInputHandler)
    //    {
    //        this._cardInputHandler = cardInputHandler;
    //    }
    //    public RectTransform Rect => _cardInputHandler.Rect;
    //    public bool IsInteractable => _cardInputHandler?.CurrentState != CardInputs.CardUIInput.Locked;

    //    public Touch FirstTouch => throw new System.NotImplementedException();

    //    public virtual void OnMouse() { }
    //    public virtual void OnTick(in Touch touchPos) { }
    //    public virtual void OnStateExit(in Touch touchPos) { }
    //    public virtual void ResetTouch() { _cardInputHandler.GetCanvasGroup.blocksRaycasts = true; }
    //    public virtual void OnStateEnter() { }
    //    public virtual void OnStateExit() { }
    //}

    //public class InHandInputState : InputStateAbst
    //{
    //    public bool HasValue { get; set; }
    //    public int Index { get; set; }
    //    CardUI _cardReference;

    //    public InHandInputState(CardInputs cardInputHandler, CardUIEvent selectCardUI, CardUIEvent zoomCard, float middlePos) : base(cardInputHandler)
    //    {
    //        _cardReference = cardInputHandler.ThisCardUI;
    //    }
    //    public override void OnStateEnter(in Touch touchPos)
    //    {
    //        CardUIHandler.Instance.CardUITouched(_cardReference);
    //    }

    //    public override void OnStateExit(in Touch touchPos)
    //    {
    //        CardUIHandler.Instance.CardUITouchedReleased(_cardReference);
    //        Debug.Log("Released card!");
    //    }

    //}




    //public class OnZoomInputState : InputStateAbst
    //{
    //    bool isZoomed;
    //    float _offset = 50f;
    //    float _middlePos;
    //    public Vector2 OriginalCardPosition { get; set; }
    //    public OnZoomInputState(CardInputs cardInputHandler, CardUIEvent ZoomedEvent, CardUIEvent selectCardEvent, float middlepos) : base(cardInputHandler)
    //    {
    //        _middlePos = middlepos;
    //    }

    //    public override void OnStateEnter(in Touch touchPos)
    //    {
    //        isZoomed = false;
    //        OnTick(touchPos);
    //    }
    //    public override void OnTick(in Touch Touch)
    //    {
    //        // check distance from the card 
    //        // if its close to the original position then zoom
    //        // if its above then move to hold
    //        // if its side ways then remove it

    //        float distance = Mathf.Abs(Touch.position.y) - Mathf.Abs(OriginalCardPosition.y);
    //        Debug.Log(distance);
    //        if (distance < _offset)
    //        {
    //            if (!isZoomed)
    //                CardUIHandler.Instance.ToZoomCardUI();
    //            isZoomed = true;
    //            return;
    //        }
    //        else
    //        {
    //            isZoomed = false;
    //            _cardInputHandler.ThisCardUI.CardTranslations.CancelAllTweens();
    //            _cardInputHandler.CurrentState = CardInputs.CardUIInput.Hold;
    //        }


    //    }

    //    private bool IsAboveTheTouchLine(in Vector2 touchPos) => (touchPos.y >= _middlePos);
    //    public override void OnStateExit(in Touch touchPos)
    //    {
    //        float distanceFromOriginalPoint = Vector2.Distance(OriginalCardPosition, touchPos.position);
    //        isZoomed = false;
    //        if (distanceFromOriginalPoint <= _offset)
    //        {
    //            CardUIHandler.Instance.ToUnZoomCardUI();

    //        }
    //        //else
    //        //{
    //        //    CardUIManager.Instance.CardUITouchedReleased(_cardInputHandler.ThisCardUI);
    //        //    _cardInputHandler.ThisCardUI.CardTranslations.CancelAllTweens();
    //        //}

    //    }

    //}
    //public class OnHoldInputstate : InputStateAbst
    //{
    //    CardUIEvent _selectCardUiEvent;
    //    CardUIEvent _zoomCardUIEvent;
    //    float _middlePos;
    //    bool isRegreting;
    //    public OnHoldInputstate(CardInputs cardInputHandler, CardUIEvent selectCardUiEvent, CardUIEvent zoomCardUIEvent, float middlePos) : base(cardInputHandler)
    //    {
    //        _middlePos = middlePos;
    //        _selectCardUiEvent = selectCardUiEvent;
    //        _zoomCardUIEvent = zoomCardUIEvent;
    //        isRegreting = false;
    //    }


    //    public override void OnTick(in Touch touchPos)
    //    {
    //        CardUIHandler.Instance.DragCard(this._cardInputHandler.ThisCardUI, touchPos.position);
    //        //  if (IsAboveTheTouchLine(touchPos))

    //    }
    //    public override void OnStateExit(in Touch touchPos)
    //    {
    //        if (IsAboveTheTouchLine(touchPos.position))
    //        {
    //            CardUIHandler.Instance.TryExecuteCardUI(_cardInputHandler.ThisCardUI);
    //        }
    //        else
    //        {
    //            CardUIHandler.Instance.CardUITouchedReleased(_cardInputHandler.ThisCardUI);
    //            _cardInputHandler.ThisCardUI.CardTranslations.CancelAllTweens();
    //        }

    //    }


    //    public override void ResetTouch()
    //    {
    //        base.ResetTouch();
    //    }

    //    private bool IsAboveTheTouchLine(in Vector2 touchPos) => (touchPos.y >= _middlePos);
    //}







}