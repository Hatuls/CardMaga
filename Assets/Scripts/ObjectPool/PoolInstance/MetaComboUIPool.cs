
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaUI;
using UnityEngine;

public class MetaComboUIPool : BasePoolObjectVisualToData<MetaComboUI, MetaComboData>
{
    public MetaComboUIPool(MetaComboUI objectPrefab, RectTransform parent) : base(objectPrefab, parent)
    {
    }
}
