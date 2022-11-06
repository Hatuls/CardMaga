using CardMaga.Meta.AccountMetaData;
using MetaUI.MetaCardUI;
using UnityEngine;

public class MetaCardUIScrollHandler : BaseScrollPanelManager<MetaCardUI,MetaCardData>
{

    [SerializeField] private MetaCardUIPool _metaCardUIPool;
    protected override BasePoolObject<MetaCardUI, MetaCardData> ObjectPool => _metaCardUIPool;
}
