using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateMachine
{
    IState CurrentState { get; }

    void TryChangeState(StateIdentificationSO stateID);
    
    void ForceChangeState(StateIdentificationSO stateID);
}
