using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCondition : MonoBehaviour
{
    private StateIdentificationSO _nextState;

    public StateIdentificationSO NextState { get; }

    public abstract bool CheckCondition();

    public virtual void InitCondition(){}
    
    
}
