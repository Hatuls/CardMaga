using CardMaga.UI;

public class MoveFromDefultStateToSelectState : BaseCondition
{
    private bool _moveCondition;

    public override bool CheckCondition()
    {
        return _moveCondition;
    }

    public override void InitCondition()
    {
        _moveCondition = false;
        HandUI.OnCardSelect += ChangeState;
    }

    private void ChangeState()
    {
        HandUI.OnCardSelect -= ChangeState;
        _moveCondition = true;
    }
}