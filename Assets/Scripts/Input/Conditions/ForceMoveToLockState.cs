using Battles.UI;
using CardMaga.UI;

public class ForceMoveToLockState : BaseCondition
{
    private bool _moveCondition;

    public override bool CheckCondition()
    {
        return _moveCondition;
    }

    public override void InitCondition()
    {
        _moveCondition = false;
        HandUI.OnDiscardAllCards += ChangeState;
    }

    private void ChangeState()
    {
        HandUI.OnDiscardAllCards -= ChangeState;
        _moveCondition = true;
    }
}