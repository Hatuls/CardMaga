using UnityEngine;

public class BattleInputDefaultState : BaseState
{
    public override void OnEnterState()
    {
        base.OnEnterState();
        Debug.Log("Enter Default State");
    }
    
    public override StateIdentificationSO OnHoldState()
    {
        return base.CheckStateCondition();
    }
}
