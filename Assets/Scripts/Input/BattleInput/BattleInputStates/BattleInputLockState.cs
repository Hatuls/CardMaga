using System.Collections;
using System.Collections.Generic;
using Battles.UI;
using UnityEngine;

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
