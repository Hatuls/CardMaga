using Unity.Events;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Battles.UI.CardUIAttributes
{
    [RequireComponent(typeof(EventTrigger))]
    public class CardInputs :  IPointerClickHandler//, IBeginDragHandler,IPointerUpHandler
    {

        private CanvasGroup _canvasGroup;
        private EventTrigger _eventTrigger;
        public CanvasGroup GetCanvasGroup => _canvasGroup;

        private EventTrigger.Entry endDrag;
        private EventTrigger.Entry beginDrag;
        private EventTrigger.Entry onClick;
        private EventTrigger.Entry onPointerEnter;


        public CardStateMachine CardStateMachine { get; private set; }
        private CardUI _thisCard;
        internal CardUI ThisCardUI => _thisCard;

        private RectTransform _rect;
        public RectTransform Rect =>_rect;
    

        public CardInputs(CanvasGroup canvasGroup, EventTrigger eventTrigger, CardStateMachine.CardUIInput cardUIInput, CardUI card )
        {
            _thisCard = card;
            _rect = card.GFX.GetRectTransform;
            _canvasGroup = canvasGroup;
            _eventTrigger = eventTrigger;

          
            
            CardStateMachine = new CardStateMachine(card, canvasGroup, cardUIInput);
            //const byte states = 5;
            //_touchables = new ITouchable[states]
            //  {
            //    null, // <- locked state
            //    new InHandInputState(this,selectCardEvent ,zoomCardEvent, middlePos),
            //    new OnZoomInputState(this,zoomCardEvent, selectCardEvent, middlePos),
            //    new OnHoldInputstate(this,selectCardEvent ,zoomCardEvent,middlePos),
            //    null
            //   };



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

        int f = 0;
        int j = 0;
        int i = 0;
        public void OnPointerClick(PointerEventData eventData)
        {
            InputManager.Instance.AssignObjectFromTouch(CardStateMachine.CurrentState);
            Debug.Log($"Card {CardStateMachine.CardReference.name} click  id : {++i}");
        }

        public void BeginDrag(PointerEventData eventData)
        {
            Debug.Log($"Card {CardStateMachine.CardReference.name} Begin Drag id : {++j}");

            //if (InputManager.inputState ==  InputManager.InputState.Touch)
            // _canvasGroup.blocksRaycasts = false;
       //     if (CardStateMachine.CurrentState != null&& CardStateMachine.CurrentState.State == CardStateMachine.CardUIInput.Hand)
              InputManager.Instance.AssignObjectFromTouch(CardStateMachine.CurrentState, eventData.position);

        }
        public void EndDrag(PointerEventData eventData)
        {
            Debug.Log($"Card {CardStateMachine.CardReference.name} End Drag id : {++f}");

            ////if (InputManager.inputState == InputManager.InputState.Touch)
            ////    _canvasGroup.blocksRaycasts = true;
            //if (CardStateMachine.CurrentState != null && CardStateMachine.CurrentState.State == CardStateMachine.CardUIInput.Hand)
            //    CardUIHandler.Instance.CardUITouchedReleased(CardStateMachine.CardReference);
        }

        //public void OnBeginDrag(PointerEventData eventData)
        //{
        //    BeginDrag(eventData);
        //}


        //public void OnPointerUp(PointerEventData eventData)
        //{
        //    BeginDrag(eventData);
        //}
 
    }



}