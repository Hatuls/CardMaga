using UnityEngine;

public abstract class BaseState : MonoBehaviour, IState
{
    [SerializeField] protected StateIdentificationSO _stateID;

    [SerializeField] protected BaseCondition[] _conditions;

    public StateIdentificationSO StateID => _stateID;

    public BaseCondition[] Conditions => _conditions;

    public virtual void OnEnterState()
    {
        for (var i = 0; i < _conditions.Length; i++) _conditions[i].InitCondition();
    }

    public virtual void OnExitState()
    {
        for (var i = 0; i < _conditions.Length; i++) _conditions[i].ResetCondition();
    }

    public virtual StateIdentificationSO OnHoldState()
    {
        return CheckStateCondition();
    }


    public virtual StateIdentificationSO CheckStateCondition()
    {
        for (var i = 0; i < Conditions.Length; i++)
            if (Conditions[i].CheckCondition())
            {
#if UNITY_EDITOR
                Debug.Log("Move State from: " + name + " To: " + Conditions[i].NextState);
#endif
                return Conditions[i].NextState;
            }

        return StateID;
    }
}