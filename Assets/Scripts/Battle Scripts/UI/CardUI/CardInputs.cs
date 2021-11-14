using Unity.Events;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Battles.UI.CardUIAttributes
{
    [RequireComponent(typeof(EventTrigger))]
    public class CardInputs :MonoBehaviour,IPointerClickHandler//, IPointerEnterHandlerIBeginDragHandler,IPointerUpHandler
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;
        [SerializeField]
        private EventTrigger _eventTrigger;
        public CanvasGroup GetCanvasGroup => _canvasGroup;

        private EventTrigger.Entry endDrag;
        private EventTrigger.Entry beginDrag;
        private EventTrigger.Entry onClick;
        private EventTrigger.Entry onPointerEnter;


        public CardStateMachine CardStateMachine { get; private set; }
        [SerializeField]
        private CardUI _thisCard;
        internal CardUI ThisCardUI => _thisCard;

        [SerializeField]
        private RectTransform _rect;
        public RectTransform Rect => _rect;

        private void Awake()
        {
            RegisterInputs();
        }
        public CardInputs(CanvasGroup canvasGroup, EventTrigger eventTrigger, CardStateMachine.CardUIInput cardUIInput, CardUI card)
        {




            CardStateMachine = new CardStateMachine(card, canvasGroup, cardUIInput);
   
            RegisterInputs();
        }
        private void OnDestroy()
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

        //int f = 0;
        //int j = 0;
        //int i = 0;
        public void OnPointerClick(PointerEventData eventData)
        {
            InputManager.Instance.AssignObjectFromTouch(CardStateMachine.CurrentState);
            //      Debug.Log($"Card {CardStateMachine.CardReference.name} click  id : {++i}");
        }

        public void BeginDrag(PointerEventData eventData)
        {
            //         Debug.Log($"Card {CardStateMachine.CardReference.name} Begin Drag id : {++j}");

            //if (InputManager.inputState ==  InputManager.InputState.Touch)
            // _canvasGroup.blocksRaycasts = false;
            //     if (CardStateMachine.CurrentState != null&& CardStateMachine.CurrentState.State == CardStateMachine.CardUIInput.Hand)
            InputManager.Instance.AssignObjectFromTouch(CardStateMachine.CurrentState, eventData.position);

        }
        public void EndDrag(PointerEventData eventData)
        {
            //     Debug.Log($"Card {CardStateMachine.CardReference.name} End Drag id : {++f}");

        }



        //public void OnPointerEnter(PointerEventData eventData)
        //{
        //    if (this.CardStateMachine.CurrentState.State == CardStateMachine.CardUIInput.Hand)
        //    {
        //        InputManager.Instance.AssignObjectFromTouch(CardStateMachine.CurrentState, eventData.position);
        //        CardStateMachine.TouchPos = eventData.position;
        //        CardUIHandler.Instance.CardUITouched(_thisCard);
        //    }
        //}

    }



}