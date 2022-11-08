using System;
using System.Collections.Generic;
using CardMaga.Meta.AccountMetaData;
using CardMaga.Tools.Pools;
using CardMaga.UI;
using CardMaga.UI.ScrollPanel;
using TMPro;
using UnityEngine;

namespace MetaUI.MetaCardUI
{
    public class MetaCardUI : MonoBehaviour, IPoolableMB<MetaCardUI>,IShowableUI,IVisualAssign<MetaCardData>//need to change to MetaCardData 
    {
        public event Action<MetaCardUI> OnDisposed;
        public event Action<int> OnAddCard; 
        public event Action<int> OnRemoveCard; 
        
        [SerializeField] private BaseCardVisualHandler _cardVisuals;
        [SerializeField] private TMP_Text _cardNumberText;
        private List<MetaCardData> _cardDatas;
        private int _cardId;
        private int _cardLevel;

        public int CardID => _cardId;
        public int CardLevel => _cardLevel;
        
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

        public void AssingVisual(MetaCardData data)
        {
            _cardLevel = data.CardData.CardInstanceID.Level;
            _cardId = data.CardData.CardInstanceID.ID;
            _cardVisuals.Init(data.CardData);
        }

        public void AddToDeck()
        {
            UpdateCardNumber();
            Debug.Log("AddCard"+ this.name);
            OnAddCard?.Invoke(_cardId);   
        }

        public void RemoveFromDeck()
        {
            UpdateCardNumber();
            OnRemoveCard?.Invoke(_cardId);
        }

        private void UpdateCardNumber()
        {
            _cardNumberText.text = _cardDatas.Count.ToString();
        }
    }
}