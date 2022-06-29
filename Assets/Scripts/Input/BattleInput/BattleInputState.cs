using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInputState : BaseState
{
    [SerializeField] private BattleInputStateMachine _battleInputStateMachine;
    
    public override void OnEnterState()
    {
       _battleInputStateMachine.InitStateMachine();
    }

    public override StateIdentificationSO OnHoldState()
    {
        _battleInputStateMachine.TryChangeState(_battleInputStateMachine.CurrentState.OnHoldState());
        return base.CheckStateCondition();
    }
}
