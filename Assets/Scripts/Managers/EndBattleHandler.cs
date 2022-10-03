using System;
using Battle;
using Battle.Data;
using CardMaga.Rules;

public class EndBattleHandler
{
    public event Action<bool> OnBattleEnded;

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
        
        _isGameEnded = false;
    }

    public void DeConstrctor()//need work
    {
        RuleManager.OnGameEnded -= EndBattle;
    }

    public void EndBattle(bool isLeftPlayerWon)
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
}
