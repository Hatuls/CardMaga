using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;
using Account.GeneralData;
using CardMaga.Rules;

namespace CardMaga.BattleConfigSO
{
    [CreateAssetMenu(fileName = "New Battle Config SO", menuName = "ScriptableObjects/Tutorial/Battle/Battle Config SO")]
    public class BattleConfigSO : ScriptableObject
    {
        [SerializeField] private int _maxCardsInHand;
        [SerializeField] private bool _isRandom;
        [SerializeField] private bool _isOverideCharacter;
        [SerializeField] private Character _player;
        [SerializeField] private Character _enemy;
        [SerializeField] private int _turnCountdown;
        [SerializeField] private bool _timerActive;
        [SerializeField] private bool _isPlayerStart;
        [SerializeField] private BaseRuleFactorySO[] _gameRules;
        [SerializeField] private BaseBoolRuleFactorySO[] _endGameRules; 
        [SerializeField] private BattleTutorial _battleTutorial;

        public int MaxCardsInHand => _maxCardsInHand;
        public bool IsRandom => _isRandom;
        public bool IsOverideCharacter => _isOverideCharacter;
        public Character Player => _player;
        public Character Enemy => _enemy;
        public int TurnCountdown => _turnCountdown;
        public bool TimerActive => _timerActive;
        public bool IsPlayerStart => _isPlayerStart;
        public BaseRuleFactorySO[] GameRule => _gameRules;
        public BaseRuleFactorySO<bool>[] EndGameRule => _endGameRules;
        public BattleTutorial BattleTutorial => _battleTutorial;
    }
}


