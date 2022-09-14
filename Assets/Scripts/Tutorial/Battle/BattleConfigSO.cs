using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;
using Account.GeneralData;

namespace CardMaga.BattleConfigSO
{
    [CreateAssetMenu(fileName = "New Battle Config SO", menuName = "ScriptableObjects/Tutorial/Battle/Battle Config SO")]
    public class BattleConfigSO : ScriptableObject
    {
        private int _maxCardsInHand;
        private bool _isRandom;
        private bool _isOverideCharacter;
        private Character _player;
        private Character _enemy;
        private int _turnCountdown;
        private bool _timerActive;
        private bool _isPlayerStart;
        [SerializeField] private BattleTutorial _battleTutorial;

        public int MaxCardsInHand => _maxCardsInHand;
        public bool IsRandom => _isRandom;
        public bool IsOverideCharacter => _isOverideCharacter;
        public Character Player => _player;
        public Character Enemy => _enemy;
        public int TurnCountdown => _turnCountdown;
        public bool TimerActive => _timerActive;
        public bool IsPlayerStart => _isPlayerStart;
        public BattleTutorial BattleTutorial => _battleTutorial;
    }
}


