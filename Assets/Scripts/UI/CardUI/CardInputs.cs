using Unity.Events;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Battles.UI.CardUIAttributes
{
    [RequireComponent(typeof(EventTrigger))]
    public class CardInputs : ITouchable , IPointerClickHandler, IBeginDragHandler, IPointerDownHandler,IPointerUpHandler,IPointerExitHandler
    {
        public enum CardUIInput {
            Locked= 0,
            Hand =1,
            Zoomed =2,
            Hold = 3
        };

        public CardUIInput CurrentState
        {
            get => _thisCard._currentState;
            set => _thisCard._currentState = value;
        }
        ITouchable[] _touchables;


        private CanvasGroup _canvasGroup;
        private CardUI _thisCard;
        private EventTrigger _eventTrigger;
        public CanvasGroup GetCanvasGroup => _canvasGroup;

        private EventTrigger.Entry endDrag;
        private EventTrigger.Entry beginDrag;
        private EventTrigger.Entry onClick;
        private EventTrigger.Entry onPointerEnter;



        internal CardUI ThisCardUI => _thisCard;

        public RectTransform Rect =>_rect;

        private RectTransform _rect;

        public CardInputs(CanvasGroup canvasGroup,RectTransform rect, CardUIEvent zoomCardEvent, CardUIEvent selectCardEvent, CardUI card )
        {
            _rect = rect;
            _canvasGroup = canvasGroup;
            _thisCard = card;
            _eventTrigger = card.gameObject.GetComponent<EventTrigger>();

          
            float middlePos = CardUIManager.Instance.GetInputHandLine;

            _touchables = new ITouchable[4]
              {
                null, // <- locked state
                new InHandInputState(this,selectCardEvent ,zoomCardEvent, middlePos),
                new OnZoomInputState(this,zoomCardEvent, selectCardEvent, middlePos),
                new OnHoldInputstate(this,selectCardEvent ,zoomCardEvent,middlePos)
               };



            RegisterInputs();
        }
        ~CardInputs()
        {
            onPointerEnter.callback.RemoveAllListeners();
            beginDrag.callback.RemoveAllListeners();
            endDrag.callback.RemoveAllListeners();
            onClick.callback.RemoveAllListeners();
  
        }
        public void RegisterInputs()
        {
            beginDrag = new EventTrigger.Entry();
            beginDrag.eventID = EventTriggerType.BeginDrag;
            beginDrag.callback.AddListener((data) => { BeginDrag((PointerEventData)data); });
            _eventTrigger.triggers.Add(beginDrag);

            endDrag = new EventTrigger.Entry();
            endDrag.eventID = EventTriggerType.EndDrag;
            endDrag.callback.AddListener((data) => { EndDrag((PointerEventData)data); });
            _eventTrigger.triggers.Add(endDrag);

            onClick = new EventTrigger.Entry();
            onClick.eventID = EventTriggerType.PointerClick;
            onClick.callback.AddListener((data) => { OnPointerClick((PointerEventData)data); });
            _eventTrigger.triggers.Add(onClick);


            onPointerEnter = new EventTrigger.Entry();
            onPointerEnter.eventID = EventTriggerType.PointerEnter;
            onPointerEnter.callback.AddListener((Data) => { BeginDrag((PointerEventData)Data); });
            _eventTrigger.triggers.Add(onPointerEnter);
        }




        public void OnPointerClick(PointerEventData eventData)
        {
            switch (CurrentState)
            {
                case CardUIInput.Locked:
                default:
                    break;
                case CardUIInput.Zoomed:
                case CardUIInput.Hold:
                case CardUIInput.Hand:
                        InputManager.Instance.AssignObjectFromTouch(this);

                    break;

            }
        }

        public void BeginDrag(PointerEventData eventData)
        {
            if (InputManager.inputState != InputManager.InputState.Touch)
            {
                return;
            }
            switch (CurrentState)
            {
                case CardUIInput.Locked:
                default:
                    break;
                case CardUIInput.Zoomed:
                case CardUIInput.Hold:
                case CardUIInput.Hand:
                    if (InputManager.inputState ==  InputManager.InputState.Touch)
                     _canvasGroup.blocksRaycasts = false;

                    InputManager.Instance.AssignObjectFromTouch(this, eventData.position);

                    break;

            }

        }

        public void EndDrag(PointerEventData eventData)
        {
            switch (CurrentState)
            {
                case CardUIInput.Locked:
                default:
                    break;
                case CardUIInput.Zoomed:
                case CardUIInput.Hold:
                case CardUIInput.Hand:
                    if (InputManager.inputState == InputManager.InputState.Touch)
                        _canvasGroup.blocksRaycasts = true;
                    InputManager.Instance.ResetTouch();
                    break;

            }

            Debug.Log("End Touch");
        }






       

        public ITouchable GetTouchable(CardUIInput cardUIInput)
        {
            return _touchables[(int)cardUIInput];
        }

        public void OnFirstTouch(in Vector2 touchPos)
        {
            GetTouchable(CurrentState)?.OnFirstTouch(in touchPos);
        }

        public void OnReleaseTouch(in Vector2 touchPos)
        {
            GetTouchable(CurrentState)?.OnReleaseTouch(in touchPos);
        }

        public void OnHoldTouch(in Vector2 touchPos, in Vector2 startPos)
        {
            GetTouchable(CurrentState)?.OnHoldTouch(in touchPos,in startPos);
        }

        public void ResetTouch()
        {
            GetTouchable(CurrentState)?.ResetTouch();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            BeginDrag(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
         
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            BeginDrag(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
        //    throw new System.NotImplementedException();
        }

        public bool IsInteractable 
            => (CurrentState == CardUIInput.Locked) ? false : GetTouchable(CurrentState).IsInteractable;
    }





    public abstract class InputStateAbst : ITouchable
    {
        protected CardInputs _cardInputHandler;
        public InputStateAbst(CardInputs cardInputHandler)
        {
            this._cardInputHandler = cardInputHandler;
        }

        public RectTransform Rect => _cardInputHandler.Rect;

        public bool IsInteractable => _cardInputHandler?.CurrentState != CardInputs.CardUIInput.Locked;

        public virtual void OnFirstTouch(in Vector2 touchPos) { }
        public virtual void OnHoldTouch(in Vector2 touchPos, in Vector2 startPos) { }
        public virtual void OnReleaseTouch(in Vector2 touchPos) { }
        public virtual void ResetTouch() { _cardInputHandler.GetCanvasGroup.blocksRaycasts = true; }
    }

    public class InHandInputState : InputStateAbst
    {
        CardUIEvent _zoomCardUIEvent;
        CardUIEvent _selectCardUIEvent;

        float _middlePos;
        public InHandInputState(CardInputs cardInputHandler, CardUIEvent selectCardUI, CardUIEvent zoomCard,float middlePos) : base(cardInputHandler)
        {
            _zoomCardUIEvent = zoomCard;
            _selectCardUIEvent = selectCardUI;
            _middlePos = middlePos; 
        }
        public override void OnFirstTouch(in Vector2 touchPos)
        {
            // while in hand our first touch will determine which card we want to do focus on
            // there for we want to lock the other cards and move this card to the next state (zoom)

            if (touchPos.y <= _middlePos)
            {
                _cardInputHandler.CurrentState = CardInputs.CardUIInput.Zoomed;
                _zoomCardUIEvent?.Raise(_cardInputHandler.ThisCardUI);

            }
        }
        public override void OnHoldTouch(in Vector2 touchPos, in Vector2 startPos)
        {

        }
        public override void OnReleaseTouch(in Vector2 touchPos)
        {
            _zoomCardUIEvent.Raise(null);
            Debug.Log("Released card!");
        }

    }


  

    public class OnZoomInputState : InputStateAbst
    {
   
        CardUIEvent _selectCardEvent;
        CardUIEvent _zoomCardEvent;
        float _middlePos;
        public OnZoomInputState(CardInputs cardInputHandler, CardUIEvent ZoomedEvent, CardUIEvent selectCardEvent, float middlepos) : base(cardInputHandler)
        {
            _middlePos = middlepos;
            _zoomCardEvent = ZoomedEvent;
            _selectCardEvent = selectCardEvent;
        }


        public override void OnHoldTouch(in Vector2 touchPos, in Vector2 startPos)
        {
            // change this behaviour
            if (IsAboveTheTouchLine(touchPos))
            {
               _cardInputHandler.CurrentState = CardInputs.CardUIInput.Hold;
            }
            else
            {
                _zoomCardEvent.Raise(_cardInputHandler.ThisCardUI);
            }
        }
        private bool IsAboveTheTouchLine(in Vector2 touchPos) => (touchPos.y >= _middlePos);
        public override void OnReleaseTouch(in Vector2 touchPos)
        {

            if (!IsAboveTheTouchLine(touchPos))
            {
                _cardInputHandler.ThisCardUI.CardTranslations.CancelAllTweens();
                _zoomCardEvent.Raise(null);

            }
        }
        public override void ResetTouch()
        {
            //_selectCardEvent?.Raise(null);
            //_zoomCardEvent?.Raise(null);

            _cardInputHandler.CurrentState = CardInputs.CardUIInput.Hand;
        }
    }
    public class OnHoldInputstate : InputStateAbst
    {
        CardUIEvent _selectCardUiEvent;
        CardUIEvent _zoomCardUIEvent;
        float _middlePos;
        bool isRegreting;
        public OnHoldInputstate(CardInputs cardInputHandler,CardUIEvent selectCardUiEvent, CardUIEvent zoomCardUIEvent, float middlePos) : base (cardInputHandler)
        {
            _middlePos = middlePos;
            _selectCardUiEvent = selectCardUiEvent;
            _zoomCardUIEvent = zoomCardUIEvent;
            isRegreting = false;
        }


        public override void OnHoldTouch(in Vector2 touchPos, in Vector2 startPos)
        {
            if (IsAboveTheTouchLine(touchPos))
            {
                _selectCardUiEvent.Raise(_cardInputHandler.ThisCardUI);
                isRegreting = true;
            }
            else
            {
                if (isRegreting)
                {
                    _cardInputHandler.CurrentState = CardInputs.CardUIInput.Zoomed;
                    _selectCardUiEvent.Raise(null);
                    _zoomCardUIEvent.Raise(_cardInputHandler.ThisCardUI);
                    isRegreting = false;
                }
            }
        }
        public override void OnReleaseTouch(in Vector2 touchPos)
        {
            isRegreting = false;
            if (IsAboveTheTouchLine(touchPos))
            {
                if (CardExecutionManager.Instance.TryExecuteCard(_cardInputHandler.ThisCardUI))
                {
                    CardUIManager.Instance.ExecuteCardUI();
                }
                  _selectCardUiEvent.Raise(null);
                    return;
            }
            _cardInputHandler.CurrentState = CardInputs.CardUIInput.Zoomed;
            _selectCardUiEvent.Raise(null);
            _zoomCardUIEvent.Raise(null);
        }
         

        public override void ResetTouch()
        {
            base.ResetTouch();
        }

        private bool IsAboveTheTouchLine(in Vector2 touchPos) => (touchPos.y >= _middlePos);
    }

}