using Battles.UI;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace UI.Meta.Laboratory
{




    [Serializable]
    public class CardUIEvent : UnityEvent<CardUI> { }
    public class MetaCardUIHandler : MonoBehaviour
    {
        public static Action OnCloseMetaCardUIOptionPanel;

        private bool _cardIsWaitingForInput = false;


        [SerializeField]
        CardUI _cardUI;
        [SerializeField]
        public UnityEvent OnCardClicked;
        [SerializeField]
        public CardUIEvent OnCardUIClicked;




        [SerializeField]
        private MetaCardUIInteractionPanel _metaCardUIInteraction;
        public MetaCardUIInteractionPanel MetaCardUIInteraction => _metaCardUIInteraction;
        public CardUI CardUI => _cardUI;
        public bool CardIsWaitingForInput { get => _cardIsWaitingForInput; set => _cardIsWaitingForInput = value; }


        public void OnClick()
        {
            if (CardIsWaitingForInput)
                OnCardUIClicked?.Invoke(this.CardUI);
            else
                OnCardClicked?.Invoke();
        }

        //public void OnSelected()
        //{
        //    OnSelectEvent?.Invoke(CardUI);
        //    OnCloseMetaCardUIOptionPanel?.Invoke();

        //}

        //public void OnInfo()
        //{
        //    OnInfoEvent?.Invoke(CardUI);
        //    OnCloseMetaCardUIOptionPanel?.Invoke();

        //}
        //public void OnRemove()
        //{
        //    OnRemoveEvent?.Invoke(CardUI);
        //    OnCloseMetaCardUIOptionPanel?.Invoke();
        //}
        //public void OnDismental()
        //{
        //    OnDismentalEvent?.Invoke(CardUI);
        //    OnCloseMetaCardUIOptionPanel?.Invoke();

        //}

        //public void OnCardClicked()
        //{
        //    Debug.Log("Changeing Drop List State");
        //    if (_dropList.activeSelf)
        //    {
        //        CloseDropList();
        //    }
        //    else
        //    {
        //        OnCloseMetaCardUIOptionPanel?.Invoke();
        //    //    _onMetaCardUIClicked?.OpenScreen(this);
        //        _dropList.SetActive(true);
        //    }
        //}



    }
}
