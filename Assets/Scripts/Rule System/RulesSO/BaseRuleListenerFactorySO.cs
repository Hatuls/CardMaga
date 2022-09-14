using Battle;
using UnityEngine;

namespace CardMaga.Rules
{
    public abstract class BaseRuleListenerFactorySO : ScriptableObject
    {
        [SerializeField] private BaseRuleLogicFactorySO _logicFactorySo;

        protected abstract BaseRuleListener CreateRuleListener(IBattleManager battleManager);

        public Rule CreateRule(IBattleManager battleManager)
        {
            return new Rule(_logicFactorySo.CreateRuleLogic(battleManager), CreateRuleListener(battleManager),
                battleManager);
        }
    }
    
    public abstract class BaseRuleListenerFactorySO<T> : ScriptableObject
    {
        [SerializeField] private BaseRuleLogicFactorySO<T> _logicFactorySo;

        protected abstract BaseRuleListener<T> CreateRuleListener(IBattleManager battleManager);

        public Rule<T> CreateRule(IBattleManager battleManager)
        {
            return new Rule<T>(_logicFactorySo.CreateRuleLogic(battleManager), CreateRuleListener(battleManager),
                battleManager);
        }
    }
}