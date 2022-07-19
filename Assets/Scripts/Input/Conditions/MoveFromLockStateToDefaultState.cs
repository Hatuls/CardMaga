using CardMaga.UI;

namespace CardMaga.Input
{

    public class MoveFromLockStateToDefaultState : BaseCondition
{
    private bool _moveCondition;

    public override bool CheckCondition()
    {
        return _moveCondition;
    }

    public override void InitCondition()
    {
        _moveCondition = false;
        HandUI.OnCardDrawnAndAlign += ChangeState;
    }

    private void ChangeState()
    {
        HandUI.OnCardDrawnAndAlign -= ChangeState;
        _moveCondition = true;
    }
}
}