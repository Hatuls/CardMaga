using System;
using UnityEngine;

public class NewInputManager : BaseStateMachine
{
    private void Awake()
    {
        SceneHandler.OnSceneLateStart += InitStateMachine;
    }

    private void OnDestroy()
    {
        SceneHandler.OnSceneLateStart -= InitStateMachine;
    }

    public void Update()
    {
        if (_currentState == null)
        {
            return;
        }
        
        TryChangeState(_currentState.OnHoldState());
    }
}
