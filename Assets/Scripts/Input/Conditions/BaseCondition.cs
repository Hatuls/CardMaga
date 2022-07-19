using UnityEngine;

public abstract class BaseCondition : MonoBehaviour
{
    [SerializeField] private StateIdentificationSO _nextState;

    public StateIdentificationSO NextState => _nextState;

    public abstract bool CheckCondition();


    public virtual void InitCondition()
    {
    }
}