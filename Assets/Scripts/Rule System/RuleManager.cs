using System;
using System.Collections.Generic;
using Battle;

namespace CardMaga.Rules
{
    public class RuleManager
    {
        private List<Rule> _activeRules;

        private List<Rule> _baseRules;
        private List<Rule> _endGameRules;
        public static event Action<bool> OnEndGameRule;

        public void InitRuleList(BaseRuleListenerFactorySO[] rules, BaseRuleListenerFactorySO[] endGameRules,
            IBattleManager battleManager)
        {
            for (var i = 0; i < rules.Length; i++)
            {
                var temp = rules[i].CreateRule(battleManager);
                _baseRules.Add(temp);
            }

            for (var i = 0; i < endGameRules.Length; i++)
            {
                var temp = endGameRules[i].CreateRule(battleManager);
                _endGameRules.Add(temp);
            }
        }

        private void DisposRules()
        {
            for (var i = 0; i < _baseRules.Count; i++) _baseRules[i].Dispose();
        }

        private void GameEnd(bool isLeft)
        {
        }
    }
}