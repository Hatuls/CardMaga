using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCondition : MonoBehaviour
{
    [SerializeField] private StateIdentificationSO _nextState;

    public  StateIdentificationSO NextState
    {
        get { return _nextState; }
    }

    public abstract bool CheckCondition();

    public virtual void InitCondition(){}
    
    
}
