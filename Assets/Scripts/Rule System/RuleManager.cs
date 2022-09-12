using System.Collections.Generic;
using System;
using Battle;

namespace CardMaga.Rules
{
    public class RuleManager
    {
        public static event Action<bool> OnEndGameRule;

        private List<BaseRule> _baseRules;
        private List<BaseRule> _endGameRules;
        private List<BaseRule> _activeRules;

        public void InitRuleList(BaseRuleListenerFactorySO[] rules,BaseRuleListenerFactorySO[] endGameRules, BattleManager battleManager)
        {
            for (int i = 0; i < rules.Length; i++)
            {
                BaseRule temp = rules[i].CreateRule(battleManager);
                _baseRules.Add(temp);
            }

            for (int i = 0; i < endGameRules.Length; i++)
            {
                BaseRule temp = endGameRules[i].CreateRule(battleManager);
                _endGameRules.Add(temp);
            }
        }

        private void DisposRules()
        {
            for (int i = 0; i < _baseRules.Count; i++)
            {
                _baseRules[i].Dispose();
            }  
        }

        private void GameEnd(bool isLeft)
        {
        }
    }

}
