using System;
using CardMaga.Card;
using CardMaga.Meta.AccountMetaData;
using CardMaga.Tools.Pools;
using CardMaga.UI.Card;
using CardMaga.UI.ScrollPanel;
using UnityEngine;

namespace MetaUI.MetaCardUI
{
    public class MetaCardUI : MonoBehaviour, IPoolableMB<MetaCardUI>,IShowableUI,IVisualAssign<MetaCardData>//need to change to MetaCardData 
    {
        public event Action<MetaCardUI> OnDisposed;
        public event Action<CardData> OnAddCard; 
        public event Action<CardData> OnRemoveCard; 

        [SerializeField] private CardUI _cardUI;

        private CardData _cardData;
        
        public void Init()
        {
            _cardUI.Init();
        }

        public void Dispose()
        {
            _cardUI.Dispose();
            gameObject.SetActive(false);
            OnDisposed?.Invoke(this);
        }

        public void Show()
        {
            Init();
            _cardUI.Init();
        }

        public void AssingVisual(MetaCardData data)
        {
            _cardData = data.CardData;
            _cardUI.AssingVisual(data.CardData);
        }

        public void AddToDeck()
        {
            Debug.Log("AddCard"+ this.name);
            OnAddCard?.Invoke(_cardData);   
        }

        public void RemoveFromDeck()
        {
            OnRemoveCard?.Invoke(_cardData);
        }
    }
}