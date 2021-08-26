using Unity.Events;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Battles.UI.CardUIAttributes
{
    [RequireComponent(typeof(EventTrigger))]
    public class CardInputs : ITouchable
    {
        public enum CardUIInput {None =0 , Locked= 1, Hold = 2,Hand =3,Zoomed =4};

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

          

            
            _touchables = new ITouchable[5]
            {
                null,
                null,
                new OnHoldInputstate(this),
                new InHandInputState(this,selectCardEvent ,zoomCardEvent),
                new OnZoomInputState(this,zoomCardEvent, selectCardEvent)
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
                case CardUIInput.None:
                case CardUIInput.Locked:
                default:
                    break;

                case CardUIInput.Hold:
                case CardUIInput.Hand:
                    InputManager.Instance.AssignObjectFromTouch(this);

                    //      _onClickedCardEvent?.Raise(_thisCard);
                    break;

            }
        }

        public void BeginDrag(PointerEventData eventData)
        {
            switch (CurrentState)
            {
                case CardUIInput.None:
                case CardUIInput.Locked:
                default:
                    break;

                case CardUIInput.Hold:
                case CardUIInput.Hand:
                    _canvasGroup.blocksRaycasts = false;
                    InputManager.Instance.AssignObjectFromTouch(this, eventData.position);

                    //_removeCardEvent?.Raise(_thisCard);


                    //_selectCardEvent?.Raise(_thisCard);
                    break;

            }

        }

        public void EndDrag(PointerEventData eventData)
        {
            switch (CurrentState)
            {
                case CardUIInput.None:
                case CardUIInput.Locked:
                default:
                    break;

                case CardUIInput.Hold:
                case CardUIInput.Hand:
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
    }







    public class OnZoomInputState : InputStateAbst
    {
   
        CardUIEvent _selectCardEvent;
        CardUIEvent _zoomCardEvent;
        float _middlePos;
        public OnZoomInputState(CardInputs cardInputHandler, CardUIEvent ZoomedEvent, CardUIEvent selectCardEvent) : base(cardInputHandler)
        {
            _middlePos = CardUIManager.Instance.GetInputHandLine;
            _zoomCardEvent = ZoomedEvent;
            _selectCardEvent = selectCardEvent;
        }


        public override void OnHoldTouch(in Vector2 touchPos, in Vector2 startPos)
        {

            if (touchPos.y >= _middlePos/2)
            {
                Debug.Log("Zoomed");
                _selectCardEvent?.Raise(_cardInputHandler.ThisCardUI);
            }
            else
            {
                _cardInputHandler.CurrentState = CardInputs.CardUIInput.Hand;
                _selectCardEvent?.Raise(null);

            }
        }

        public override void OnReleaseTouch(in Vector2 touchPos)
        {


            if (touchPos.y >= _middlePos / 2)
                CardExecutionManager.Instance.TryExecuteCard(_cardInputHandler.ThisCardUI);


                _selectCardEvent?.Raise(null);
                _zoomCardEvent?.Raise(null);
            
        }
        public override void ResetTouch()
        {
            _selectCardEvent?.Raise(null);
            _zoomCardEvent?.Raise(null);

            _cardInputHandler.CurrentState = CardInputs.CardUIInput.Hand;
        }
    }
   


    public class OnHoldInputstate : InputStateAbst
    {
        public OnHoldInputstate(CardInputs cardInputHandler) : base (cardInputHandler)
        {    
        }


        public override void OnHoldTouch(in Vector2 touchPos, in Vector2 startPos)
        { 
        }
        public override void OnReleaseTouch(in Vector2 touchPos)
        {
            
        }

        public override void ResetTouch()
        {
            base.ResetTouch();
        }
    }


    public abstract class InputStateAbst : ITouchable
    {
        protected CardInputs _cardInputHandler;
        public InputStateAbst(CardInputs cardInputHandler)
        {
            this._cardInputHandler = cardInputHandler;
        }

        public RectTransform Rect => _cardInputHandler.Rect;

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
        public InHandInputState(CardInputs cardInputHandler, CardUIEvent selectCardUI, CardUIEvent zoomCard) : base(cardInputHandler)
        {
            _zoomCardUIEvent = zoomCard;
            _selectCardUIEvent = selectCardUI;
            _middlePos = CardUIManager.Instance.GetInputHandLine;
        }

        public override void OnHoldTouch(in Vector2 touchPos, in Vector2 startPos)
        {
            /*
             * check if he holding the card and if his dragging it
             * if he hold it and move it up it means hes taking the card from the hand
             */

                     Debug.Log("touchPos " + touchPos);
            //if card is pulled up
            if (touchPos.y <= _middlePos)
            {
                _cardInputHandler.CurrentState = CardInputs.CardUIInput.Zoomed;
                _zoomCardUIEvent?.Raise(_cardInputHandler.ThisCardUI);
            }
   

        }
        public override void OnReleaseTouch(in Vector2 touchPos)
        {
            _selectCardUIEvent?.Raise(null);
            _zoomCardUIEvent?.Raise(null);
        }
        public override void ResetTouch()
        {
            base.ResetTouch();
        }
    }


  
}