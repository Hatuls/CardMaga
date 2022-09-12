using System.Collections;
using System.Collections.Generic;
using Battle;
using UnityEngine;

namespace CardMaga.Rules
{
    public abstract class BaseRuleLogicFactorySO : ScriptableObject
    {
        public abstract BaseRuleLogic CreateRuleLogic(BattleManager battleManager);
    }

}
