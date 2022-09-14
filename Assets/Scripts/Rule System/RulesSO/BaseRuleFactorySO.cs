using Battle;
using UnityEngine;

namespace CardMaga.Rules
{
    public abstract class BaseRuleFactorySO : ScriptableObject
    {
        [SerializeField] private BaseRuleLogicFactorySO[] _logicFactorySo;

        protected abstract BaseRule CreateRuleListener(IBattleManager battleManager);

        public BaseRule CreateRule(IBattleManager battleManager)
        {
            BaseRuleLogic[] ruleLogics = new BaseRuleLogic[_logicFactorySo.Length];
            BaseRule baseRule = CreateRuleListener(battleManager);

            for (int i = 0; i < _logicFactorySo.Length; i++)
            {
                ruleLogics[i] = _logicFactorySo[i].CreateRuleLogic(battleManager);
                ruleLogics[i].InitRuleLogic(battleManager);
            }
            
            baseRule.InitRuleListener(battleManager,ruleLogics);
            
            return baseRule;
        }
    }
    
    public abstract class BaseRuleFactorySO<T> : ScriptableObject
    {
        [SerializeField] private BaseRuleLogicFactorySO<T>[] _logicFactorySo;

        protected abstract BaseRule<T> CreateRuleListener(IBattleManager battleManager);

        public BaseRule<T> CreateRule(IBattleManager battleManager)
        {
            BaseRuleLogic<T>[] ruleLogics = new BaseRuleLogic<T>[_logicFactorySo.Length];
            BaseRule<T> baseRule = CreateRuleListener(battleManager);

            for (int i = 0; i < _logicFactorySo.Length; i++)
            {
                ruleLogics[i] = _logicFactorySo[i].CreateRuleLogic(battleManager);
                ruleLogics[i].InitRuleLogic(battleManager);
            }
            
            baseRule.InitRuleListener(battleManager,ruleLogics);
            
            return baseRule;
        }
    }

}