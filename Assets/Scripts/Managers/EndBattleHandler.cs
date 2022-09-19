using System;
using Battle;
using Battle.Data;

public class EndBattleHandler
{
    public static event Action<bool> OnBattleEnded;

    private readonly IPlayersManager _playersManager;
    private readonly BattleData _battleData;
    private readonly CardExecutionManager _cardExecutionManager;

    private bool _isGameEnded = false;

    public bool IsGameEnded
    {
        get => _isGameEnded;
    }
    
    public EndBattleHandler(IBattleManager battleManager)
    {
        _playersManager = battleManager.PlayersManager;
        _battleData = battleManager.BattleData;
        _cardExecutionManager = battleManager.CardExecutionManager;

        _isGameEnded = false;
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

        _battleData.PlayerWon = isLeftPlayerWon;
        
        _isGameEnded = true;
        
        OnBattleEnded?.Invoke(isLeftPlayerWon);
    }

    private void LeftPlayerWon()
    {
        _playersManager.LeftCharacter.VisualCharacter.AnimatorController.CharacterWon();
        _playersManager.LeftCharacter.Character.CharacterData.CharacterSO.VictorySound.PlaySound();
        _playersManager.RightCharacter.VisualCharacter.AnimatorController.CharacterIsDead();
    }

    private void RightPlayerWon()
    {
        _playersManager.RightCharacter.VisualCharacter.AnimatorController.CharacterWon();
        _playersManager.RightCharacter.Character.CharacterData.CharacterSO.VictorySound.PlaySound();
        _playersManager.LeftCharacter.VisualCharacter.AnimatorController.CharacterIsDead();
    }
}
