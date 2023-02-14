using System;
using System.Collections.Generic;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.DeckBuilding;
using CardMaga.SequenceOperation;
using MetaData;
using ReiTools.TokenMachine;

namespace CardMaga.MetaData.Collection
{
    public class MetaDeckEditingDataManager : ISequenceOperation<MetaDataManager>, IDisposable
    {
        public event Action OnSuccessUpdateDeck;
        public event Action<bool> OnFailedUpdateDeck;
        
        private DeckBuilder _deckBuilder;
        private MetaAccountDataManager _metaAccountDataManager;
        private MetaDeckData _metaDeckDataCopy;
        
        private AccountCollectionCopyHelper _collectionCopy;
        private AccountDataCollectionHelper _accountDataCollectionHelper;

        private bool _isDefaultDeck;

        private bool _isDeckUpdateToAccount;

        private CardsCollectionDataHandler _cardsCollectionDataHandler;
        private ComboCollectionDataHandler _comboCollectionDataHandler;

        public CardsCollectionDataHandler CardCollectionDataHandler => _cardsCollectionDataHandler;
        public ComboCollectionDataHandler ComboCollectionDataHandler => _comboCollectionDataHandler;
        
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
            _accountDataCollectionHelper.UpdateCollection();
            _collectionCopy = _accountDataCollectionHelper.GetCollectionCopy();
            
            if (_isDeckUpdateToAccount)
            {
                var metaDeckData = _metaAccountDataManager.MetaAccountData.CharacterDatas.MainCharacterData.MainDeck;
                _isDefaultDeck = metaDeckData.DeckId == 0;
                _metaDeckDataCopy = metaDeckData.GetCopy();
                _isDeckUpdateToAccount = false;
            }

            _cardsCollectionDataHandler = _collectionCopy.GetCardCollectionByDeck(_metaDeckDataCopy.DeckId);
            _comboCollectionDataHandler = _collectionCopy.GetComboCollectionByDeck(_metaDeckDataCopy.DeckId);

            _deckBuilder.AssignDeckToEdit(_metaDeckDataCopy,_cardsCollectionDataHandler,_comboCollectionDataHandler);
        }

        public void ExitDeckEditing()
        {
            if (_deckBuilder.TryApplyDeck(out MetaDeckData metaDeckData))
            {
                _metaDeckDataCopy = metaDeckData;
                UpdateDeck();
            }
            else
            {
                _metaDeckDataCopy = metaDeckData;
                OnFailedUpdateDeck?.Invoke(_isDefaultDeck);
            }
        }

        public void UpdateDeck()
        {
            TokenMachine tokenMachine = new TokenMachine(OnSuccessUpdateDeck);
            _metaAccountDataManager.UpdateDeck(_metaDeckDataCopy,tokenMachine);
            _isDeckUpdateToAccount = true;
        }

        public void DiscardDeck()
        {
            if (_isDefaultDeck)
            {
                _metaAccountDataManager.UpdateDeckAssociate(_metaDeckDataCopy.DeckId);
            }
            _metaDeckDataCopy = null;
            _isDeckUpdateToAccount = true;
            _deckBuilder.ResetDeckEditing();
        }

        public void Dispose()
        {
        }
    }
}