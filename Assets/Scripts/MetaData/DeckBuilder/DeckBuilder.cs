using System;
using Account.GeneralData;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.Collection;
using CardMaga.SequenceOperation;
using CardMaga.ValidatorSystem;
using MetaData;
using ReiTools.TokenMachine;
using UnityEngine;

namespace CardMaga.MetaData.DeckBuilding
{
    public class DeckBuilder : ISequenceOperation<MetaDataManager>
    {
        #region Events
        public event Action OnDeckNameUpdate;
        public event Action<IValidFailedInfo> OnFailedToAddCombo; 
        public event Action<IValidFailedInfo> OnFailedToAddCard; 
        public event Action<IValidFailedInfo> OnFailedToUpdateDeckName; 
        public event Action<MetaDeckData> OnNewDeckLoaded; 
        public event Action<MetaCardInstanceInfo> OnSuccessfulCardAdd;
        public event Action<MetaCardInstanceInfo> OnSuccessfulCardRemove;
        public event Action<MetaComboInstanceInfo> OnSuccessfulComboAdd;
        public event Action<MetaComboInstanceInfo> OnSuccessfulComboRemove;

        #endregion

        #region Fields

        private const int MAX_CARD_IN_DECK = 8;
        private const int MAX_COMBO_IN_DECK = 3;

        private MetaDeckData _deck;

        private CardsCollectionDataHandler _cardsCollectionDataHandler;
        private ComboCollectionDataHandler _comboCollectionDataHandler;
        
        #endregion
        
        public void ExecuteTask(ITokenReceiver tokenMachine, MetaDataManager data)
        {
        }

        public int Priority => 1;

        public void AssignDeckToEdit(MetaDeckData deckData,CardsCollectionDataHandler cardsCollectionDataHandler,ComboCollectionDataHandler comboCollectionDataHandler)
        {
            _deck = deckData;
            
            _cardsCollectionDataHandler = cardsCollectionDataHandler;
            _comboCollectionDataHandler = comboCollectionDataHandler;

            foreach (var cardData in _cardsCollectionDataHandler.CollectionCardDatas.Values)
            {
                cardData.OnTryAddItemToCollection += TryAddCard;
                cardData.OnTryRemoveItemFromCollection += TryRemoveCard;
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

        public void ResetDeckEditing()
        {
            foreach (var cardData in _cardsCollectionDataHandler.CollectionCardDatas.Values)
            {
                cardData.OnTryAddItemToCollection -= TryAddCard;
                cardData.OnTryRemoveItemFromCollection -= TryRemoveCard;
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
            
            _deck = null;
            _cardsCollectionDataHandler = null;
            _comboCollectionDataHandler = null;
        }

        public bool TryApplyDeck(out MetaDeckData metaDeckData)
        {
            if (Validator.Valid(_deck,out var validInfo,ValidationTag.MetaDeckDataSystem))
            {
                metaDeckData = _deck;
                return true;
            }
            
            metaDeckData = _deck;
            return false;
        }

        #region DeckEditing
        
        public void TryEditDeckName(string name)
        {
            //if (!Validator.Valid(name,out IValidFailedInfo validInfo,default)) need to conect!!!!
            //{
            //    OnFailedToUpdateDeckName?.Invoke(validInfo);
            //    return;
           // }
            
            _deck.UpdateDeckName(name);
            OnDeckNameUpdate?.Invoke();
        }

        private void TryAddCard(MetaCardInstanceInfo cardInstance)
        {
            if (_deck.Cards.Count >= MAX_CARD_IN_DECK)
            {
                //OnFailedToAddCard?.Invoke("Max card in deck");
                return;
            }
            
            _deck.AddCard(cardInstance);
            
            if (!Validator.Valid(_deck,out IValidFailedInfo validInfo,ValidationTag.MetaDeckDataGameDesign))
            {
                OnFailedToAddCard?.Invoke(validInfo);
                return;
            }

            if (_cardsCollectionDataHandler.TryRemoveCardInstance(cardInstance.InstanceID,false))
            {
                //_deck.AddCard(cardInstance);
                OnSuccessfulCardAdd?.Invoke(cardInstance);
            }
            else
            {
                Debug.LogError("Failed to remove card from collection");
            }
        }

        private void TryAddCombo(MetaComboInstanceInfo comboInstance)
        {
            if (_deck.Combos.Count >= MAX_COMBO_IN_DECK)
            {
                //OnFailedToAddCombo?.Invoke("Max combo in deck");
                return;
            }

            _deck.AddCombo(comboInstance);
            
            if (!Validator.Valid(_deck,out IValidFailedInfo validInfo,ValidationTag.MetaDeckDataGameDesign))
            {
                OnFailedToAddCombo?.Invoke(validInfo);
                return;
            }

            if (_comboCollectionDataHandler.TryRemoveComboCollection(comboInstance.CoreID))
            {
                //_deck.AddCombo(comboInstance);
                OnSuccessfulComboAdd?.Invoke(comboInstance);
            }
        }

        private void TryRemoveCard(CardCore cardCore)
        {
            if (_deck.FindCardData(cardCore.CoreID,out MetaCardInstanceInfo cardInstance))
            {
                _deck.RemoveCard(cardInstance);
                _cardsCollectionDataHandler.AddCardInstance(cardInstance);
               
                OnSuccessfulCardRemove?.Invoke(cardInstance);
            }
        }

        private void TryRemoveCombo(ComboCore comboCore)
        {
            if (_deck.FindComboData(comboCore.CoreID,out MetaComboInstanceInfo comboInstance))
            {
                _comboCollectionDataHandler.AddComboCollection(comboInstance);
                _deck.RemoveCombo(comboInstance);
                OnSuccessfulComboRemove?.Invoke(comboInstance);
            }
        }

        #endregion
    }
}