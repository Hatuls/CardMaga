using System.Collections;
using System.Collections.Generic;
using Battles.UI;
using UnityEngine;

public class ForceMoveToLockState : BaseCondition
{
    private bool _moveCondition = false;
    
    public override bool CheckCondition()
    {
        return _moveCondition;
    }

    public override void InitCondition()
    {
        HandUI.OnDiscardAllCards += ChangeState;
        _moveCondition = false;
    }
    
    private void ChangeState()
    {
        HandUI.OnDiscardAllCards -= ChangeState;
        _moveCondition = true;
    }
}
