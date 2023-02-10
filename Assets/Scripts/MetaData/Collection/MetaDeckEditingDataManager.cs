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
        public event Action<bool> OnFailedUpdateDeck;
        
        private DeckBuilder _deckBuilder;
        private AccountDataAccess _accountDataAccess;
        private MetaDeckData _metaDeckData;
        private MetaDeckData _metaDeckDataCopy;
        private AccountDataCollectionHelper _accountDataCollectionHelper;

        private CardsCollectionDataHandler _cardCollectionDataHandler;
        private ComboCollectionDataHandler _comboCollectionDataHandler;

        private bool _isDefaultDeck;

        private bool _isDeckUpdateToAccount;
        
        public CardsCollectionDataHandler CardCollectionDataHandler => _cardCollectionDataHandler;
        public ComboCollectionDataHandler ComboCollectionDataHandler => _comboCollectionDataHandler;
        
        public MetaDeckData MetaDeckData => _metaDeckDataCopy;
        
        public int Priority => 2;
        
        public void ExecuteTask(ITokenReceiver tokenMachine, MetaDataManager data)
        {
            _accountDataAccess = data.AccountDataAccess;
            _deckBuilder = data.DeckBuilder;
            _accountDataCollectionHelper = data.AccountDataCollectionHelper;
            _isDeckUpdateToAccount = true;
        }
        
        public void AssignDeckDataToEdit()
        {
            if (_isDeckUpdateToAccount)
            {
                _metaDeckData = _accountDataAccess.AccountData.CharacterDatas.MainCharacterData.MainDeck;
                _metaDeckDataCopy = _metaDeckData.GetCopy();
                _isDefaultDeck = _metaDeckData.DeckId == 0;
                _isDeckUpdateToAccount = false;
            }
            
            _cardCollectionDataHandler = _accountDataCollectionHelper.GetCardCollectionByDeck(_metaDeckDataCopy.DeckId);
            _comboCollectionDataHandler = _accountDataCollectionHelper.GetComboCollectionByDeck(_metaDeckDataCopy.DeckId);
            
            _accountDataCollectionHelper.CollectionCopy.SetData(_metaDeckDataCopy,_cardCollectionDataHandler,_comboCollectionDataHandler);
            _deckBuilder.AssignDeckToEdit(_metaDeckDataCopy,_cardCollectionDataHandler,_comboCollectionDataHandler);
        }

        public void ExitDeckEditing()
        {
            if (_deckBuilder.TryApplyDeck(out MetaDeckData metaDeckData))
            {
                _metaDeckData = metaDeckData;
                UpdateDeck();
            }
            else
            {
                _metaDeckData = metaDeckData;
                OnFailedUpdateDeck?.Invoke(_isDefaultDeck);
            }
        }

        public void UpdateDeck()
        {
            TokenMachine tokenMachine = new TokenMachine(OnSuccessUpdateDeck);
            _accountDataAccess.UpdateDeck(_metaDeckData,tokenMachine);
            _accountDataCollectionHelper.UpdateCollection();
            _isDeckUpdateToAccount = true;
        }

        public void DiscardDeck()
        {
            _metaDeckData = null;
            _metaDeckDataCopy = null;
            _cardCollectionDataHandler = null;
            _comboCollectionDataHandler = null;
            _isDeckUpdateToAccount = true;
            
            _deckBuilder.ResetDeckEditing();
        }

        public void Dispose()
        {
        }
    }
}

