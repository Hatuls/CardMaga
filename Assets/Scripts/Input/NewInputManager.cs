using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class NewInputManager : BaseStateMachine
{
    public override IState CurrentState
    {
        get { return _currentState; }
    }
    

    public override void TryChangeState(StateIdentificationSO stateID)
    {
        throw new System.NotImplementedException();
    }

    public override void ForceChangeState(StateIdentificationSO stateID)
    {
        throw new System.NotImplementedException();
    }
}
