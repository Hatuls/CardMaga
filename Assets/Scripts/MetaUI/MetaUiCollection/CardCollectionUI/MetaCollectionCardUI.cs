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
    public class MetaCollectionCardUI : BaseCollectionUIItem<MetaCardData>, IPoolableMB<MetaCollectionCardUI>,IVisualAssign<MetaCollectionCardData>//need to change to MetaCardData 
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
            OnTryAddToDeck -= _cardData.TryAddItemToCollection;
            OnTryRemoveFromDeck -= _cardData.TryRemoveItemFromCollection;
            _cardData.OnSuccessfulAddItemToCollection -= SuccessAddToCollection;
            _cardData.OnSuccessfulRemoveItemFromCollection -= SuccessRemoveFromCollection;
            
            Hide();
            OnDisposed?.Invoke(this);
        }
        
        public void AssignVisual(MetaCollectionCardData cardData)
        {
            _cardData = cardData;
            _cardNumberText.text = NumberOfInstant.ToString();
            _cardVisuals.Init(_cardData.ItemReference.BattleCardData);
            
            OnTryAddToDeck += _cardData.TryAddItemToCollection;
            OnTryRemoveFromDeck += _cardData.TryRemoveItemFromCollection;
            _cardData.OnSuccessfulAddItemToCollection += SuccessAddToCollection;
            _cardData.OnSuccessfulRemoveItemFromCollection += SuccessRemoveFromCollection;
            
            UpdateCardNumber();
        }
        
        private void UpdateCardNumber()
        {
            _cardNumberText.text = NumberOfInstant.ToString();

            if (_cardData.IsNotMoreInstants)
            {
                DisablePlus();
                return;
            }

            if (_cardData.IsMaxInstants)
            {
                DisableMins();
                return;
            }
            Enable();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public override void SuccessAddToCollection(MetaCardData itemData)
        {
            UpdateCardNumber();
        }

        public override void SuccessRemoveFromCollection(MetaCardData itemData)
        {
            UpdateCardNumber();
        }
    }
}