public class EndTurnButton : ButtonUI
{

    public override void ButtonPressed()
    {
        if (Battles.Turns.TurnHandler.CurrentState == Battles.Turns.TurnState.PlayerTurn)
        {
            _playSound?.Raise(SoundsNameEnum.EndTurn);
            Battles.Turns.TurnHandler.FinishTurn = true;

        }
        else
        {
            _playSound?.Raise(SoundsNameEnum.TapCard);
        }
    }
}