using CardMaga.UI;

namespace CardMaga.Input
{ 
    public class MoveFromLockStateToDefaultState : BaseCondition
{

    public override bool CheckCondition()
    {
        return _moveCondition;
    }

    public override void InitCondition()
    {
        HandUI.OnCardDrawnAndAlign += ChangeState;
    }

    private void ChangeState()
    {
        HandUI.OnCardDrawnAndAlign -= ChangeState;
        _moveCondition = true;
    }
}
}