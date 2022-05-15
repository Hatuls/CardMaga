using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateMachine
{
    BaseState CurrentState { get; }

    void TryChangeState(StateIdentificationSO stateID);
    
    void ForceChangeState(StateIdentificationSO stateID);
}
