using System;
using System.Collections.Generic;
using CardMaga.Meta.AccountMetaData;
using CardMaga.MetaData.Collection;
using CardMaga.Tools.Pools;
using CardMaga.UI.ScrollPanel;
using TMPro;
using UnityEngine;

namespace CardMaga.UI.MetaUI
{
    public class MetaCollectionCardUI : MonoBehaviour, IPoolableMB<MetaCollectionCardUI>,IShowableUI,IVisualAssign<MetaCollectionCardData>//need to change to MetaCardData 
    {
        public event Action<MetaCollectionCardUI> OnDisposed;
        public event Action<int> OnAddCard; 
        public event Action<int> OnRemoveCard; 
        
        [SerializeField] private BaseCardVisualHandler _cardVisuals;
        [SerializeField] private TMP_Text _cardNumberText;
        private MetaCardData _cardData;
        private int _cardId;
        private int _numberOfInstant;

        public int CardID => _cardId;
        
        public void Init()
        {
            gameObject.SetActive(true);
        }

        public void Dispose()
        {
            gameObject.SetActive(false);
            OnDisposed?.Invoke(this);
        }

        public void Show()
        {
            Init();
        }

        public void AssignVisual(MetaCollectionCardData data)
        {
            _cardId = data.CardReference.BattleCardData.CardInstance.ID;
            _cardVisuals.Init(data.CardReference.BattleCardData);
            _numberOfInstant = data.NumberOfInstant;
            UpdateCardNumber();
        }

        public void AddToDeck()
        {
            _numberOfInstant--;
            UpdateCardNumber();
            Debug.Log("AddCard"+ this.name);
            OnAddCard?.Invoke(_cardId);   
        }

        public void RemoveFromDeck()
        {
            _numberOfInstant++;
            UpdateCardNumber();
            OnRemoveCard?.Invoke(_cardId);
        }

        private void UpdateCardNumber()
        {
            _cardNumberText.text = _numberOfInstant.ToString();
        }
    }
}