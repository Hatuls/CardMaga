using DesignPattern;
using Unity.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


namespace Battle.UI.CardUIAttributes
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

 
        
        [SerializeField]
        private CardUI _thisCard;
        internal CardUI ThisCardUI => _thisCard;

        [SerializeField]
        private RectTransform _rect;
        public RectTransform Rect => _rect;

        [SerializeField]
        ObserverSO _observer;

      
        private void OnEnable()
        {
            RegisterInputs();
        }
        private void OnDisable()
        {
            if (onPointerEnter.callback.GetPersistentEventCount() > 0)
            {
             onPointerEnter.callback.RemoveAllListeners();
             beginDrag.callback.RemoveAllListeners();
             endDrag.callback.RemoveAllListeners();
             onClick.callback.RemoveAllListeners();
            }
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

  

        public UnityAction<CardUI,PointerEventData> OnPointerClickEvent;
        public UnityAction<CardUI, PointerEventData> OnBeginDragEvent;
        public UnityAction<CardUI, PointerEventData> OnEndDragEvent;

        public void OnPointerClick(PointerEventData eventData)
        {
            //  InputManager.Instance.AssignObjectFromTouch(CardStateMachine.CurrentState);
            //      Debug.Log($"Card {CardStateMachine.CardReference.name} click  id : {++i}");
            OnPointerClickEvent?.Invoke(_thisCard,eventData);
        }

        public void BeginDrag(PointerEventData eventData)
        {
            //         Debug.Log($"Card {CardStateMachine.CardReference.name} Begin Drag id : {++j}");

            //if (InputManager.inputState ==  InputManager.InputState.Touch)
            // _canvasGroup.blocksRaycasts = false;
            //     if (CardStateMachine.CurrentState != null&& CardStateMachine.CurrentState.State == CardStateMachine.CardUIInput.Hand)
            //  InputManager.Instance.AssignObjectFromTouch(CardStateMachine.CurrentState, eventData.position);

            OnBeginDragEvent?.Invoke(_thisCard, eventData);

        }
        public void EndDrag(PointerEventData eventData)
        {
            //     Debug.Log($"Card {CardStateMachine.CardReference.name} End Drag id : {++f}");
            OnEndDragEvent?.Invoke(_thisCard, eventData);

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


    [CreateAssetMenu]
    public class InputSO : ScriptableObject
    {
       // public UnityEvent
    }

}