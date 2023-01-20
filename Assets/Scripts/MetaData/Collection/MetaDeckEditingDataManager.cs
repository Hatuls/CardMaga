using System;
using System.Collections.Generic;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.DeckBuilding;
using CardMaga.SequenceOperation;
using MetaData;
using ReiTools.TokenMachine;

namespace CardMaga.MetaData.Collection
{
    [Serializable]
    public class MetaDeckEditingDataManager : ISequenceOperation<MetaDataManager>, IDisposable
    {
        public event Action OnSuccessUpdateDeck;
        public event Action OnFailedUpdateDeck;
        
        private DeckBuilder _deckBuilder;
        private AccountDataAccess _accountDataAccess;
        private MetaDeckData _metaDeckData;
        private AccountDataCollectionHelper _accountDataCollectionHelper;

        private CardsCollectionDataHandler _cardCollectionDataHandler;
        private ComboCollectionDataHandler _comboCollectionDataHandler;
        
        public CardsCollectionDataHandler CardCollectionDataHandler => _cardCollectionDataHandler;
        public ComboCollectionDataHandler ComboCollectionDataHandler => _comboCollectionDataHandler;
        
        public MetaDeckData MetaDeckData => _metaDeckData;
        
        public int Priority => 2;
        
        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
            _accountDataAccess = data.AccountDataAccess;
            _deckBuilder = data.DeckBuilder;
            _accountDataCollectionHelper = data.AccountDataCollectionHelper;
        }
        
        public void AssingDeckDataToEdit()
        {
            _metaDeckData = _accountDataAccess.AccountData.CharacterDatas.CharacterData.MainDeck;

            _cardCollectionDataHandler = _accountDataCollectionHelper.GetCardCollectionByDeck(_metaDeckData.DeckId);
            _comboCollectionDataHandler = _accountDataCollectionHelper.GetComboCollectionByDeck(_metaDeckData.DeckId);
            
            _deckBuilder.AsingDeckToEdit(_metaDeckData,_cardCollectionDataHandler,_comboCollectionDataHandler);
        }

        public void ExitDeckEditing()
        {
            _deckBuilder.TryApplyDeck();
        }

        private void FailedToUpdateDeck()
        {
            
        }
        
        private void FailedToUpdateDefaultDeck()
        {
            
        }

        private void SuccessUpdateDeck(MetaDeckData metaDeckData)
        {
            TokenMachine tokenMachine = new TokenMachine(OnSuccessUpdateDeck);
            _accountDataAccess.UpdateDeck(metaDeckData,tokenMachine);
            _deckBuilder.ResetDeckEditing();
        }

        public void DiscardDeck()
        {
            _deckBuilder.ResetDeckEditing();
        }

        public void Dispose()
        {
        }
    }
}

