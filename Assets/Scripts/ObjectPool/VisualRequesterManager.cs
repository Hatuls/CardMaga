using System.Collections.Generic;
using Account.GeneralData;
using CardMaga.MetaData.Collection;
using CardMaga.MetaUI;
using CardMaga.SequenceOperation;
using MetaData;
using ReiTools.TokenMachine;
using UnityEngine;

namespace CardMaga.ObjectPool
{
    public class VisualRequesterManager : MonoSingleton<VisualRequesterManager>, ISequenceOperation<MetaDataManager>
    {
        [Header("PreFab Ref")]
        [SerializeField] private MetaCardUI _cardPrefabRef;
        [SerializeField] private MetaComboUI _comboPrefabRef;
        [SerializeField] private MetaCollectionCardUI _metaCollectionCardPrefabRef;
        [SerializeField] private MetaCollectionComboUI collectionComboUIPrefabRef;
        
        private VisualRequester<MetaComboUI, ComboCore> _comboVisualRequester;
        private VisualRequester<MetaCardUI, CardInstance> _cardVisualRequester;
        private VisualRequester<MetaCollectionCardUI, MetaCollectionCardData> _cardCollectionVisualRequester;
        private VisualRequester<MetaCollectionComboUI, MetaCollectionComboData> _comboCollectionVisualRequester;

        public int Priority => 0;

        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
            
        }

        public override void Awake()
        {
            base.Awake();
            _comboVisualRequester = new VisualRequester<MetaComboUI, ComboCore>(_comboPrefabRef);
            _cardVisualRequester = new VisualRequester<MetaCardUI, CardInstance>(_cardPrefabRef);
            _cardCollectionVisualRequester =
                new VisualRequester<MetaCollectionCardUI, MetaCollectionCardData>(_metaCollectionCardPrefabRef);
            _comboCollectionVisualRequester = new VisualRequester<MetaCollectionComboUI, MetaCollectionComboData>(collectionComboUIPrefabRef);
        }

        public List<MetaCardUI> GetMetaCardUIs(List<CardInstance> metaCardDatas)
        {
           return _cardVisualRequester.GetVisual(metaCardDatas);
        }
        
        public List<MetaCardUI> GetMetaCardUIs(int amount)
        {
            return _cardVisualRequester.GetVisual(amount);
        }

        public List<MetaComboUI> GetMetaComboUIs(List<ComboCore> metaComboDatas)
        {
            return _comboVisualRequester.GetVisual(metaComboDatas);
        }
        
        public List<MetaComboUI> GetMetaComboUIs(int amount)
        {
            return _comboVisualRequester.GetVisual(amount);
        }

        public List<MetaCollectionCardUI> GetMetaCollectionCardUI(List<MetaCollectionCardData> metaCollectionCardDatas)
        {
            return _cardCollectionVisualRequester.GetVisual(metaCollectionCardDatas);
        }
        
        public List<MetaCollectionCardUI> GetMetaCollectionCardUI(int amount)
        {
            return _cardCollectionVisualRequester.GetVisual(amount);
        }

        public List<MetaCollectionComboUI> GetMetaCollectionComboUis(
            List<MetaCollectionComboData> metaCollectionComboDatas)
        {
            return _comboCollectionVisualRequester.GetVisual(metaCollectionComboDatas);
        }
        
        public List<MetaCollectionComboUI> GetMetaCollectionComboUis(int amount)
        {
            return _comboCollectionVisualRequester.GetVisual(amount);
        }
    }
}