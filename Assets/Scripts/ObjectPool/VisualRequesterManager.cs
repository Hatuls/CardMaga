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
        [SerializeField] private MetaCollectionCardUI _metaCollectionCardWithOutLimitPrefabRef;
        
        private VisualRequester<MetaComboUI, ComboCore> _comboCoreVisualRequester;
        private VisualRequester<MetaComboUI, MetaComboInstanceInfo> _comboInstanceVisualRequester;
        private VisualRequester<MetaCardUI, MetaCardInstanceInfo> _cardVisualRequester;
        private VisualRequester<MetaCollectionCardUI, MetaCollectionCardData> _cardCollectionVisualRequester;
        private VisualRequester<MetaCollectionComboUI, MetaCollectionComboData> _comboCollectionVisualRequester;
        private VisualRequester<MetaCollectionCardUI, MetaCollectionCardData> _cardCollectionWithoutLimitVisualRequester;

        public int Priority => 0;

        public void ExecuteTask(ITokenReceiver tokenMachine, MetaDataManager data)
        {
            
        }

        public override void Awake()
        {
            base.Awake();
            _comboInstanceVisualRequester = new VisualRequester<MetaComboUI, MetaComboInstanceInfo>(_comboPrefabRef);
            _comboCoreVisualRequester = new VisualRequester<MetaComboUI, ComboCore>(_comboPrefabRef);
            _cardVisualRequester = new VisualRequester<MetaCardUI, MetaCardInstanceInfo>(_cardPrefabRef);
            _cardCollectionVisualRequester =
                new VisualRequester<MetaCollectionCardUI, MetaCollectionCardData>(_metaCollectionCardPrefabRef);
            _comboCollectionVisualRequester = new VisualRequester<MetaCollectionComboUI, MetaCollectionComboData>(collectionComboUIPrefabRef);
            _cardCollectionWithoutLimitVisualRequester =
                new VisualRequester<MetaCollectionCardUI, MetaCollectionCardData>(_metaCollectionCardWithOutLimitPrefabRef);
        }

        public List<MetaCardUI> GetMetaCardUIs(List<MetaCardInstanceInfo> metaCardDatas)
        {
           return _cardVisualRequester.GetVisual(metaCardDatas);
        }
        
        public MetaCardUI GetMetaCardUIs(MetaCardInstanceInfo metaCardData)
        {
            return _cardVisualRequester.GetVisual(metaCardData);
        }
        
        public List<MetaCardUI> GetMetaCardUIs(int amount)
        {
            return _cardVisualRequester.GetVisual(amount);
        }

        public List<MetaComboUI> GetMetaComboUIs(List<ComboCore> metaComboDatas)
        {
            return _comboCoreVisualRequester.GetVisual(metaComboDatas);
        }
        
        public List<MetaComboUI> GetMetaComboUIs(List<MetaComboInstanceInfo> metaComboDatas)
        {
            return _comboInstanceVisualRequester.GetVisual(metaComboDatas);
        }
        
        public MetaComboUI GetMetaComboUIs(MetaComboInstanceInfo metaComboData)
        {
            return _comboInstanceVisualRequester.GetVisual(metaComboData);
        }
        
        public List<MetaComboUI> GetMetaComboUIs(int amount)
        {
            return _comboInstanceVisualRequester.GetVisual(amount);
        }

        public List<MetaCollectionCardUI> GetMetaCollectionCardUI(List<MetaCollectionCardData> metaCollectionCardDatas)
        {
            return _cardCollectionVisualRequester.GetVisual(metaCollectionCardDatas);
        }
        
        public List<MetaCollectionCardUI> GetMetaCollectionWithoutLimitCardUI(List<MetaCollectionCardData> metaCollectionCardDatas)
        {
            return _cardCollectionWithoutLimitVisualRequester.GetVisual(metaCollectionCardDatas);
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