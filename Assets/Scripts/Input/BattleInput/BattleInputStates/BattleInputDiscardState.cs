using UnityEngine;

public class BattleInputDiscardState : BaseState
{
    public override void OnEnterState()
    {
        base.OnEnterState();
        Debug.Log("Enter Discard State");
    }

    public override StateIdentificationSO OnHoldState()
    {
        return base.CheckStateCondition();
    }
}