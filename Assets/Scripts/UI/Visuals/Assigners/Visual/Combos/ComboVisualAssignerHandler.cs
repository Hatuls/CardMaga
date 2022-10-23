using Battle.Combo;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class ComboVisualAssignerHandler : BaseVisualAssignerHandler<ComboData>
    {
        [Header("Visuals")]
        [SerializeField] ComboTypeVisualAssigner _comboTypeVisualAssigners;
        [SerializeField] ComboSequenceVisualAssigner _comboSequenceVisualAssigner;
        public override IEnumerable<BaseVisualAssigner<ComboData>> VisualAssigners
        {
             get
            {
                yield return _comboTypeVisualAssigners;
                yield return _comboSequenceVisualAssigner;
            }
        }
    }
}