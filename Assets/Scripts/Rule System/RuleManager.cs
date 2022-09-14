using System;
using System.Collections.Generic;
using Battle;
using ReiTools.TokenMachine;

namespace CardMaga.Rules
{
    public class RuleManager
    {
        public event Action<bool> OnGameEnded; 

        private List<BaseRule> _activeRules;

        private List<BaseRule> _baseRules;
        private List<BaseRule<bool>> _endGameRules;
        
        public void InitRuleList(BaseRuleFactorySO[] rules, BaseRuleFactorySO<bool>[] endGameRules,
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
                temp.OnActive += GameEnd;
                _endGameRules.Add(temp);
            }
        }

        private void DisposeRules()
        {
            for(var j = 0; j < _endGameRules.Count; j++) _endGameRules[j].OnActive -= GameEnd;
            
            for (var i = 0; i < _baseRules.Count; i++) _baseRules[i].Dispose();
        }

        private void GameEnd(bool isLeft)
        {
            OnGameEnded?.Invoke(isLeft);
        }
    }
}