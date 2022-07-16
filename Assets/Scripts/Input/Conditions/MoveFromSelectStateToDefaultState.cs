﻿

using Battles.UI;
using UnityEngine;

public class MoveFromSelectStateToDefaultState : BaseCondition
{
    private bool _moveCondition = false;
    
    public override bool CheckCondition()
    {
        return _moveCondition;
    }

    public override void InitCondition()
    {
        _moveCondition = false;
        HandUI.OnCardReturnToHand += ChangeState;
    }
    
    private void ChangeState()
    {
        HandUI.OnCardReturnToHand -= ChangeState;
        _moveCondition = true;
    }
}
