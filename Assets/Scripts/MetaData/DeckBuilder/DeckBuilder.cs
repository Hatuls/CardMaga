using System;
using System.Collections.Generic;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.Collection;

namespace CardMaga.MetaData.DeckBuilding
{
    public class DeckBuilder
    {
        #region Events
        
        public event Action<MetaCardData> OnSuccessCardAdd;
        public event Action<string> OnFailedCardAdd;
        public event Action<MetaCardData> OnSuccessCardRemove; 
        public event Action<MetaComboData> OnSuccessComboAdd;
        public event Action<string> OnFailedComboAdd; 
        public event Action<MetaComboData> OnSuccessComboRemove; 

        #endregion
        
        private const int MAX_CARD_IN_DECK = 8;
        private const int MAX_COMBO_IN_DECK = 3;

        private MetaDeckData _deck;
        
        private AccountDataCollectionHelper _accountDataCollection;

        public DeckBuilder(AccountDataCollectionHelper accountDataCollectionHelper)
        {
            _accountDataCollection = accountDataCollectionHelper;
        }

        public void AssingDeckToEdit(MetaDeckData deckData)
        {
            _deck = deckData;

            foreach (var cardData in _accountDataCollection.CollectionCardDatas)
            {
                cardData.OnTryAddItem += TryAddCard;
                cardData.OnTryRemoveItem += TryRemoveCard;
                OnSuccessCardAdd += cardData.RemoveItemReference;
                OnSuccessCardRemove += cardData.AddItemReference;
            }
            
            foreach (var comboData in _accountDataCollection.CollectionComboDatas)
            {
                comboData.OnTryAddItem += TryAddCombo;
                comboData.OnTryRemoveItem += TryRemoveCombo;
                OnSuccessComboAdd += comboData.RemoveItemReference;
                OnSuccessComboRemove += comboData.AddItemReference;
            }
        }

        public void DisposeDeck()
        {
            foreach (var cardData in _accountDataCollection.CollectionCardDatas)
            {
                cardData.OnTryAddItem -= TryAddCard;
                cardData.OnTryRemoveItem -= TryRemoveCard;
                OnSuccessCardAdd -= cardData.RemoveItemReference;
                OnSuccessCardRemove -= cardData.AddItemReference;
            }
            
            foreach (var comboData in _accountDataCollection.CollectionComboDatas)
            {
                comboData.OnTryAddItem -= TryAddCombo;
                comboData.OnTryRemoveItem -= TryRemoveCombo;
                OnSuccessComboAdd -= comboData.RemoveItemReference;
                OnSuccessComboRemove -= comboData.AddItemReference;
            }
        }

        private void TryAddCard(MetaCardData cardData)
        {
            if (_deck.Cards.Count >= MAX_CARD_IN_DECK)
            {
                OnFailedCardAdd?.Invoke("MAX_CARD_IN_DECK");
                return;
            }
            _deck.AddCard(cardData);
            OnSuccessCardAdd?.Invoke(cardData);
            return;
        }

        public void TryAddCombo(MetaComboData comboData)
        {
            if (_deck.Combos.Count >= MAX_COMBO_IN_DECK)
            {
                OnFailedComboAdd?.Invoke("MAX_COMBO_IN_DECK");
                return;
            }
            _deck.AddCombo(comboData);
            OnSuccessComboAdd?.Invoke(comboData);
            return;
        }

        private void TryRemoveCard(MetaCardData cardData)
        {
            if (_deck.FindMetaCardData(cardData.CardInstance.ID,out MetaCardData metaCardData))
            {
                _deck.RemoveCard(metaCardData);
                OnSuccessCardRemove?.Invoke(metaCardData);
            }
        }

        public void TryRemoveCombo(MetaComboData comboData)
        {
            if (_deck.FindMetaComboData(comboData.ID,out MetaComboData metaComboData))
            {
                _deck.RemoveCombo(metaComboData);
                OnSuccessComboRemove?.Invoke(metaComboData);
            }
        }
    }
}