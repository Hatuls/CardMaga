﻿using System.Collections;
using System.Collections.Generic;
using Battles.UI;
using UnityEngine;

public class MoveFromLockStateToDefaultState : BaseCondition
{
    private bool _moveCondition = false;
    
    public override bool CheckCondition()
    {
        return _moveCondition;
    }

    public override void InitCondition()
    {
        _moveCondition = false;
        HandUI.OnCardDrawnAndAlign += ChangeState;
    }

    private void ChangeState()
    {
        HandUI.OnCardDrawnAndAlign -= ChangeState;
        _moveCondition = true;
    }
}
