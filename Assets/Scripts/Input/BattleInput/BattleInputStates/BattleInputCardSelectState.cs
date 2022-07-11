using System;
using System.Collections;
using System.Collections.Generic;
using Battles.UI;
using UnityEngine;

public class BattleInputCardSelectState : BaseState
{
    [SerializeField] private HandUI _handUI;
    
    private IDisposable _selectedToken;

    public override void OnEnterState()
    {
        
    }

    public override void OnExitState()
    {
        
    }

    public override StateIdentificationSO OnHoldState()
    {
        return CheckStateCondition();
    }
}
