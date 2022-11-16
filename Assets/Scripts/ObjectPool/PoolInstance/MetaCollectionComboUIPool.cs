
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaUI;
using UnityEngine;

namespace CardMaga.UI.Combos
{
    public class MetaCollectionComboUIPool : BasePoolObjectVisualToData<MetaCollectionComboUI, MetaComboData>
    {
        public MetaCollectionComboUIPool(MetaCollectionComboUI objectPrefab, RectTransform parent) : base(objectPrefab, parent)
        {
        }
    }
}