using System;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.Collection;
using CardMaga.Tools.Pools;
using CardMaga.UI;
using TMPro;
using UnityEngine;

namespace CardMaga.MetaUI
{
    public class MetaCollectionCardUI : BaseUIElement, IPoolableMB<MetaCollectionCardUI>,IVisualAssign<MetaCollectionCardData>//need to change to MetaCardData 
    {
        public event Action<MetaCollectionCardUI> OnDisposed;
        public event Action OnTryAddCardToDeck; 
        public event Action OnTryRemoveCardFromDeck; 
        
        [SerializeField] private BaseCardVisualHandler _cardVisuals;
        [SerializeField] private TMP_Text _cardNumberText;
        private MetaCollectionCardData _cardData;
        private int _cardId;

        public int CardID => _cardId;

        public int NumberOfInstant => _cardData.NumberOfInstant;
        
        public override void Init()
        {
            base.Init();
            Show();
            OnTryAddCardToDeck += _cardData.TryAddCardToDeck;
            OnTryRemoveCardFromDeck += _cardData.TryRemoveCardFromDeck;
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
            UpdateCardNumber();
        }

        public void TryAddToDeck()
        {
            OnTryAddCardToDeck?.Invoke();
        }

        public void TryRemoveFromDeck()
        {
            OnTryRemoveCardFromDeck?.Invoke();
        }

        public void SuccessAddCardToDeck(MetaCardData metaCardData)
        {
            UpdateCardNumber();
        }
        
        public void SuccessRemoveCardFromDeck(MetaCardData metaCardData)
        {
            UpdateCardNumber();
        }

        private void UpdateCardNumber()
        {
            _cardNumberText.text = NumberOfInstant.ToString();
        }
    }
}