using System;
using System.Collections.Generic;
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
        [SerializeField] private MetaCollectionUIManager _metaCollectionUIManager;
        [SerializeField] private MetaDeckUICollectionManager _metaDeckUICollectionManager;
        private VisualRequester<MetaComboUI, MetaComboData> _comboVisualRequester;
        private VisualRequester<MetaCardUI, MetaCardData> _cardVisualRequester;
        
        private MetaDataManager _metaDataManager;
        private SequenceHandler<MetaUIManager> _sequenceHandler = new SequenceHandler<MetaUIManager>();
        
        public int Priority => 1;
        public VisualRequester<MetaComboUI, MetaComboData> ComboVisualRequester => _comboVisualRequester;
        public VisualRequester<MetaCardUI, MetaCardData> CardVisualRequester => _cardVisualRequester;

        public MetaCollectionUIManager MetaCollectionUIManager => _metaCollectionUIManager;

        public MetaDeckUICollectionManager MetaDeckUICollectionManager => _metaDeckUICollectionManager;

        public MetaDataManager MetaDataManager => _metaDataManager;
        
        private IEnumerable<ISequenceOperation<MetaUIManager>> VisualInitializers
        {
            get
            {
                yield return _metaCollectionUIManager;
                yield return _metaDeckUICollectionManager;
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
        
        public void LogTest()
        {
            Debug.Log("Test_needToRemove");
        }

    }
}