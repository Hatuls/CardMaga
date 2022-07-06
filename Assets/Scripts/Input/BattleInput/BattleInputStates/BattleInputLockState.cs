using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInputLockState : BaseState
{

    public override StateIdentificationSO OnHoldState()
    {
        return CheckStateCondition();
    }
}
