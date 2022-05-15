using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : MonoBehaviour , IState
{
    [SerializeField] protected StateIdentificationSO _stateID;

    [SerializeField] protected BaseCondition[] _conditions;

    [SerializeField] protected List<TouchableItem> _touchableItems;
    public  StateIdentificationSO StateID
    {
        get { return _stateID; }
    }
    
    public BaseCondition[] Conditions
    {
        get { return _conditions; }
    }

   

    public virtual void OnEnterState()
    {
        if (_touchableItems.Count > 0)
        {
            for (int i = 0; i < _touchableItems.Count; i++)
            {
                _touchableItems[i]._isTouchable = true;
            }
        }
    }


    public virtual void OnExitState()
    {
        if (_touchableItems.Count > 0)
        {
            for (int i = 0; i < _touchableItems.Count; i++)
            {
                _touchableItems[i]._isTouchable = false;
            }
        }
    }

    public abstract StateIdentificationSO OnHoldState();
    
    

    public virtual StateIdentificationSO CheckStateCondition()
    {
        for (int i = 0; i < Conditions.Length; i++)
        {
            if (Conditions[i].CheckCondition())
            {
                Debug.Log("Move State from: " + name + " To: " + Conditions[i].NextState);
                return Conditions[i].NextState;
            }
        }
        return StateID;
    }
}
