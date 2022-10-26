using Battle;
using UnityEngine;

namespace CardMaga.Rules
{
    public abstract class BaseRuleLogicFactorySO : ScriptableObject
    {
        public abstract BaseRuleLogic CreateRuleLogic(IBattleManager iBattleManager);
    }
    
    public abstract class BaseRuleLogicFactorySO<T> : ScriptableObject
    {
        public abstract BaseRuleLogic<T> CreateRuleLogic(IBattleManager iBattleManager);
    }

    public abstract class BaseBoolRuleLogicFactorySO : BaseRuleLogicFactorySO<bool>
    {
        
    }
}