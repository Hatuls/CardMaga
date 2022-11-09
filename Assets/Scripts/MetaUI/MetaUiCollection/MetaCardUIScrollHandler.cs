using CardMaga.Meta.AccountMetaData;
using CardMaga.MetaData.Collection;
using CardMaga.UI.MetaUI;
using UnityEngine;

namespace CardMaga.UI.ScrollPanel
{
    public class MetaCardUIScrollHandler : BaseScrollPanelManager<MetaCollectionCardUI,MetaCollectionCardData>
    {

        [SerializeField] private MetaCollectionCardUIPool metaCollectionCardUIPool;
        protected override BasePoolObject<MetaCollectionCardUI, MetaCollectionCardData> ObjectPool => metaCollectionCardUIPool;
    }
}

