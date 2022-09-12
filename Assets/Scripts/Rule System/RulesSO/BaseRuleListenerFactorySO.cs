using Battle;
using UnityEngine;

namespace CardMaga.Rules
{
    public abstract class BaseRuleListenerFactorySO : ScriptableObject
    {
        [SerializeField] private BaseRuleLogicFactorySO _logicFactorySo;

        public abstract BaseRuleListener RuleListener(BattleManager battleManager);
    
        public BaseRule CreateRule(BattleManager battleManager)
        {
            return new BaseRule(_logicFactorySo.CreateRuleLogic(battleManager), RuleListener(battleManager));
        }
    }    
}

