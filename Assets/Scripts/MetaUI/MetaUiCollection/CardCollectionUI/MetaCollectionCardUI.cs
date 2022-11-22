using System;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.Collection;
using CardMaga.MetaUI.CollectionUI;
using CardMaga.Tools.Pools;
using CardMaga.UI;
using TMPro;
using UnityEngine;

namespace CardMaga.MetaUI
{
    public class MetaCollectionCardUI : BaseCollectionItemUI<MetaCardData>, IPoolableMB<MetaCollectionCardUI>,IVisualAssign<MetaCollectionCardData>//need to change to MetaCardData 
    {
        public event Action<MetaCollectionCardUI> OnDisposed;

        [SerializeField] private BaseCardVisualHandler _cardVisuals;
        [SerializeField] private TMP_Text _cardNumberText;
        private MetaCollectionCardData _cardData;
        public int CardID => _cardData.ItemReference.CardInstance.ID;

        public int NumberOfInstant => _cardData.NumberOfInstant;
        
        public override void Init()
        {
            base.Init();
            Show();
        }

        public void Dispose()
        {
            OnTryAddToDeck -= _cardData.TryRemoveItemReference;
            OnTryRemoveFromDeck -= _cardData.TryAddItemReference;
            _cardData.OnSuccessfullAddItem -= SuccessAddToDeck;
            _cardData.OnSuccessfullRemoveItem -= SuccessRemoveFromDeck;
            
            Hide();
            OnDisposed?.Invoke(this);
        }
        public void AssignVisual(MetaCollectionCardData data)
        {
            _cardData = data;
            _cardNumberText.text = NumberOfInstant.ToString();
            _cardVisuals.Init(_cardData.ItemReference.BattleCardData);
            
            OnTryAddToDeck += _cardData.TryRemoveItemReference;
            OnTryRemoveFromDeck += _cardData.TryAddItemReference;
            _cardData.OnSuccessfullAddItem += SuccessAddToDeck;
            _cardData.OnSuccessfullRemoveItem += SuccessRemoveFromDeck;
            
            UpdateCardNumber();
        }

        public override void SuccessAddToDeck(MetaCardData metaCardData)
        {
            UpdateCardNumber();
        }

        public override void SuccessRemoveFromDeck(MetaCardData metaCardData)
        {
            UpdateCardNumber();
        } 
        private void UpdateCardNumber()
        {
            _cardNumberText.text = NumberOfInstant.ToString();
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}