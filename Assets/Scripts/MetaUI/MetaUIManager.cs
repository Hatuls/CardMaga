using System;
using System.Collections.Generic;
using CardMaga.Meta.Upgrade;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaUI.CollectionUI;
using CardMaga.ObjectPool;
using CardMaga.SequenceOperation;
using MetaData;
using ReiTools.TokenMachine;
using UnityEngine;

namespace CardMaga.MetaUI
{
    public class MetaUIManager : MonoSingleton<MetaUIManager>, ISequenceOperation<MetaDataManager>
    {
        public static event Action OnMetaUIInitializes;
        [SerializeField] private UpgradeUIManager _upgradeUIManager;
        [SerializeField] private MetaDeckCollectionUIManager _metaDeckCollectionUIManager;
        [SerializeField] private MetaCharacterCollectionManager _metaCharacterCollectionManager;
        private VisualRequester<MetaComboUI, MetaComboData> _comboVisualRequester;
        private VisualRequester<MetaCardUI, MetaCardData> _cardVisualRequester;
        
        private MetaDataManager _metaDataManager;
        private SequenceHandler<MetaUIManager> _sequenceHandler = new SequenceHandler<MetaUIManager>();
        
        public int Priority => 1;
        public VisualRequester<MetaComboUI, MetaComboData> ComboVisualRequester => _comboVisualRequester;
        public VisualRequester<MetaCardUI, MetaCardData> CardVisualRequester => _cardVisualRequester;

        public MetaDeckCollectionUIManager MetaDeckCollectionUIManager => _metaDeckCollectionUIManager;
        public UpgradeUIManager UpgradeUIManager => _upgradeUIManager;
        public MetaCharacterCollectionManager MetaCharacterCollectionManager => _metaCharacterCollectionManager;

        public MetaDataManager MetaDataManager => _metaDataManager;
        
        private IEnumerable<ISequenceOperation<MetaUIManager>> VisualInitializers
        {
            get
            {
                yield return _upgradeUIManager;
                yield return _metaDeckCollectionUIManager;
                yield return _metaCharacterCollectionManager;
            }
        }

        public override void Awake()
        {
            base.Awake();
            _metaDataManager = new MetaDataManager();
            _sequenceHandler.Register(_metaDataManager,OrderType.Before);
            
            foreach (var operation in VisualInitializers)
            {
                _sequenceHandler.Register(operation);
            }
        }

        public void Register(ISequenceOperation<MetaUIManager> sequenceOperation, OrderType to = OrderType.Default)
        {
            _sequenceHandler.Register(sequenceOperation,to);
        }

        private void Start()
        {
            _sequenceHandler.StartAll(this,MetaUIInitializes);
        }

        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
        }

        private void MetaUIInitializes()
        {
            OnMetaUIInitializes?.Invoke();
        }
    }
}