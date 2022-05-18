﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public abstract class BaseStateMachine : MonoBehaviour , IStateMachine
{
    

    protected BaseState _currentState;

    protected Dictionary<StateIdentificationSO, BaseState> _inputStateDict;

    [FormerlySerializedAs("States")] [SerializeField] private List<BaseState> _states;

    public StateIdentificationSO FirstState;

    public  BaseState CurrentState
    {
        get { return _currentState; }
    }

    private void Awake()
    {
        InitStateMachine();
    }
    
    protected virtual void InitStateMachine()
    {
        _inputStateDict = new Dictionary<StateIdentificationSO, BaseState>();

        for (int i = 0; i < _states.Count; i++)
        {
            for (int j = 0; j < _states[i].Conditions.Length; j++)
            {
                
            }
            _inputStateDict.Add(_states[i].StateID,_states[i]);
        }
        
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