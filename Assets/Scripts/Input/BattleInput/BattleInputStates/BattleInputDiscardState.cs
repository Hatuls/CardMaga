using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInputDiscardState : BaseState
{
    public override void OnEnterState()
    {
        Debug.Log("Enter Discard State");
    }

    public override StateIdentificationSO OnHoldState()
    {
        return StateID;
    }
}
