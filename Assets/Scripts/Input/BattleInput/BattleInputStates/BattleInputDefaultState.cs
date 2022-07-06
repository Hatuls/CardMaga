using System;
using Battles.UI;
using UnityEngine;

public class BattleInputDefaultState : BaseState
{
    [SerializeField] private HandUI _handUI;
    
    private IDisposable _handToken;
    
    
    public override void OnEnterState()
    {
        base.OnEnterState();
        
        _handToken = _handUI.HandLockTokenReceiver.GetToken();
        
        //HandUI.OnCardDrawnAndAlign += UnLockTouchableItems;
    }

    public override void OnExitState()
    {
        _handToken.Dispose();
    }

    public override StateIdentificationSO OnHoldState()
    {
        return base.CheckStateCondition();
    }
}
