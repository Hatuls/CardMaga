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
            _playSound?.Raise(SoundsNameEnum.Reject);
        }
    }


    public static void FinishTurn()
    {
        _instance._playSound?.Raise(SoundsNameEnum.EndTurn);
        _OnFinishTurnPress?.Invoke();
    }

}