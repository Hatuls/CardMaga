using Battle;
using UnityEngine;

namespace CardMaga.Rules
{
    public abstract class BaseRuleLogicFactorySO : ScriptableObject
    {
        public abstract BaseRuleLogic CreateRuleLogic(IBattleManager battleManager);
    }
    
    public abstract class BaseRuleLogicFactorySO<T> : ScriptableObject
    {
        public abstract BaseRuleLogic<T> CreateRuleLogic(IBattleManager battleManager);
    }
}