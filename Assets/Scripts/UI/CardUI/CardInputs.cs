using Unity.Events;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Battles.UI.CardUIAttributes
{
    [RequireComponent(typeof(EventTrigger))]
    public class CardInputs
    {

        CardUIEvent _selectCardEvent;
        CardUIEvent _removeCardEvent;
        CardUIEvent _onClickedCardEvent;
        CanvasGroup _canvasGroup;
        CardUI _thisCard;
        EventTrigger _eventTrigger;
        public CanvasGroup GetCanvasGroup => _canvasGroup;
        EventTrigger.Entry endDrag;
        EventTrigger.Entry beginDrag;
        EventTrigger.Entry onClick;
        public CardInputs(ref CanvasGroup canvasGroup, ref CardUIEvent SelectCardEvent, ref CardUIEvent RemoveCardEvent, ref CardUIEvent OnClickedCardEvent, CardUI card)
        {
            _selectCardEvent = SelectCardEvent;
            _removeCardEvent = RemoveCardEvent;
            _onClickedCardEvent = OnClickedCardEvent;
            _canvasGroup = canvasGroup;
            _thisCard = card;
            _eventTrigger = card.gameObject.GetComponent<EventTrigger>();

            RegisterInputs();
        }
        ~CardInputs()
        {
            beginDrag.callback.RemoveAllListeners();
            endDrag.callback.RemoveAllListeners();
            onClick.callback.RemoveAllListeners();
            
        }
        public void RegisterInputs ()
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
            _onClickedCardEvent?.Raise(_thisCard);
         
        }
     
        public  void BeginDrag(PointerEventData eventData)
        {
            _removeCardEvent?.Raise(_thisCard);

            _canvasGroup.blocksRaycasts = false;

            _selectCardEvent?.Raise(_thisCard);
        }

        public  void EndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;

            Debug.Log("End Touch");
        }




    }
}