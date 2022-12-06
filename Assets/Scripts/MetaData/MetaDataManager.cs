using System;
using System.Collections.Generic;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.Collection;
using CardMaga.MetaData.DeckBuilding;
using CardMaga.MetaUI;
using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;

namespace MetaData
{
    public class MetaDataManager : ISequenceOperation<MetaUIManager>
    {
        public static event Action OnDataInitializes;
        
        private SequenceHandler<MetaDataManager> _sequenceHandler;
        private AccountDataAccess _accountDataAccess;
        private AccountDataCollectionHelper _accountDataCollectionHelper;
        private MetaCollectionDataManager _metaCollectionDataManager;
        private DeckBuilder _deckBuilder;

        private IDisposable _token;
        
        public AccountDataCollectionHelper AccountDataCollectionHelper => _accountDataCollectionHelper;
        public MetaCollectionDataManager MetaCollectionDataManager => _metaCollectionDataManager;
        public DeckBuilder DeckBuilder => _deckBuilder;
        public AccountDataAccess AccountDataAccess => _accountDataAccess;
        public MetaAccountData MetaAccountData => _accountDataAccess.AccountData;
        
        public int Priority => 0;

        private IEnumerable<ISequenceOperation<MetaDataManager>> DataInitializers
        {
            get
            {
                yield return _accountDataAccess = new AccountDataAccess();
                yield return _accountDataCollectionHelper = new AccountDataCollectionHelper();
                yield return _metaCollectionDataManager = new MetaCollectionDataManager();
                yield return _deckBuilder = new DeckBuilder();
            }
        }
        
        public void ExecuteTask(ITokenReciever tokenMachine, MetaUIManager data)
        {
            _token = tokenMachine.GetToken();
            InitData();
        }

        private void InitData()
        {
            _sequenceHandler = new SequenceHandler<MetaDataManager>();

            foreach (var operation in DataInitializers)
            {
                _sequenceHandler.Register(operation);
            }
            
            _sequenceHandler.StartAll(this,DataInitializes);
        }

        private void DataInitializes()
        {
            OnDataInitializes?.Invoke();
            _token.Dispose();
        }
    }
}