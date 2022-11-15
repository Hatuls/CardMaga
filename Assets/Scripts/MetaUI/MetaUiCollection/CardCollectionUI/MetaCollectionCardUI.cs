using System;
using CardMaga.Meta.AccountMetaData;
using CardMaga.MetaData.Collection;
using CardMaga.Tools.Pools;
using TMPro;
using UnityEngine;

namespace CardMaga.UI.MetaUI
{
    public class MetaCollectionCardUI : BaseUIElement, IPoolableMB<MetaCollectionCardUI>,IVisualAssign<MetaCollectionCardData>//need to change to MetaCardData 
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

        public override void Init()
        {
            base.Init();
            Show();
        }

        public void Dispose()
        {
            Hide();
            OnDisposed?.Invoke(this);
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