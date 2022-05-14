using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : MonoBehaviour , IState
{
    public StateIdentificationSO StateID { get; }
    
    public BaseCondition[] Conditions { get; }

    public virtual void OnEnterState(){}
    

    public virtual void OnExitState()
    {
        throw new System.NotImplementedException();
    }

    public abstract StateIdentificationSO OnHoldState();
    

    public virtual StateIdentificationSO CheckStateCondition()
    {
        return StateID;
    }
}
