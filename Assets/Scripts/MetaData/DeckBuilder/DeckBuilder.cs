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
                cardData.OnTryAddCard += TryAddCard;
                cardData.OnTryRemoveCard += TryRemoveCard;
                OnSuccessCardAdd += cardData.AddCardToDeck;
                OnSuccessCardRemove += cardData.RemoveCardFromDeck;
            }
        }

        public void DisposeDeck()
        {
            foreach (var cardData in _accountDataCollection.CollectionCardDatas)
            {
                cardData.OnTryAddCard -= TryAddCard;
                cardData.OnTryRemoveCard -= TryRemoveCard;
                OnSuccessCardAdd -= cardData.AddCardToDeck;
                OnSuccessCardRemove -= cardData.RemoveCardFromDeck;
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

        public bool TryAddCombo(MetaComboData comboData)
        {
            if (_deck.Combos.Count >= MAX_COMBO_IN_DECK)
            {
                OnFailedComboAdd?.Invoke("MAX_COMBO_IN_DECK");
                return false;
            }
            _deck.AddCombo(comboData);
            OnSuccessComboAdd?.Invoke(comboData);
            return true;
        }

        private void TryRemoveCard(MetaCardData cardData)
        {
            if (_deck.FindMetaCardData(cardData.CardInstance.ID,out MetaCardData metaCardData))
            {
                _deck.RemoveCard(metaCardData);
                OnSuccessCardRemove?.Invoke(metaCardData);
            }
        }

        public void RemoveCombo(int id)
        {
            if (_deck.FindMetaComboData(id,out MetaComboData metaComboData))
            {
                OnSuccessComboRemove?.Invoke(metaComboData);
            }
        }
    }
}