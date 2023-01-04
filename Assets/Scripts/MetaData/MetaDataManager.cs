using System;
using System.Collections.Generic;
using CardMaga.Meta.Upgrade;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.Collection;
using CardMaga.MetaData.DeckBuilding;
using CardMaga.MetaData.Dismantle;
using CardMaga.MetaUI;
using CardMaga.ObjectPool;
using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;
using UnityEngine;

namespace MetaData
{
    [Serializable]
    public class MetaDataManager : ISequenceOperation<MetaUIManager>
    {
        public static event Action OnDataInitializes;
        
        private SequenceHandler<MetaDataManager> _sequenceHandler;
        [SerializeField] private AccountDataAccess _accountDataAccess;
        [SerializeField] private AccountDataCollectionHelper _accountDataCollectionHelper;
        [SerializeField] private MetaDeckEditingDataManager _metaDeckEditingDataManager;
        [SerializeField] private DeckBuilder _deckBuilder;
        [SerializeField] private DismantleDataManager _dismantleDataManager;
        [SerializeField] private UpgradeManager _upgradeManager;
        
        private IDisposable _token;
        
        public AccountDataCollectionHelper AccountDataCollectionHelper => _accountDataCollectionHelper;
        public MetaDeckEditingDataManager MetaDeckEditingDataManager => _metaDeckEditingDataManager;
        public DeckBuilder DeckBuilder => _deckBuilder;
        public AccountDataAccess AccountDataAccess => _accountDataAccess;
        public MetaAccountData MetaAccountData => _accountDataAccess.AccountData;

        public UpgradeManager UpgradeManager => _upgradeManager;

        public DismantleDataManager DismantleDataManager => _dismantleDataManager;

        public int Priority => 0;

        private IEnumerable<ISequenceOperation<MetaDataManager>> DataInitializers
        {
            get
            {
                yield return _accountDataAccess = new AccountDataAccess();
                yield return _accountDataCollectionHelper = new AccountDataCollectionHelper();
                yield return _metaDeckEditingDataManager = new MetaDeckEditingDataManager();
                yield return _deckBuilder = new DeckBuilder();
                yield return _upgradeManager = new UpgradeManager();
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