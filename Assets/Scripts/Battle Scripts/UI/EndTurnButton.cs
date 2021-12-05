using UnityEngine;

public class EndTurnButton : ButtonUI
{
    public static System.Action _OnFinishTurnPress;

    private static EndTurnButton _instance;
    [SerializeField]
    SoundEventSO OnRejectSound;

    private void Awake()
    {
        _instance = this;
    }
    public override void ButtonPressed()
    {
        if (Battles.Turns.TurnHandler.CurrentState == Battles.Turns.TurnState.PlayerTurn)
        {
            FinishTurn();
        }
        else
        {
            OnRejectSound?.PlaySound();
           // _playSound?.Raise("Reject");
        }
    }


    public static void FinishTurn()
    {
        if (Battles.Turns.TurnHandler.CurrentState == Battles.Turns.TurnState.PlayerTurn && Battles.BattleManager.isGameEnded == false)
        {

            _OnFinishTurnPress?.Invoke();
        }
    }
}