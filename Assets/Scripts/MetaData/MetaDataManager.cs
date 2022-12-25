using System;
using System.Collections.Generic;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.Collection;
using CardMaga.MetaData.DeckBuilding;
using CardMaga.MetaData.Dismantle;
using CardMaga.MetaUI;
using CardMaga.ObjectPool;
using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;

namespace MetaData
{
    [Serializable]
    public class MetaDataManager : ISequenceOperation<MetaUIManager>
    {
        public static event Action OnDataInitializes;
        
        private SequenceHandler<MetaDataManager> _sequenceHandler;
        private AccountDataAccess _accountDataAccess;
        private AccountDataCollectionHelper _accountDataCollectionHelper;
        private DeckEditingDataManager _deckEditingDataManager;
        private DeckBuilder _deckBuilder;
        private DismantleDataManager _dismantleDataManager;
        private VisualRequesterManager _visualRequester = VisualRequesterManager.Instance;

        private IDisposable _token;
        
        public AccountDataCollectionHelper AccountDataCollectionHelper => _accountDataCollectionHelper;
        public DeckEditingDataManager DeckEditingDataManager => _deckEditingDataManager;
        public DeckBuilder DeckBuilder => _deckBuilder;
        public AccountDataAccess AccountDataAccess => _accountDataAccess;
        public MetaAccountData MetaAccountData => _accountDataAccess.AccountData;
        public DismantleDataManager DismantleDataManager => _dismantleDataManager;
        
        public int Priority => 0;

        private IEnumerable<ISequenceOperation<MetaDataManager>> DataInitializers
        {
            get
            {
                //yield return _visualRequester;
                yield return _accountDataAccess = new AccountDataAccess();
                yield return _accountDataCollectionHelper = new AccountDataCollectionHelper();
                yield return _deckEditingDataManager = new DeckEditingDataManager();
                yield return _deckBuilder = new DeckBuilder();
                yield return _dismantleDataManager = new DismantleDataManager();
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