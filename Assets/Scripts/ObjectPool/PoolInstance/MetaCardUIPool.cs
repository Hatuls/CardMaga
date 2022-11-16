
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaUI;
using UnityEngine;

public class MetaCardUIPool : BasePoolObjectVisualToData<MetaCardUI, MetaCardData>
{
    public MetaCardUIPool(MetaCardUI objectPrefab, RectTransform parent) : base(objectPrefab, parent)
    {
    }
}
