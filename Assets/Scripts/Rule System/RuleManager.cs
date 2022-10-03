using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;
using UnityEngine;

namespace CardMaga.Rules
{
    public class RuleManager : ISequenceOperation<IBattleManager>
    {
        public static event Action<bool> OnGameEnded;

        private MonoBehaviour _monoBehaviour;
        private List<BaseRule> _activeRules;
        
        private List<BaseRule> _baseRules;
        private List<BaseEndGameRule> _endGameRules;
        
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
            for(var j = 0; j < _endGameRules.Count; j++) _endGameRules[j].OnEndGameRuleActive -= GameEnd;
            
            for (var i = 0; i < _baseRules.Count; i++) _baseRules[i].Dispose();
        }

        private void GameEnd(float delay,bool isLeft)
        {
            _monoBehaviour.StartCoroutine(EndGameCountDown(delay, isLeft));
        }

        private IEnumerator EndGameCountDown(float delay,bool isLeft)
        {
            WaitForSeconds waitForSeconds =  new WaitForSeconds(delay);
            yield return waitForSeconds;
            OnGameEnded?.Invoke(isLeft);
        }

        public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager data)
        {
            _monoBehaviour = data.MonoBehaviour;
            
            BaseRuleFactorySO[] rules = data.BattleData.BattleConfigSO.GameRule;
            BaseEndGameRuleFactorySO[] endGameRules = data.BattleData.BattleConfigSO.EndGameRule;
            
            _baseRules = new List<BaseRule>();
            _endGameRules = new List<BaseEndGameRule>();
            
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
                    BaseEndGameRule temp = endGameRules[i].CreateRule(data);
                    temp.OnEndGameRuleActive += GameEnd;
                    _endGameRules.Add(temp);
                }    
            }
        }
    }
}