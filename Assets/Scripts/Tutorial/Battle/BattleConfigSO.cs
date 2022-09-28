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
        [SerializeField] private bool _timerActive;
        [SerializeField] private int _timerCountdown;
        [SerializeField] private CharacterSelecter _characterSelecter;
        [SerializeField] private BaseRuleFactorySO[] _gameRules;
        [SerializeField] private BaseEndGameRuleFactorySO[] _endGameRules; 
        [SerializeField] private BattleTutorial _battleTutorial;

        public int MaxCardsInHand => _maxCardsInHand;
        public bool IsRandom => _isRandom;
        public bool IsOverideCharacter => _isOverideCharacter;
        public Character Player => _player;
        public Character Enemy => _enemy;
        public int TimerCountdown => _timerCountdown;
        public bool TimerActive => _timerActive;
        public CharacterSelecter CharacterSelecter => _characterSelecter;
        public BaseRuleFactorySO[] GameRule => _gameRules;
        public BaseEndGameRuleFactorySO[] EndGameRule => _endGameRules;
        public BattleTutorial BattleTutorial => _battleTutorial;
    }
}


