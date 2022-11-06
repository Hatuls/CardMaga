using System;
using CardMaga.Card;
using CardMaga.Input;
using CardMaga.Tools.Pools;
using CardMaga.UI.Card;
using CardMaga.UI.ScrollPanel;
using UnityEngine;

namespace MetaUI.MetaCardUI
{
    public class MetaCardUI : MonoBehaviour, IPoolableMB<MetaCardUI>,IShowableUI,IVisualAssign<CardData>//need to change to MetaCardData 
    {
        public event Action<MetaCardUI> OnDisposed;
        public event Action<CardData> OnAddCard; 
        public event Action<CardData> OnRemoveCard; 

        [SerializeField] private CardUI _cardUI;
        [SerializeField] private Button _plusBuuton;
        [SerializeField] private Button _minusBuuton;

        private CardData _cardData;
        
        public void Init()
        {
            _cardUI.Init();
            _plusBuuton.OnClick += AddToDeck;
            _minusBuuton.OnClick += RemoveFromDeck;
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

        public void AssingVisual(CardData data)
        {
            _cardData = data;
            _cardUI.AssingVisual(data);
        }

        private void AddToDeck()
        {
            OnAddCard?.Invoke(_cardData);   
        }

        private void RemoveFromDeck()
        {
            OnRemoveCard?.Invoke(_cardData);
        }
    }
}