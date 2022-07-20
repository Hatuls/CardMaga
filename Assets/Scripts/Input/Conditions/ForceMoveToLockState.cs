using Battles.UI;
using CardMaga.UI;

public class ForceMoveToLockState : BaseCondition
{
    public override bool CheckCondition()
    {
        return _moveCondition;
    }

    public override void InitCondition()
    {
        HandUI.OnDiscardAllCards += ChangeState;
    }

    private void ChangeState()
    {
        HandUI.OnDiscardAllCards -= ChangeState;
        _moveCondition = true;
    }
}