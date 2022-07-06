using ReiTools.TokenMachine;
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

   

    public virtual void OnEnterState()
    {
        for (int i = 0; i < _conditions.Length; i++)
        {
            _conditions[i].InitCondition();
        }
        
        Debug.Log("Enter " + base.name);
    }

    protected void LockTouchableItems()
    {
        
    }
    protected void UnLockTouchableItems()
    {
        
    }


    public virtual void OnExitState()
    {
       
    }

    public virtual StateIdentificationSO OnHoldState()
    {
        return CheckStateCondition();
    }
    
    

    public virtual StateIdentificationSO CheckStateCondition()
    {
        for (int i = 0; i < Conditions.Length; i++)
        {
            if (Conditions[i].CheckCondition())
            {
                Debug.Log("Move State from: " + name + " To: " + Conditions[i].NextState);
                OnExitState();
                return Conditions[i].NextState;
            }
        }
        return StateID;
    }

    public void AddTouchableItem(TouchableItem touchableItem)
    {
        
    }
    
    public void AddTouchableItem(TouchableItem[] touchableItem)
    {
        
    }
    
    public void RemoveTouchableItem(TouchableItem touchableItem)
    {
        
    }
}
