using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : MonoBehaviour , IState
{
    [SerializeField] protected StateIdentificationSO _stateID;

    [SerializeField] protected BaseCondition[] _conditions;
    public  StateIdentificationSO StateID
    {
        get { return _stateID; }
    }
    
    public BaseCondition[] Conditions
    {
        get { return _conditions; }
    }

    public virtual void OnEnterState(){}


    public virtual void OnExitState()
    {
    }

    public abstract StateIdentificationSO OnHoldState();
    

    public virtual StateIdentificationSO CheckStateCondition()
    {
        return StateID;
    }
}
