using System;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.Collection;
using CardMaga.SequenceOperation;
using MetaData;
using ReiTools.TokenMachine;

namespace CardMaga.MetaData.DeckBuilding
{
    public class DeckBuilder : ISequenceOperation<MetaDataManager>
    {
        #region Events
        public event Action OnDeckNameUpdate; 
        public event Action<MetaCardData> OnSuccessCardAdd;
        public event Action<string> OnFailedCardAdd;
        public event Action<MetaCardData> OnSuccessCardRemove;
        public event Action<MetaComboData> OnSuccessComboAdd;
        public event Action<string> OnFailedComboAdd; 
        public event Action<MetaComboData> OnSuccessComboRemove;
        public event Action<MetaDeckData> OnNewDeckLoaded; 

        #endregion
        
        private const int MAX_CARD_IN_DECK = 8;
        private const int MAX_COMBO_IN_DECK = 3;

        private MetaDeckData _deck;
        
        private AccountDataCollectionHelper _accountDataCollection;
        

        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
            _accountDataCollection = data.AccountDataCollectionHelper;
        }

        public int Priority => 1;

        public void AssingDeckToEdit(MetaDeckData deckData)
        {
            _deck = deckData;

            foreach (var cardData in _accountDataCollection.CollectionCardDatas)
            {
                cardData.OnTryAddItemToCollection += TryAddCard;
                cardData.OnTryRemoveItemFromCollection += TryRemoveCard;
                OnSuccessCardAdd += cardData.AddItemToCollection;
                OnSuccessCardRemove += cardData.RemoveItemFromCollection;
            }
            
            foreach (var comboData in _accountDataCollection.CollectionComboDatas)
            {
                comboData.OnTryAddItemToCollection += TryAddCombo;
                comboData.OnTryRemoveItemFromCollection += TryRemoveCombo;
                OnSuccessComboAdd += comboData.AddItemToCollection;
                OnSuccessComboRemove += comboData.RemoveItemFromCollection;
            }
            
            OnNewDeckLoaded?.Invoke(_deck);
        }

        public void DisposeDeck()//Need to call
        {
            foreach (var cardData in _accountDataCollection.CollectionCardDatas)
            {
                cardData.OnTryAddItemToCollection -= TryAddCard;
                cardData.OnTryRemoveItemFromCollection -= TryRemoveCard;
                OnSuccessCardAdd -= cardData.AddItemToCollection;
                OnSuccessCardRemove -= cardData.RemoveItemFromCollection;
            }
            
            foreach (var comboData in _accountDataCollection.CollectionComboDatas)
            {
                comboData.OnTryAddItemToCollection -= TryAddCombo;
                comboData.OnTryRemoveItemFromCollection -= TryRemoveCombo;
                OnSuccessComboAdd -= comboData.AddItemToCollection;
                OnSuccessComboRemove -= comboData.RemoveItemFromCollection;
            }
        }
        
        public bool TryEditDeckName(string name)
        {
            //add name valid
            _deck.UpdateDeckName(name);
            OnDeckNameUpdate?.Invoke();
            return true;
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
        }

        private void TryAddCombo(MetaComboData comboData)
        {
            if (_deck.Combos.Count >= MAX_COMBO_IN_DECK)
            {
                OnFailedComboAdd?.Invoke("MAX_COMBO_IN_DECK");
                return;
            }
            _deck.AddCombo(comboData);
            OnSuccessComboAdd?.Invoke(comboData);
        }

        private void TryRemoveCard(MetaCardData cardData)
        {
            if (_deck.FindMetaCardData(cardData.CardInstance.ID,out MetaCardData metaCardData))
            {
                _deck.RemoveCard(metaCardData);
                OnSuccessCardRemove?.Invoke(metaCardData);
            }
        }

        private void TryRemoveCombo(MetaComboData comboData)
        {
            if (_deck.FindMetaComboData(comboData.ID,out MetaComboData metaComboData))
            {
                _deck.RemoveCombo(metaComboData);
                OnSuccessComboRemove?.Invoke(metaComboData);
            }
        }

        public bool TryApplyDeck(out MetaDeckData metaDeckData)
        {
            if (_deck.Cards.Count == MAX_CARD_IN_DECK && _deck.Combos.Count <= MAX_COMBO_IN_DECK)
            {
                metaDeckData = _deck;
                return true;
            }

            metaDeckData = null;
            return false;
        }

       
    }
}