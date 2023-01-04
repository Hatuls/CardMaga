using System;
using Account.GeneralData;
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
        public event Action<CardInstance> OnSuccessCardAdd;
        public event Action<string> OnFailedCardAdd;
        public event Action<CardInstance> OnSuccessCardRemove;
        public event Action<ComboCore> OnSuccessComboAdd;
        public event Action<string> OnFailedComboAdd; 
        public event Action<ComboCore> OnSuccessComboRemove;
        public event Action<MetaDeckData> OnNewDeckLoaded; 

        #endregion
        
        private const int MAX_CARD_IN_DECK = 8;
        private const int MAX_COMBO_IN_DECK = 3;

        private MetaDeckData _deck;

        private MetaDeckEditingDataManager _deckEditingData;

        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
            _deckEditingData = data.MetaDeckEditingDataManager;
        }

        public int Priority => 1;

        public void AssingDeckToEdit(MetaDeckData deckData)
        {
            _deck = deckData;

            foreach (var cardData in _deckEditingData.MetaCollectionCardDatas)
            {
                cardData.OnTryAddItemToCollection += TryAddCard;
                cardData.OnTryRemoveItemFromCollection += TryRemoveCard;
            }
            
            foreach (var comboData in _deckEditingData.MetaCollectionComboDatas)
            {
                comboData.OnTryAddItemToCollection += TryAddCombo;
                comboData.OnTryRemoveItemFromCollection += TryRemoveCombo;
            }
            
            OnNewDeckLoaded?.Invoke(_deck);
        }

        public void DisposeDeck()//Need to call
        {
            foreach (var cardData in _deckEditingData.MetaCollectionCardDatas)
            {
                cardData.OnTryAddItemToCollection -= TryAddCard;
                cardData.OnTryRemoveItemFromCollection -= TryRemoveCard;
                
            }
            
            foreach (var comboData in _deckEditingData.MetaCollectionComboDatas)
            {
                comboData.OnTryAddItemToCollection -= TryAddCombo;
                comboData.OnTryRemoveItemFromCollection -= TryRemoveCombo;
            }
        }
        
        public void TryEditDeckName(string name)
        {
            //add name valid
            _deck.UpdateDeckName(name);
            OnDeckNameUpdate?.Invoke();
        }

        private void TryAddCard(MetaCollectionCardData collectionCardData)
        {
            if (_deck.Cards.Count >= MAX_CARD_IN_DECK)
            {
                OnFailedCardAdd?.Invoke("MAX_CARD_IN_DECK");
                return;
            }

            var cache = collectionCardData.GetCardInstanceData();
            collectionCardData.AddItemToCollection();
            _deck.AddCard(cache);
            OnSuccessCardAdd?.Invoke(cache);
        }

        private void TryAddCombo(MetaCollectionComboData collectionComboData)
        {
            if (_deck.Combos.Count >= MAX_COMBO_IN_DECK)
            {
                OnFailedComboAdd?.Invoke("MAX_COMBO_IN_DECK");
                return;
            }
            
            collectionComboData.AddItemToCollection();
            _deck.AddCombo(collectionComboData.ComboData);
            OnSuccessComboAdd?.Invoke(collectionComboData.ComboData);
        }

        private void TryRemoveCard(MetaCollectionCardData collectionCardData)
        {
            if (_deck.FindCardData(collectionCardData.CoreId,out CardInstance metaCardData))
            {
                collectionCardData.RemoveItemFromCollection();
                _deck.RemoveCard(metaCardData);
                OnSuccessCardRemove?.Invoke(metaCardData);
            }
        }

        private void TryRemoveCombo(MetaCollectionComboData collectionComboData)
        {
            if (_deck.FindComboData(collectionComboData.ComboID,out ComboCore comboData))
            {
                collectionComboData.RemoveItemFromCollection();
                _deck.RemoveCombo(comboData);
                OnSuccessComboRemove?.Invoke(comboData);
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