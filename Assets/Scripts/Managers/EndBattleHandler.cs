using Battle.Data;
using CardMaga.Battle.Visual;
using CardMaga.Rules;
using ReiTools.TokenMachine;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace CardMaga.Battle
{
    [Serializable]
    public class EndBattleHandler : IDisposable
    {
        public event Action<ITokenReciever> OnBattleFinished;
        public event Action<bool> OnBattleEnded;
        public event Action OnBattleEndedVoid;
        public event Action OnCharacterAnimatonEnd;
        public event Action OnLeftPlayerWon;
        public event Action OnRightPlayerWon;
        public event Action OnAnimationsEnded;
        
        [SerializeField, EventsGroup]
        private UnityEvent OnPlayerDefeat;
        [SerializeField, EventsGroup]
        private UnityEvent OnPlayerVictory;

        private readonly IPlayersManager _playersManager;
        private readonly BattleData _battleData;
        private readonly IDisposable _gameCommands;
        private IDisposable _endGameToken;
        private TokenMachine _endGameTokenMachine;
        private bool _isGameEnded = false;
        private bool _isLeftPlayerWon;

        public bool IsGameEnded
        {
            get => _isGameEnded;
        }
        public bool IsLeftPlayerWon => _isLeftPlayerWon;
        public EndBattleHandler(IBattleManager battleManager)
        {
            _playersManager = battleManager.PlayersManager;
            _gameCommands = battleManager.GameCommands;
            RuleManager.OnGameEnded += EndBattle;
            AnimatorController.OnDeathAnimationFinished += DeathAnimationFinished;
            _endGameTokenMachine = new TokenMachine(OnAnimationsEnded);
            _isGameEnded = false;
        }
        public void ForceEndBattle(bool isLeftWon) => EndBattle(isLeftWon);
        private void EndBattle(bool isLeftPlayerWon)
        {
            if (_isGameEnded)
                return;

            _isGameEnded = true;
            _isLeftPlayerWon = isLeftPlayerWon;
            _gameCommands.Dispose();

            if (isLeftPlayerWon)
            {
                LeftPlayerWon();
            }
            else
            {
                RightPlayerWon();
            }

            EndGame();

        }

        private void EndGame()
        {
            _endGameToken = _endGameTokenMachine.GetToken();
            OnBattleFinished?.Invoke(_endGameTokenMachine);
            BattleData.Instance.IsPlayerWon = _isLeftPlayerWon;
            OnBattleEnded?.Invoke(_isLeftPlayerWon);
            OnBattleEndedVoid?.Invoke();
        }

        private void DeathAnimationFinished(bool isPlayer)
        {
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Scene Parameter", 0);
            _endGameToken?.Dispose();
            OnCharacterAnimatonEnd?.Invoke();
        }

        private void LeftPlayerWon()
        {
            OnLeftPlayerWon?.Invoke();

            if (_playersManager.LeftCharacter.CharacterSO.VictorySound != null)
                _playersManager.LeftCharacter.CharacterSO.VictorySound.PlaySound();
        }

        private void RightPlayerWon()
        {
            OnRightPlayerWon?.Invoke();

            if (_playersManager.RightCharacter.CharacterSO.VictorySound != null)
                _playersManager.RightCharacter.CharacterSO.VictorySound.PlaySound();
        }

        public void Dispose()
        {
            RuleManager.OnGameEnded -= EndBattle;
            AnimatorController.OnDeathAnimationFinished -= DeathAnimationFinished;
        }
    }
}