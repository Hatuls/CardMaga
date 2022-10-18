using Battle;
using UnityEngine;
using UnityEngine.Serialization;

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
        public abstract BaseRuleLogicFactorySO<T>[] BaseRuleLogicFactorySo { get; }

        protected abstract BaseRule<T> CreateRuleListener(IBattleManager battleManager);

        public BaseRule<T> CreateRule(IBattleManager battleManager)
        {
            BaseRuleLogic<T>[] ruleLogics = new BaseRuleLogic<T>[BaseRuleLogicFactorySo.Length];
            BaseRule<T> baseRule = CreateRuleListener(battleManager);

            for (int i = 0; i < BaseRuleLogicFactorySo.Length; i++)
            {
                ruleLogics[i] = BaseRuleLogicFactorySo[i].CreateRuleLogic(battleManager);
                ruleLogics[i].InitRuleLogic(battleManager);
            }
            
            baseRule.InitRuleListener(battleManager,ruleLogics);
            
            return baseRule;
        }
    }

    public abstract class BaseEndGameRuleFactorySO : ScriptableObject
    {
        [SerializeField] protected float _delayToEndGame;

        public float DelayToEndGame
        {
            get => _delayToEndGame;
        }
        
        protected abstract BaseEndGameRule CreateRuleListener(IBattleManager battleManager);

        public BaseEndGameRule CreateRule(IBattleManager battleManager)
        {
            BaseEndGameRule baseRule = CreateRuleListener(battleManager);
            
            baseRule.InitRuleListener(battleManager, null);

            return baseRule;
        }
    }

}