using UnityEngine;

public abstract class BaseCondition : MonoBehaviour
{
    [SerializeField] private StateIdentificationSO _nextState;
    protected bool _moveCondition;
    public StateIdentificationSO NextState => _nextState;

    public abstract bool CheckCondition();


    public virtual void InitCondition()
    {
    }

    public void ResetCondition()
    {
        _moveCondition = false;
    }
}