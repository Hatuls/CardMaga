using Unity.Events;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Battles.UI.CardUIAttributes
{
    [RequireComponent(typeof(EventTrigger))]
    public class CardInputs : ITouchable , IPointerClickHandler, IBeginDragHandler,IPointerUpHandler
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

            const byte states = 4;
            _touchables = new ITouchable[states]
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



        public OnZoomInputState OnZoomInputState { get => (OnZoomInputState)GetTouchable(CardUIInput.Zoomed); }


       

        public ITouchable GetTouchable(CardUIInput cardUIInput)
        {
            return _touchables[(int)cardUIInput];
        }

        public void OnFirstTouch(in Vector2 touchPos)
        {
            Debug.Log($"First Touch With Current state {CurrentState}");
            GetTouchable(CurrentState)?.OnFirstTouch(in touchPos);
        }

        public void OnReleaseTouch(in Vector2 touchPos)
        {
            Debug.Log($"Release With Current state {CurrentState}");
            GetTouchable(CurrentState)?.OnReleaseTouch(in touchPos);
        }

        public void OnHoldTouch(in Vector2 touchPos, in Vector2 startPos)
        {
            Debug.Log($"Holding With Current state {CurrentState}");
            GetTouchable(CurrentState)?.OnHoldTouch(in touchPos,in startPos);
        }

        public void ResetTouch()
        {
            Debug.Log($"Reseting With Current state {CurrentState}");
            GetTouchable(CurrentState)?.ResetTouch();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            BeginDrag(eventData);
        }


        public void OnPointerUp(PointerEventData eventData)
        {
            BeginDrag(eventData);
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
        CardUI _cardReference;
        float _middlePos;
        public InHandInputState(CardInputs cardInputHandler, CardUIEvent selectCardUI, CardUIEvent zoomCard,float middlePos) : base(cardInputHandler)
        {
            _zoomCardUIEvent = zoomCard;
            _selectCardUIEvent = selectCardUI;
            _middlePos = middlePos;
            _cardReference = cardInputHandler.ThisCardUI;
        }
        public override void OnFirstTouch(in Vector2 touchPos)
        {
            CardUIManager.Instance.CardUITouched(_cardReference);
        }

        public override void OnReleaseTouch(in Vector2 touchPos)
        {
            CardUIManager.Instance.CardUITouchedReleased(_cardReference);
            Debug.Log("Released card!");
        }

    }


  

    public class OnZoomInputState : InputStateAbst
    {
        float _offset =130f;
        float _middlePos;
        public Vector2 OriginalCardPosition { get; set; }
        public OnZoomInputState(CardInputs cardInputHandler, CardUIEvent ZoomedEvent, CardUIEvent selectCardEvent, float middlepos) : base(cardInputHandler)
        {
            _middlePos = middlepos;
        }

        public override void OnFirstTouch(in Vector2 touchPos)
        {
            OnHoldTouch(touchPos, touchPos);
        }
        public override void OnHoldTouch(in Vector2 touchPos, in Vector2 startPos)
        {
            // check distance from the card 
            // if its close to the original position then zoom
            // if its above then move to hold
            // if its side ways then remove it
            float distanceFromOriginalPoint = Vector2.Distance(OriginalCardPosition, touchPos);
            Debug.Log(distanceFromOriginalPoint);



            if (IsAboveTheTouchLine(touchPos))
            {
                _cardInputHandler.CurrentState = CardInputs.CardUIInput.Hold;
            }
        }
        private bool IsAboveTheTouchLine(in Vector2 touchPos) => (touchPos.y >= _middlePos);
        public override void OnReleaseTouch(in Vector2 touchPos)
        {

            CardUIManager.Instance.CardUITouchedReleased(_cardInputHandler.ThisCardUI);
            _cardInputHandler.ThisCardUI.CardTranslations.CancelAllTweens();

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

        }
        public override void OnReleaseTouch(in Vector2 touchPos)
        {

            CardUIManager.Instance.CardUITouchedReleased(_cardInputHandler.ThisCardUI);
            _cardInputHandler.ThisCardUI.CardTranslations.CancelAllTweens();

        }


        public override void ResetTouch()
        {
            base.ResetTouch();
        }

        private bool IsAboveTheTouchLine(in Vector2 touchPos) => (touchPos.y >= _middlePos);
    }

}