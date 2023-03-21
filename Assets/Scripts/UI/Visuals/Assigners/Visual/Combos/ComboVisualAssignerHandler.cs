using Battle.Combo;
using System.Collections.Generic;
using Account.GeneralData;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class ComboVisualAssignerHandler : BaseVisualAssignerHandler<ComboCore>
    {
        [Header("Visuals")]
        [SerializeField] ComboTypeVisualAssigner _comboTypeVisualAssigner;
        [SerializeField] ComboSequenceVisualAssigner _comboSequenceVisualAssigner;
        [SerializeField] ComboTitleAndArrowVisualAssigner _comboTitleAndArrowVisualAssigner;
        public override IEnumerable<BaseVisualAssigner<ComboCore>> VisualAssigners
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