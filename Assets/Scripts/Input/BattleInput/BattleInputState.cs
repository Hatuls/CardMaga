using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInputState : BaseState
{
    [SerializeField] private BattleInputStateMachine _battleInputStateMachine;
    
    public override void OnEnterState()
    {
        _battleInputStateMachine.TryChangeState(_battleInputStateMachine.FirstState);
    }

    public override StateIdentificationSO OnHoldState()
    {
        return StateID;
    }
}
