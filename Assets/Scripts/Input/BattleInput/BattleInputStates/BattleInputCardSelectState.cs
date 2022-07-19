public class BattleInputCardSelectState : BaseState
{
    public override void OnEnterState()
    {
        base.OnEnterState();
    }

    public override void OnExitState()
    {
    }

    public override StateIdentificationSO OnHoldState()
    {
        return CheckStateCondition();
    }
}