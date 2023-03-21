using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class BaseStateMachine : MonoBehaviour, IStateMachine
{
    [SerializeField] [ReadOnly] protected BaseState _currentState;

    [SerializeField] private List<BaseState> _states;

    public StateIdentificationSO FirstState;

    protected Dictionary<StateIdentificationSO, BaseState> _inputStateDict;

    public List<BaseState> States => _states;

    public BaseState CurrentState => _currentState;

    public virtual void TryChangeState(StateIdentificationSO stateID) //need work
    {
        BaseState newState;

        if (!_inputStateDict.TryGetValue(stateID, out newState))
        {
            Debug.LogError("State " + stateID + "not found");
            return;
        }

        if (_currentState != null)
        {
            if (_currentState == newState) 
                return;
            
            _currentState.OnExitState();
        }

        _currentState = newState;

        _currentState.OnEnterState();
    }


    public virtual void ForceChangeState(StateIdentificationSO stateID) //need work!
    {
        BaseState newState;

        if (!_inputStateDict.TryGetValue(stateID, out newState))
        {
            Debug.LogError("State " + stateID + "not found");
            return;
        }

        if (_currentState != null) _currentState.OnExitState();

        _currentState = newState;

        _currentState.OnEnterState();
    }


    public void InitStateMachine()
    {
        _inputStateDict = new Dictionary<StateIdentificationSO, BaseState>();

        for (var i = 0; i < _states.Count; i++) _inputStateDict.Add(_states[i].StateID, _states[i]);

        Debug.Log(name + " Init");
        TryChangeState(FirstState);
    }
}