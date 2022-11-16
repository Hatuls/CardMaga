using Battle.Combo;
using UnityEngine;

namespace CardMaga.UI.Combos
{
    public class BattleComboUIPool : BasePoolObjectVisualToData<BattleComboUI, BattleComboData>
    {
        public BattleComboUIPool(BattleComboUI objectPrefab, RectTransform parent) : base(objectPrefab, parent)
        {
        }
    }

}