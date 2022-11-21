using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class AccountBarVisualAssignerHandler : BaseVisualAssignerHandler<AccountBarVisualData>
    {
        [SerializeField] AccountImageVisualAssigner _accountImageVisualAssigner;
        [SerializeField] EXPBarVisualAssigner _expBarVisualAssigner;
        public override IEnumerable<BaseVisualAssigner<AccountBarVisualData>> VisualAssigners
        {
            get
            {
                yield return _accountImageVisualAssigner;
                yield return _expBarVisualAssigner;
            }
        }
    }
}