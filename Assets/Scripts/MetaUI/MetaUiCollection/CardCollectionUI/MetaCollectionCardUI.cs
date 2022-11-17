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
        public int CardID => _cardData.CardReference.CardInstance.ID;

        public int NumberOfInstant => _cardData.NumberOfInstant;
        
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
            _cardData = data;
            
            _cardVisuals.Init(_cardData.CardReference.BattleCardData);
            
            OnTryAddCardToDeck += _cardData.TryRemoveCardReference;
            OnTryRemoveCardFromDeck += _cardData.TryAddCardReference;
            _cardData.OnSuccessfullAddCard += SuccessAddCardToDeck;
            _cardData.OnSuccessfullRemoveCard += SuccessRemoveCardFromDeck;
            
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