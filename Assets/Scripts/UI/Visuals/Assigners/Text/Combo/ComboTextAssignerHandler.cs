
using Battle.Combo;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.UI.Text
{
    [System.Serializable]
    public class ComboTextAssignerHandler : BaseTextAssignerHandler<BattleComboData>
    {
        [Header("Texts")]
        [SerializeField] ComboNameTextAssigner _comboNameTextAssigner;
        [SerializeField] ComboTypeTextAssigner _comboTypeTextAssigner;
        public override IEnumerable<BaseTextAssigner<BattleComboData>> TextAssigners
        {
            get
            {
                yield return _comboNameTextAssigner;
                yield return _comboTypeTextAssigner;
            }
        }
    }
}
