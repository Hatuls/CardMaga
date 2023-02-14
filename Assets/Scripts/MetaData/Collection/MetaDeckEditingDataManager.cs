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
        private MetaAccountDataManager _metaAccountDataManager;
        private MetaDeckData _metaDeckData;
        private MetaDeckData _metaDeckDataCopy;
        private AccountCollectionCopyHelper _collectionCopy;
        private AccountDataCollectionHelper _accountDataCollectionHelper;

        private bool _isDefaultDeck;

        private bool _isDeckUpdateToAccount;
        
        public CardsCollectionDataHandler CardCollectionDataHandler => _collectionCopy.CardsCollectionDataHandler;
        public ComboCollectionDataHandler ComboCollectionDataHandler => _collectionCopy.ComboCollectionDataHandler;
        
        public MetaDeckData MetaDeckData => _metaDeckDataCopy;
        
        public int Priority => 2;
        
        public void ExecuteTask(ITokenReceiver tokenMachine, MetaDataManager data)
        {
            _metaAccountDataManager = data.AccountDataManager;
            _deckBuilder = data.DeckBuilder;
            _accountDataCollectionHelper = data.AccountDataCollectionHelper;
            _isDeckUpdateToAccount = true;
        }
        
        public void AssignDeckDataToEdit()
        {
            if (_isDeckUpdateToAccount)
            {
                var metaDeckData = _metaAccountDataManager.MetaAccountData.CharacterDatas.MainCharacterData.MainDeck;
                _collectionCopy = _accountDataCollectionHelper.GetCollectionCopy(metaDeckData);
                _isDefaultDeck = metaDeckData.DeckId == 0;
                _isDeckUpdateToAccount = false;
            }

            _deckBuilder.AssignDeckToEdit(_collectionCopy.MetaDeckData,_collectionCopy.CardsCollectionDataHandler,_collectionCopy.ComboCollectionDataHandler);
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
            _metaAccountDataManager.UpdateDeck(_metaDeckData,tokenMachine);
            _isDeckUpdateToAccount = true;
        }

        public void DiscardDeck()
        {
            _metaDeckData = null;
            _metaDeckDataCopy = null;
            _isDeckUpdateToAccount = true;
            
            _deckBuilder.ResetDeckEditing();
        }

        public void Dispose()
        {
        }
    }
}

