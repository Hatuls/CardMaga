using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public abstract class BaseStateMachine : MonoBehaviour , IStateMachine
{
    protected BaseState _currentState;

    protected Dictionary<StateIdentificationSO, BaseState> _inputStateDict;

    [SerializeField] private List<BaseState> _states;

    public StateIdentificationSO FirstState;

    public  BaseState CurrentState
    {
        get { return _currentState; }
    }

    public List<BaseState> States
    {
        get { return _states; }
    }
    
    
    public void InitStateMachine()
    {
        _inputStateDict = new Dictionary<StateIdentificationSO, BaseState>();

        for (int i = 0; i < _states.Count; i++)
        {
            _inputStateDict.Add(_states[i].StateID,_states[i]);
        }

        Debug.Log(base.name + " Init");
        TryChangeState(FirstState);
    }
    
    public virtual void TryChangeState(StateIdentificationSO stateID)//need work
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
            {
                return;
            }
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

        if (_currentState != null)
        {
            _currentState.OnExitState();
        }
        
        _currentState = newState;
        
        _currentState.OnEnterState();
    }

}
