using System;
using Battle;
using Battle.Data;
using CardMaga.Rules;


public class EndBattleHandler : IDisposable
{
    public event Action<bool> OnBattleEnded;
    public event Action OnBattleAnimatonEnd;

    private readonly IPlayersManager _playersManager;
    private readonly BattleData _battleData;
    private readonly CardExecutionManager _cardExecutionManager;

    private bool _isGameEnded = false;
    private bool _isInTutorial = false;

    public bool IsGameEnded
    {
        get => _isGameEnded;
    }
    
    public EndBattleHandler(IBattleManager battleManager)
    {
        _playersManager = battleManager.PlayersManager;
        _cardExecutionManager = battleManager.CardExecutionManager;
        RuleManager.OnGameEnded += EndBattle;
        AnimatorController.OnDeathAnimationFinished += DeathAnimationFinished;
        
        _isGameEnded = false;
    }

    private void EndBattle(bool isLeftPlayerWon)
    {
        if (_isGameEnded)
            return;
        
        _cardExecutionManager.ResetExecution();
        
        if (isLeftPlayerWon)
        {
            LeftPlayerWon();
        }
        else
        {
            RightPlayerWon();
        }

        _playersManager.LeftCharacter.VisualCharacter.AnimatorController.ResetLayerWeight();
        _playersManager.RightCharacter.VisualCharacter.AnimatorController.ResetLayerWeight();

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
        _playersManager.LeftCharacter.VisualCharacter.AnimatorController.CharacterWon();
        _playersManager.LeftCharacter.CharacterSO.VictorySound.PlaySound();
        _playersManager.RightCharacter.VisualCharacter.AnimatorController.CharacterIsDead();
    }

    private void RightPlayerWon()
    {
        _playersManager.RightCharacter.VisualCharacter.AnimatorController.CharacterWon();
        _playersManager.RightCharacter.CharacterSO.VictorySound.PlaySound();
        _playersManager.LeftCharacter.VisualCharacter.AnimatorController.CharacterIsDead();
    }

    public void Dispose()
    {
        RuleManager.OnGameEnded -= EndBattle;
        AnimatorController.OnDeathAnimationFinished -= DeathAnimationFinished;
    }
}
