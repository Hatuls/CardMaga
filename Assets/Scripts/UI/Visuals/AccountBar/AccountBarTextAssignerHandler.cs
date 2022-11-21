using CardMaga.UI.Visuals;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.UI.Text
{
    [Serializable]
    public class AccountBarTextAssignerHandler : BaseTextAssignerHandler<AccountBarVisualData>
    {
        [SerializeField] AccountNameTextAssigner _accountNameTextAssigner;
        [SerializeField] EXPBarTextAssigner _expBarTextAssigner;
        public override IEnumerable<BaseTextAssigner<AccountBarVisualData>> TextAssigners
        {
            get
            {
                yield return _accountNameTextAssigner;
                yield return _expBarTextAssigner;
            }
        }

    }
}