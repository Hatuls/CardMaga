using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStateMachineNew : MonoBehaviour
{
    [SerializeField] private CardStateBase _firstState;
    private CardStateBase _currnetState;
    void Start()
    {
        _currnetState = _firstState;
        _currnetState.OnEnterState();
    }

    public void MoveState(CardStateBase nextState)
    {
        if (_currnetState != nextState)
        {
            _currnetState.OnExitState();
            _currnetState = nextState;
            _currnetState.OnEnterState();
        }
    }
}
