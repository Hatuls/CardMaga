using Battles.UI;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace UI.Meta.Laboratory
{
    //public interface IOnMetaCardUIClicked
    //{
    //    void OpenScreen(MetaCardUIHandler metaCardUIHandler);
    //}
    [System.Serializable]
    public class CardUIEvent : UnityEvent<CardUI> { }
    public class MetaCardUIHandler : MonoBehaviour
    {
        public static Action OnCloseMetaCardUIOptionPanel;

        //[SerializeField] CardUIEvent OnSelectEvent;
        //[SerializeField] CardUIEvent OnRemoveEvent;
        // public static Action<CardUI> OnInfoEvent;
        //[SerializeField] CardUIEvent OnDismentalEvent;



        [SerializeField]
        GameObject _dropList;
        [SerializeField]
        GameObject _useButton;
        [SerializeField]
        GameObject _removeButton;
        [SerializeField]
        GameObject _dismantleButton;
        [SerializeField]
        CardUI _cardUI;
        [SerializeField]
        GameObject _infoButton;

        public CardUI CardUI => _cardUI;

        public GameObject RemoveButton => _removeButton;
        public GameObject InfoButton => _infoButton;
        public GameObject UseButton { get => _useButton; }
        public GameObject DismantleButton { get => _dismantleButton; }

        [SerializeField]
        private MetaCardUIInteractionPanel _metaCardUIInteraction;
        public MetaCardUIInteractionPanel MetaCardUIInteraction => _metaCardUIInteraction;


        public void CloseDropList()
        {
            if (_dropList.activeSelf)
                _dropList.SetActive(false);
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


        public void ResetMetaCardInteraction()
        {
            _dropList.SetActive(false); 
        }
    }
}
