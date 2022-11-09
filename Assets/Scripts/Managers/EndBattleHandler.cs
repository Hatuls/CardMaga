using System;
using Battle;
using Battle.Data;
using CardMaga.Battle.Execution;
using CardMaga.Battle.Visual;
using CardMaga.Rules;
using UnityEngine;
using UnityEngine.Events;

namespace CardMaga.Battle
{


[Serializable]
public class EndBattleHandler : IDisposable
{
    public event Action<bool> OnBattleEnded;
    public event Action OnBattleAnimatonEnd;
    public event Action OnLeftPlayerWon;
    public event Action OnRightPlayerWon;
    public event Action OnBattleFinished;



    
    [SerializeField, EventsGroup]
    private UnityEvent OnPlayerDefeat;
    [SerializeField, EventsGroup]
    private UnityEvent OnPlayerVictory;

    private readonly IPlayersManager _playersManager;
    private readonly BattleData _battleData;
    private readonly IDisposable _gameCommands;

    private bool _isGameEnded = false;
    private bool _isLeftPlayerWon;

    public bool IsGameEnded
    {
        get => _isGameEnded;
    }
    
    public EndBattleHandler(IBattleManager battleManager)
    {
        _playersManager = battleManager.PlayersManager;
            _gameCommands = battleManager.GameCommands;
        RuleManager.OnGameEnded += EndBattle;
        AnimatorController.OnDeathAnimationFinished += DeathAnimationFinished;
        
        _isGameEnded = false;
    }
    public void ForceEndBattle(bool isLeftWon) => EndBattle(isLeftWon);
    private void EndBattle(bool isLeftPlayerWon)
    {
        if (_isGameEnded)
            return;

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

            OnBattleFinished?.Invoke();
        BattleData.Instance.PlayerWon = isLeftPlayerWon;
        
        _isGameEnded = true;
        
        OnBattleEnded?.Invoke(isLeftPlayerWon);

    }
        
    private void DeathAnimationFinished(bool isPlayer)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Scene Parameter", 0);

        OnBattleAnimatonEnd?.Invoke();
    }
    
    private void LeftPlayerWon()
    {
            OnLeftPlayerWon?.Invoke();
        _playersManager.LeftCharacter.CharacterSO.VictorySound.PlaySound();
    }

    private void RightPlayerWon()
    {
            OnRightPlayerWon?.Invoke();
        _playersManager.RightCharacter.CharacterSO.VictorySound.PlaySound();
    }

    public void Dispose()
    {
        RuleManager.OnGameEnded -= EndBattle;
        AnimatorController.OnDeathAnimationFinished -= DeathAnimationFinished;
    }
}
}