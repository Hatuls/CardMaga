using System;
using System.Collections.Generic;
using Battle;
using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;
using UnityEngine;

namespace CardMaga.Rules
{
    public class RuleManager : ISequenceOperation<IBattleManager>
    {
        public event Action<bool> OnGameEnded; 

        private List<BaseRule> _activeRules;

        private List<BaseRule> _baseRules;
        private List<BaseRule<bool>> _endGameRules;
        
        public int Priority
        {
            get { return 0; }
        }

        public RuleManager()
        {
            BattleManager.Register(this,OrderType.After);
        }
        
        public void DisposeRules()
        {
            for(var j = 0; j < _endGameRules.Count; j++) _endGameRules[j].OnActive -= GameEnd;
            
            for (var i = 0; i < _baseRules.Count; i++) _baseRules[i].Dispose();
        }

        private void GameEnd(bool isLeft)
        {
            Debug.Log("GameEnded");
            OnGameEnded?.Invoke(isLeft);
        }

        public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager data)
        {
            BaseRuleFactorySO[] rules = data.BattleData.BattleConfigSO.GameRule;
            BaseRuleFactorySO<bool>[] endGameRules = data.BattleData.BattleConfigSO.EndGameRule;
            
            _baseRules = new List<BaseRule>();
            _endGameRules = new List<BaseRule<bool>>();
            
            if (rules.Length > 0)
            {
                for (var i = 0; i < rules.Length; i++)
                {
                    var temp = rules[i].CreateRule(data);
                    _baseRules.Add(temp);
                }    
            }

            if (endGameRules.Length > 0)
            {
                for (var i = 0; i < endGameRules.Length; i++)
                {
                    var temp = endGameRules[i].CreateRule(data);
                    temp.OnActive += GameEnd;
                    _endGameRules.Add(temp);
                }    
            }
        }
    }
}