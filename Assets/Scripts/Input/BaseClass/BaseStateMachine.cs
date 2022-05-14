using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStateMachine : MonoBehaviour , IStateMachine
{
    protected IState _currentState;

    public Dictionary<StateIdentificationSO, IState> _inputStateDict;
    
    public abstract IState CurrentState { get; }

    public abstract void TryChangeState(StateIdentificationSO stateID);


    public abstract void ForceChangeState(StateIdentificationSO stateID);

}
