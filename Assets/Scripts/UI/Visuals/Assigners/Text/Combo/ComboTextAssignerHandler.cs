using System.Collections.Generic;
using Account.GeneralData;
using UnityEngine;

namespace CardMaga.UI.Text
{
    [System.Serializable]
    public class ComboTextAssignerHandler : BaseTextAssignerHandler<ComboCore>
    {
        [Header("Texts")]
        [SerializeField] ComboNameTextAssigner _comboNameTextAssigner;
        [SerializeField] ComboTypeTextAssigner _comboTypeTextAssigner;
        public override IEnumerable<BaseTextAssigner<ComboCore>> TextAssigners
        {
            get
            {
                yield return _comboNameTextAssigner;
                yield return _comboTypeTextAssigner;
            }
        }
    }
}
