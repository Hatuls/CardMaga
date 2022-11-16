using CardMaga.MetaData.Collection;
using CardMaga.MetaUI;
using UnityEngine;

public class MetaCollectionCardUIPool : BasePoolObjectVisualToData<MetaCollectionCardUI, MetaCollectionCardData>
{
    public MetaCollectionCardUIPool(MetaCollectionCardUI objectPrefab, RectTransform parent) : base(objectPrefab, parent)
    {
    }
}
