using CardMaga.UI;

public class MoveFromDefultStateToSelectState : BaseCondition
{
    public override bool CheckCondition()
    {
        return _moveCondition;
    }
    
    public override void InitCondition()
    {
        HandUI.OnInputCardSelect += ChangeState;
    }

    private void ChangeState()
    {
        HandUI.OnInputCardSelect -= ChangeState;
        _moveCondition = true;
    }
}