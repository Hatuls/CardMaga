public class EndTurnButton : ButtonUI
{
    public static System.Action _OnFinishTurnPress;

    private static EndTurnButton _instance;

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
            _playSound?.Raise("Reject");
        }
    }


    public static void FinishTurn()
    {
        if (Battles.Turns.TurnHandler.CurrentState == Battles.Turns.TurnState.PlayerTurn && Battles.BattleManager.isGameEnded == false)
        {

            _instance._playSound?.Raise("EndTurn");
            _OnFinishTurnPress?.Invoke();
        }
    }
}