using Battle.Combo;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class ComboVisualAssignerHandler : BaseVisualAssignerHandler<Combo>
    {
        [Header("Visuals")]
        [SerializeField] ComboTypeVisualAssigner _comboTypeVisualAssigners;
        [SerializeField] ComboSequenceVisualAssigner _comboSequenceVisualAssigner;
        public override IEnumerable<BaseVisualAssigner<Combo>> VisualAssigners
        {
             get
            {
                yield return _comboTypeVisualAssigners;
                yield return _comboSequenceVisualAssigner;
            }
        }
    }
}