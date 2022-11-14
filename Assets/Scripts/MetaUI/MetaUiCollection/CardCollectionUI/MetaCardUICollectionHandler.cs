using CardMaga.MetaData.Collection;
using CardMaga.UI.ScrollPanel;
using UnityEngine;

namespace CardMaga.MetaUI.CollectionUI
{
    public class MetaCardUICollectionHandler : BaseScrollPanelManager<MetaCollectionCardUI,MetaCollectionCardData>
    {

        [SerializeField] private MetaCollectionCardUIPool metaCollectionCardUIPool;
        protected override BasePoolObject<MetaCollectionCardUI, MetaCollectionCardData> ObjectPool => metaCollectionCardUIPool;
    }
}

