using System;
using UnityEngine;

public abstract class BaseCondition : MonoBehaviour
{
    //public event Action<StateIdentificationSO> OnConditionMat; 
    
    [SerializeField] private StateIdentificationSO _nextState;

    public  StateIdentificationSO NextState
    {
        get { return _nextState; }
    }

    public abstract bool CheckCondition();



    public virtual void InitCondition()
    {
        
    }
    
    
}
