using CardMaga.Meta.AccountMetaData;
using CardMaga.UI.MetaUI;
using UnityEngine;

namespace CardMaga.UI.ScrollPanel
{
    public class MetaComboUIScrollHandler : BaseScrollPanelManager<MetaComboUI,MetaComboData>
    {
        [SerializeField] private MetaComboUIPool _metaComboUIPool;

        protected override BasePoolObject<MetaComboUI, MetaComboData> ObjectPool => _metaComboUIPool;
    }
}

