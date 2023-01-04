using CardMaga.Meta.Upgrade;
using CardMaga.MetaUI.CollectionUI;
using CardMaga.SequenceOperation;
using MetaData;
using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.MetaUI
{
    public class MetaUIManager : MonoSingleton<MetaUIManager>, ISequenceOperation<MetaDataManager>
    {
        public static event Action OnMetaUIInitializes;

        [SerializeField] private MetaDataManager _metaDataManager;
        [SerializeField] private MetaDeckBuildingUIManager _metaDeckBuildingUIManager;
        [SerializeField] private MetaCharacterScreenUIManager _metaCharacterScreenUIManager;
        [SerializeField] private DismantelUIManager _dismantelUIManager;
        [SerializeField] private UpgradeUIManager _upgradeUIManager;



        private SequenceHandler<MetaUIManager> _sequenceHandler = new SequenceHandler<MetaUIManager>();

        public int Priority => 1;

        public MetaDeckBuildingUIManager MetaDeckBuildingUIManager => _metaDeckBuildingUIManager;
        public MetaCharacterScreenUIManager MetaCharacterScreenUIManager => _metaCharacterScreenUIManager;
        public DismantelUIManager DismantelUIManager => _dismantelUIManager;
        public MetaDataManager MetaDataManager => _metaDataManager;


        private IEnumerable<ISequenceOperation<MetaUIManager>> VisualInitializers
        {
            get
            {
                yield return _upgradeUIManager;
                yield return _metaDeckBuildingUIManager;
                yield return _metaCharacterScreenUIManager;
                yield return _dismantelUIManager;

            }
        }

        public override void Awake()
        {
            base.Awake();
            _metaDataManager = new MetaDataManager();
            _sequenceHandler.Register(_metaDataManager, OrderType.Before);

            foreach (var operation in VisualInitializers)
            {
                _sequenceHandler.Register(operation);
            }
        }

        public void Register(ISequenceOperation<MetaUIManager> sequenceOperation, OrderType to = OrderType.Default)
        {
            _sequenceHandler.Register(sequenceOperation, to);
        }

        private void Start()
        {
            _sequenceHandler.StartAll(this, MetaUIInitializes);
        }

        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
        }

        private void MetaUIInitializes()
        {
            OnMetaUIInitializes?.Invoke();
        }


        #region Editor:
#if UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
        private void TryAssignReferences()
        {
            _metaDeckBuildingUIManager = FindObjectOfType<MetaDeckBuildingUIManager>();
            _metaCharacterScreenUIManager = FindObjectOfType<MetaCharacterScreenUIManager>();
            _dismantelUIManager = FindObjectOfType<DismantelUIManager>();
            _upgradeUIManager = FindObjectOfType<UpgradeUIManager>();
        }
#endif
        #endregion
    }
}