using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public abstract class BaseStateMachine : MonoBehaviour , IStateMachine
{
    protected BaseState _currentState;

    protected Dictionary<StateIdentificationSO, BaseState> _inputStateDict;

    [SerializeField] private List<BaseState> States;

    public StateIdentificationSO FirstState;

    public  IState CurrentState
    {
        get { return _currentState; }
    }

    private void Awake()
    {
        InitStateMachine();
        TryChangeState(FirstState);
    }

    public void Update()
    {
        if (_currentState == null)
        {
            return;
        }
        
        TryChangeState(_currentState.OnHoldState());
    }

    protected virtual void InitStateMachine()
    {
        _inputStateDict = new Dictionary<StateIdentificationSO, BaseState>();

        for (int i = 0; i < States.Count; i++)
        {
            _inputStateDict.Add(States[i].StateID,States[i]);
        }
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
