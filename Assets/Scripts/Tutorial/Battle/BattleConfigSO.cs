using Battle.Characters;
using UnityEngine;
using CardMaga.Rules;

namespace CardMaga.BattleConfigSO
{
    [CreateAssetMenu(fileName = "New Battle Config SO", menuName = "ScriptableObjects/Battle/New Battle Config SO")]
    public class BattleConfigSO : ScriptableObject
    {
        [Header("Deck configuration:")]
        [SerializeField,Tooltip("Determines whether drawing cards from the deck will be random")] 
        private bool _isShuffleCard;
        [Header("Character configuration:")]
        [SerializeField,Tooltip("Determines which character will start the battle")] 
        private CharacterSelecter _characterSelecter;//done
        [Header("Timer configuration:")]
        [SerializeField,Tooltip("Set if there is a timer in the battle")] private bool _timerActive;//done
        [SerializeField,Tooltip("Set the duration of the timer for each turn")] private int _timerCountdown;//done
        [Header("Game rules:")]
        [SerializeField,Tooltip("A list of the rules of the game")] private BaseRuleFactorySO[] _gameRules;//done
        [SerializeField,Tooltip("A list of endgame rules, these rules once their conditions are met they will end the game")] 
        private BaseEndGameRuleFactorySO[] _endGameRules;//done 
        
        public bool IsShuffleCard => _isShuffleCard;
        public int TimerCountdown => _timerCountdown;
        public bool TimerActive => _timerActive;
        public CharacterSelecter CharacterSelecter => _characterSelecter;
        public BaseRuleFactorySO[] GameRule => _gameRules;
        public BaseEndGameRuleFactorySO[] EndGameRule => _endGameRules;
       
    }
}


