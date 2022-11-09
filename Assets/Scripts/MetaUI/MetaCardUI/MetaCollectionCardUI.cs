using System;
using CardMaga.Meta.AccountMetaData;
using CardMaga.MetaData.Collection;
using CardMaga.Tools.Pools;
using TMPro;
using UnityEngine;

namespace CardMaga.UI.MetaUI
{
    public class MetaCollectionCardUI : MonoBehaviour, IPoolableMB<MetaCollectionCardUI>,IUIElement,IVisualAssign<MetaCollectionCardData>//need to change to MetaCardData 
    {
        public event Action OnShow;
        public event Action OnHide;
        public event Action<MetaCollectionCardUI> OnDisposed;
        public event Action OnInitializable;

        [SerializeField] private BaseCardVisualHandler _cardVisuals;
        [SerializeField] private TMP_Text _cardNumberText;
        private MetaCollectionCardData _cardData;

        public int CardID => _cardData.CardId;
        public int NumberOfInstant => _cardData.NumberOfInstant;


        public void Init()
        {
            Show();
            OnInitializable?.Invoke();
        }

        public void Dispose()
        {
            Hide();
            OnDisposed?.Invoke(this);
        }


        public void Show()
        {
            gameObject.SetActive(true);
            OnShow?.Invoke();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            OnHide?.Invoke();
        }

        public void AssignVisual(MetaCollectionCardData data)
        {
            _cardData = data;
            _cardVisuals.Init(data.CardReference.BattleCardData);
            UpdateCardNumber();
        }

        public void AddToDeck()
        {
            if(_cardData.TryAddCardToDeck())
                UpdateCardNumber();
        }

        public void RemoveFromDeck()
        {
            _cardData.RemoveCardFromDeck();
            UpdateCardNumber();
        }

        private void UpdateCardNumber()
        {
            _cardNumberText.text = NumberOfInstant.ToString();
        }
    }
}