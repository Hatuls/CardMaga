using Battle.Characters;
using UnityEngine;
using CardMaga.Rules;
using CardMaga.Rewards;

namespace CardMaga.BattleConfigSO
{
    [CreateAssetMenu(fileName = "New Battle Config SO", menuName = "ScriptableObjects/Battle Config/New Battle Config SO")]
    public class BattleConfigSO : ScriptableObject
    {
        [Header("Deck configuration:")]
        [SerializeField,Tooltip("Determines whether drawing cards from the deck will be random")] 
        private bool _isShuffleCards;//done
        [Header("Character configuration:")]
        [SerializeField,Tooltip("Determines which character will start the battle")] 
        private CharacterSelecter _characterSelecter;//done
        [Header("Timer configuration:")]
        [SerializeField,Tooltip("Set if there is a timer in the battle")] 
        private bool _isTimerActive;//done
        [SerializeField,Tooltip("Set the duration of the timer for each turn")] 
        private int _timerCountdown;//done
        [Header("Game rules:")]
        [SerializeField,Tooltip("A list of the rules of the game")] 
        private BaseRuleFactorySO[] _gameRules;//done
        [SerializeField,Tooltip("A list of endgame rules, these rules once their conditions are met they will end the game")] 
        private BaseEndGameRuleFactorySO[] _endGameRules;//done 
        [SerializeField, Tooltip("A reward received when the player win")]
        private BaseRewardFactorySO _winReward;
        [SerializeField, Tooltip("A reward received when the player loses")]
        private BaseRewardFactorySO _loseReward;

        [Header("Tutorial:")]
        [SerializeField,Tooltip("Tutorial configuration:")] private BattleTutorial _battleTutorial;//done
        
        public bool IsShufflingCards => _isShuffleCards;
        public int TimerCountdown => _timerCountdown;
        public bool TimerActive => _isTimerActive;
        public CharacterSelecter CharacterSelecter => _characterSelecter;
        public BaseRuleFactorySO[] GameRule => _gameRules;
        public BaseEndGameRuleFactorySO[] EndGameRule => _endGameRules;
        public BattleTutorial BattleTutorial => _battleTutorial;
        public BaseRewardFactorySO WinReward => _winReward;
        public BaseRewardFactorySO LoseReward => _loseReward;
    }
}


