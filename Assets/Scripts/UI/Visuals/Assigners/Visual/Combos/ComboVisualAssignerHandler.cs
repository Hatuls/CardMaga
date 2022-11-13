using Battle.Combo;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class ComboVisualAssignerHandler : BaseVisualAssignerHandler<BattleComboData>
    {
        [Header("Visuals")]
        [SerializeField] ComboTypeVisualAssigner _comboTypeVisualAssigner;
        [SerializeField] ComboSequenceVisualAssigner _comboSequenceVisualAssigner;
        [SerializeField] ComboTitleAndArrowVisualAssigner _comboTitleAndArrowVisualAssigner;
        public override IEnumerable<BaseVisualAssigner<BattleComboData>> VisualAssigners
        {
             get
            {
                yield return _comboTypeVisualAssigner;
                yield return _comboSequenceVisualAssigner;
                yield return _comboTitleAndArrowVisualAssigner;
            }
        }
    }
}