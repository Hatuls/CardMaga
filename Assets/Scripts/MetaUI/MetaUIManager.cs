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
        public event Action<MetaUIManager> OnMetaUIManagerDestroyed;

        [SerializeField] private MetaDataManager _metaDataManager;
        [SerializeField] private MetaDeckEditingUIManager _metaDeckEditingUIManager;
        [SerializeField] private MetaCharacterScreenUIManager _metaCharacterScreenUIManager;
        [SerializeField] private DismantelUIManager _dismantelUIManager;
        [SerializeField] private UpgradeUIManager _upgradeUIManager;



        private SequenceHandler<MetaUIManager> _sequenceHandler = new SequenceHandler<MetaUIManager>();

        public int Priority => 1;

        public MetaDeckEditingUIManager MetaDeckEditingUIManager => _metaDeckEditingUIManager;
        public MetaCharacterScreenUIManager MetaCharacterScreenUIManager => _metaCharacterScreenUIManager;
        public DismantelUIManager DismantelUIManager => _dismantelUIManager;
        public MetaDataManager MetaDataManager => _metaDataManager;


        private IEnumerable<ISequenceOperation<MetaUIManager>> VisualInitializers
        {
            get
            {
                yield return _upgradeUIManager;
                yield return _metaDeckEditingUIManager;
                yield return _metaCharacterScreenUIManager;
                yield return _dismantelUIManager;

            }
        }

        public override void Awake()
        {
            base.Awake();

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
            _metaDataManager = new MetaDataManager();
            _metaDataManager.InitData();
            _sequenceHandler.StartAll(this, MetaUIInitializes);
        }

        public void ExecuteTask(ITokenReceiver tokenMachine, MetaDataManager data)
        {
        }

        private void MetaUIInitializes()
        {
            OnMetaUIInitializes?.Invoke();
        }

        private void OnDestroy()
        {
            OnMetaUIManagerDestroyed?.Invoke(this);
        }

        #region Editor:
#if UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
        private void TryAssignReferences()
        {
            _metaDeckEditingUIManager = FindObjectOfType<MetaDeckEditingUIManager>();
            _metaCharacterScreenUIManager = FindObjectOfType<MetaCharacterScreenUIManager>();
            _dismantelUIManager = FindObjectOfType<DismantelUIManager>();
            _upgradeUIManager = FindObjectOfType<UpgradeUIManager>();
        }
#endif
        #endregion
    }
}