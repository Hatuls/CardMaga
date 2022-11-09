using System;
using System.Collections.Generic;
using CardMaga.Meta.AccountMetaData;
using CardMaga.Tools.Pools;
using CardMaga.UI.ScrollPanel;
using TMPro;
using UnityEngine;

namespace CardMaga.UI.MetaUI
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
            _cardLevel = data.BattleCardData.CardInstance.Level;
            _cardId = data.BattleCardData.CardInstance.ID;
            _cardVisuals.Init(data.BattleCardData);
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