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
using CardMaga.ValidatorSystem;
using ReiTools.TokenMachine;
using UnityEngine;
using ValidatorSystem.ValidatorTerminals;

namespace MetaData
{
    [Serializable]
    public class MetaDataManager
    {
        public static event Action OnDataInitializes;
        
        private static SequenceHandler<MetaDataManager> _sequenceHandler = new SequenceHandler<MetaDataManager>();
        
        [SerializeField] private AccountDataAccess _accountDataAccess;
        [SerializeField] private AccountDataCollectionHelper _accountDataCollectionHelper;
        [SerializeField] private MetaDeckEditingDataManager _metaDeckEditingDataManager;
        [SerializeField] private DismantleDataManager _dismantleDataManager;

        private MetaValidatorTerminal _validatorTerminal;
        private DeckBuilder _deckBuilder; 
        private UpgradeManager _upgradeManager;


        private IDisposable _token;
        
        public MetaValidatorTerminal ValidatorTerminal => _validatorTerminal;
        public AccountDataCollectionHelper AccountDataCollectionHelper => _accountDataCollectionHelper;
        public MetaDeckEditingDataManager MetaDeckEditingDataManager => _metaDeckEditingDataManager;
        public DeckBuilder DeckBuilder => _deckBuilder;
        public AccountDataAccess AccountDataAccess => _accountDataAccess;
        public MetaAccountData MetaAccountData => _accountDataAccess.AccountData;

        public UpgradeManager UpgradeManager => _upgradeManager;

        public DismantleDataManager DismantleDataManager => _dismantleDataManager;
        
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
                yield return _validatorTerminal = new MetaValidatorTerminal();
            }
        }

        public static void Register(ISequenceOperation<MetaDataManager> sequenceOperation, OrderType to = OrderType.Default)
        {
            _sequenceHandler.Register(sequenceOperation, to);
        }

        public void InitData()
        {
            foreach (var operation in DataInitializers)
            {
                _sequenceHandler.Register(operation);
            }
            
            _sequenceHandler.StartAll(this,DataInitializes);
        }

        private void DataInitializes()
        {
            OnDataInitializes?.Invoke();
        }
    }
}