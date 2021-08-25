using Unity.Events;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Battles.UI.CardUIAttributes
{
    [RequireComponent(typeof(EventTrigger))]
    public class CardInputs : ITouchable
    {
        public enum CardUIInput {None =0 , Locked= 1, Hold = 2,Hand =3};


        private CardUIInput _currentState = CardUIInput.Locked;
        public CardUIInput CurrentState
        {
            get => _currentState;
            set => _currentState = value;
        }
        ITouchable[] _touchables;

        private CardUIEvent _selectCardEvent;
        private CardUIEvent _removeCardEvent;
        private CardUIEvent _onClickedCardEvent;


        private CanvasGroup _canvasGroup;
        private CardUI _thisCard;
        private EventTrigger _eventTrigger;
        public CanvasGroup GetCanvasGroup => _canvasGroup;

        private EventTrigger.Entry endDrag;
        private EventTrigger.Entry beginDrag;
        private EventTrigger.Entry onClick;



        internal CardUI ThisCardUI => _thisCard;
        public CardInputs(CanvasGroup canvasGroup, CardUIEvent SelectCardEvent, CardUIEvent RemoveCardEvent, CardUIEvent OnClickedCardEvent, CardUI card)
        {
            _selectCardEvent = SelectCardEvent;
            _removeCardEvent = RemoveCardEvent;
            _onClickedCardEvent = OnClickedCardEvent;


            _canvasGroup = canvasGroup;
            _thisCard = card;
            _eventTrigger = card.gameObject.GetComponent<EventTrigger>();

            _touchables = new ITouchable[4]
            {
                null,
                null,
                null, // hold
                new InHandInputState(this)
            };



            RegisterInputs();
        }
        ~CardInputs()
        {
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
                    _onClickedCardEvent?.Raise(_thisCard);
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
                     _removeCardEvent?.Raise(_thisCard);

                       _canvasGroup.blocksRaycasts = false;

                      _selectCardEvent?.Raise(_thisCard);
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
    }














    public class InHandInputState : ITouchable
    {
        CardInputs _cardInputHandler;
        float _offsetDistance=1f;
        public InHandInputState(CardInputs ci) 
        {
            _cardInputHandler = ci;
        }

        public void OnFirstTouch(in Vector2 touchPos)
        {
            // scale card


        }

        public  void OnHoldTouch(in Vector2 touchPos , in Vector2 startPos)
        {
            /*
             * check if he holding the card and if his dragging it
             * if he hold it and move it up it means hes taking the card from the hand
             */

            //if card is pulled up
            if (startPos.y < touchPos.y && Mathf.Abs(startPos.y - touchPos.y) < _offsetDistance)
            {
                _cardInputHandler.CurrentState = CardInputs.CardUIInput.Hold;
            }
        }

        public  void OnReleaseTouch(in Vector2 touchPos)
        {

            _cardInputHandler.CurrentState = CardInputs.CardUIInput.None;

            InputManager.Instance.RemoveObjectFromTouch();
        }
    }


  
}