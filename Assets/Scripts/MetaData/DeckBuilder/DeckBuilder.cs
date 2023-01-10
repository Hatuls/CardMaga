using System;
using System.Collections.Generic;
using Account.GeneralData;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.Collection;
using CardMaga.SequenceOperation;
using CardMaga.ValidatorSystem;
using MetaData;
using ReiTools.TokenMachine;

namespace CardMaga.MetaData.DeckBuilding
{
    public class DeckBuilder : ISequenceOperation<MetaDataManager>
    {
        #region Events
        public event Action OnDeckNameUpdate; 
        public event Action<string> OnFailedToAddCombo; 
        public event Action<string> OnFailedToAddCard; 
        public event Action<string> OnFailedToUpdateDeckName; 
        public event Action<MetaDeckData> OnNewDeckLoaded; 
        public event Action<CardInstance> OnSuccessfulCardAdd;
        public event Action<CardInstance> OnSuccessfulCardRemove;
        public event Action<ComboInstance> OnSuccessfulComboAdd;
        public event Action<ComboInstance> OnSuccessfulComboRemove;

        #endregion
        
        private const int MAX_CARD_IN_DECK = 8;
        private const int MAX_COMBO_IN_DECK = 3;

        private MetaDeckData _deck;

        private CardsCollectionDataHandler _originalCardsCollection;
        private ComboCollectionDataHandler _originalComboCollection;
        
        private CardsCollectionDataHandler _cardsCollectionDataHandler;
        private ComboCollectionDataHandler _comboCollectionDataHandler;
        
        private TypeValidator<MetaDeckData> _deckValidator;
        private TypeValidator<string> _deckNameValidator;
        
        private List<BaseValidatorCondition<MetaDeckData>> _deckValidatorConditions =
            new List<BaseValidatorCondition<MetaDeckData>>()
            {
                //add validation
            };

        private List<BaseValidatorCondition<string>> _deckNameValidatorConditions =
            new List<BaseValidatorCondition<string>>()
            {
                //add validation
            };
        
        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
            _originalCardsCollection = data.AccountDataCollectionHelper.CollectionCardDatasHandler;
            _comboCollectionDataHandler = data.AccountDataCollectionHelper.CollectionComboDatasHandler;
            
            _deckValidator = new TypeValidator<MetaDeckData>(_deckValidatorConditions);
            _deckNameValidator = new TypeValidator<string>(_deckNameValidatorConditions);
        }

        public int Priority => 1;

        public void AssingDeckToEdit(MetaDeckData deckData,CardsCollectionDataHandler cardsCollectionDataHandler,ComboCollectionDataHandler comboCollectionDataHandler)
        {
            _deck = deckData;

            _cardsCollectionDataHandler = cardsCollectionDataHandler;
            _comboCollectionDataHandler = comboCollectionDataHandler;

            foreach (var cardData in _cardsCollectionDataHandler.CollectionCardDatas)
            {
                cardData.OnTryAddItemToCollection += TryAddCardToDeck;
                cardData.OnTryRemoveItemFromCollection += TryRemoveCardFromDeck;
                OnSuccessfulCardAdd += cardData.SuccessAddOrRemoveFromCollection;
                OnSuccessfulCardRemove += cardData.SuccessAddOrRemoveFromCollection;
            }
            
            foreach (var comboData in _comboCollectionDataHandler.CollectionComboDatas)
            {
                comboData.OnTryAddItemToCollection += TryAddCombo;
                comboData.OnTryRemoveItemFromCollection += TryRemoveCombo;
                OnSuccessfulComboAdd += comboData.SuccessAddOrRemoveFromCollection;
                OnSuccessfulComboRemove += comboData.SuccessAddOrRemoveFromCollection;
            }
            
            OnNewDeckLoaded?.Invoke(_deck);
        }

        public void DisposeDeck()//Need to call
        {
            foreach (var cardData in _cardsCollectionDataHandler.CollectionCardDatas)
            {
                cardData.OnTryAddItemToCollection -= TryAddCardToDeck;
                cardData.OnTryRemoveItemFromCollection -= TryRemoveCardFromDeck;
                OnSuccessfulCardAdd -= cardData.SuccessAddOrRemoveFromCollection;
                OnSuccessfulCardRemove -= cardData.SuccessAddOrRemoveFromCollection;
            }
            
            foreach (var comboData in _comboCollectionDataHandler.CollectionComboDatas)
            {
                comboData.OnTryAddItemToCollection -= TryAddCombo;
                comboData.OnTryRemoveItemFromCollection -= TryRemoveCombo;
                OnSuccessfulComboAdd -= comboData.SuccessAddOrRemoveFromCollection;
                OnSuccessfulComboRemove -= comboData.SuccessAddOrRemoveFromCollection;
            }
        }
        
        public void TryEditDeckName(string name)
        {
            if (_deckNameValidator.Valid(name,out string failedMassage))
            {
                OnFailedToUpdateDeckName?.Invoke(failedMassage);
                return;
            }
            
            _deck.UpdateDeckName(name);
            OnDeckNameUpdate?.Invoke();
        }

        private void TryAddCardToDeck(CardInstance cardInstance)
        {
            if (_deck.Cards.Count >= MAX_CARD_IN_DECK)
            {
                OnFailedToAddCard?.Invoke("Max card in deck");
                return;
            }
            
            _deck.AddCard(cardInstance);
            
            if (!_deckValidator.Valid(_deck,out string failedMassage))
            {
                _deck.RemoveCard(cardInstance);
                OnFailedToAddCard?.Invoke(failedMassage);
                return;
            }

            if (_cardsCollectionDataHandler.TryRemoveCardInstance(cardInstance.InstanceID))
            {
                //_deck.AddCard(cardInstance);
                _originalCardsCollection.AddDeckAssociate(cardInstance,_deck.DeckId);
                OnSuccessfulCardAdd?.Invoke(cardInstance);
            }
        }

        private void TryAddCombo(ComboInstance comboInstance)
        {
            if (_deck.Combos.Count >= MAX_COMBO_IN_DECK)
            {
                OnFailedToAddCombo?.Invoke("Max combo in deck");
                return;
            }

            _deck.AddCombo(comboInstance);
            
            if (_deckValidator.Valid(_deck,out string failedMassage))
            {
                _deck.RemoveCombo(comboInstance);
                OnFailedToAddCombo?.Invoke(failedMassage);
                return;
            }

            if (_comboCollectionDataHandler.TryRemoveComboCollection(comboInstance.CoreID))
            {
                //_deck.AddCombo(comboInstance);
                OnSuccessfulComboAdd?.Invoke(comboInstance);
            }
        }

        private void TryRemoveCardFromDeck(CardCore cardCore)
        {
            if (_deck.FindCardData(cardCore.CoreID,out CardInstance cardInstance))
            {
                _deck.RemoveCard(cardInstance);
                _cardsCollectionDataHandler.AddCardInstance(new MetaCardInstanceInfo(cardInstance));
                _originalCardsCollection.RemoveDeckAssociate(cardInstance,_deck.DeckId);
                OnSuccessfulCardRemove?.Invoke(cardInstance);
            }
        }

        private void TryRemoveCombo(ComboCore comboCore)
        {
            if (_deck.FindComboData(comboCore.CoreID,out ComboInstance comboInstance))
            {
               _comboCollectionDataHandler.AddComboCollection(comboInstance);
                _deck.RemoveCombo(comboInstance);
                OnSuccessfulComboRemove?.Invoke(comboInstance);
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