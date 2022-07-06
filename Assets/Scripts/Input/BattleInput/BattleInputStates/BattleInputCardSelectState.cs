using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInputCardSelectState : BaseState
{
    [SerializeField] private SelectCardUI _selectCardUI;
    
    private IDisposable _selectedToken;

    public override void OnEnterState()
    {
        _selectedToken = _selectCardUI.SelectLockTokenReceiver.GetToken();
    }

    public override void OnExitState()
    {
        _selectedToken.Dispose();
    }

    public override StateIdentificationSO OnHoldState()
    {
        throw new System.NotImplementedException();
    }
}
