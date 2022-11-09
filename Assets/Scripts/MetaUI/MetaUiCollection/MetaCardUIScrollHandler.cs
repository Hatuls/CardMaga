using CardMaga.Meta.AccountMetaData;
using CardMaga.UI.MetaUI;
using UnityEngine;

namespace CardMaga.UI.ScrollPanel
{
    public class MetaCardUIScrollHandler : BaseScrollPanelManager<MetaCardUI,MetaCardData>
    {

        [SerializeField] private MetaCardUIPool _metaCardUIPool;
        protected override BasePoolObject<MetaCardUI, MetaCardData> ObjectPool => _metaCardUIPool;
    }
}

