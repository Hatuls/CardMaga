using CardMaga.MetaData.AccoutData;
using CardMaga.UI.Combos;
using CardMaga.UI.ScrollPanel;
using UnityEngine;

    namespace CardMaga.MetaUI.CollectionUI

{
    public class MetaComboUICollectionHandler : BaseScrollPanelManager<MetaCollectionComboUI,MetaComboData>
    {
        [SerializeField] private MetaColletionComboUIPool _metaComboUIPool;

        protected override BasePoolObject<MetaCollectionComboUI, MetaComboData> ObjectPool => _metaComboUIPool;
    }
}

