using CardMaga.Meta.AccountMetaData;
using CardMaga.MetaUI.MetaComboUI;
using CardMaga.UI.Combos;
using UnityEngine;

namespace CardMaga.UI.ScrollPanel
{
    public class MetaComboUIScrollHandler : BaseScrollPanelManager<MetaCollectionComboUI,MetaComboData>
    {
        [SerializeField] private MetaColletionComboUIPool _metaComboUIPool;

        protected override BasePoolObject<MetaCollectionComboUI, MetaComboData> ObjectPool => _metaComboUIPool;
    }
}

