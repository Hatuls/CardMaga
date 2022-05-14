using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInputDefaultState : BaseState
{
    public override void OnEnterState()
    {
        Debug.Log("Enter Default State");
    }
    
    public override StateIdentificationSO OnHoldState()
    {
        return StateID;
    }
}
