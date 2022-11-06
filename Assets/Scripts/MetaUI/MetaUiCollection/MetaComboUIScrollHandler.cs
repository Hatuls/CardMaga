using CardMaga.Meta.AccountMetaData;
using MetaUI.MetaComboUI;
using UnityEngine;

public class MetaComboUIScrollHandler : BaseScrollPanelManager<MetaComboUI,MetaComboData>
{
    [SerializeField] private MetaComboUIPool _metaComboUIPool;

    protected override BasePoolObject<MetaComboUI, MetaComboData> ObjectPool => _metaComboUIPool;
}
