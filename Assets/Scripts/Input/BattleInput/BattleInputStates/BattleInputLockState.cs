public class BattleInputLockState : BaseState
{
    public override StateIdentificationSO OnHoldState()
    {
        return CheckStateCondition();
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
    }
}