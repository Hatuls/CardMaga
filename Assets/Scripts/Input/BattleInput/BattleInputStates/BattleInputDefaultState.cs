using Battles.UI;
using UnityEngine;

public class BattleInputDefaultState : BaseState
{
    public override void OnEnterState()
    {
        Debug.Log("Enter Default State");
        HandUI.OnCardDrawnAndAlign += UnLockTouchableItems;
    }
    
    public override StateIdentificationSO OnHoldState()
    {
        return base.CheckStateCondition();
    }
}
