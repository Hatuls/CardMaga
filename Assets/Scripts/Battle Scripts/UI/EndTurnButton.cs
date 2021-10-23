public class EndTurnButton : ButtonUI
{
    public static System.Action _OnFinishTurnPress;
    public override void ButtonPressed()
    {
        if (Battles.Turns.TurnHandler.CurrentState == Battles.Turns.TurnState.PlayerTurn)
        {
            _playSound?.Raise(SoundsNameEnum.EndTurn);
            _OnFinishTurnPress?.Invoke();
        }
        else
        {
            _playSound?.Raise(SoundsNameEnum.TapCard);
        }
    }
}