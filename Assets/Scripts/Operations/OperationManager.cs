using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using System.Linq;
#endif
public class OperationManager : MonoBehaviour, IOperationBehaviour
{
    #region Editor
#if UNITY_EDITOR
    [SerializeField, Tooltip("Optional")]
    private int _order;
    public int Order => _order;

    [Sirenix.OdinInspector.Button()]
    private void AssignOperationInChildren()
    {
        var operationsArray = GetComponentsInChildren<BaseOperation>();

        if (operationsArray.Length == 0)
        {
            Debug.LogWarning($"OperationManager: Could not find BaseOperations in children");
            return;
        }
      
        _operations.Clear();
        _operations.AddRange(operationsArray);
        _operations =  _operations.OrderBy(x => x.Order).ToList();
    }
#endif
    #endregion

    [SerializeField]
    private List<BaseOperation> _operations;
    private OperationEnumerable _operationsEnumerable;
    public event Action OnCompleted;
    IDisposable _disposable;
    
    public void Completed()
    {
        OnCompleted?.Invoke();
        _disposable?.Dispose();
        _operationsEnumerable.OnCompleted -= Completed;
    }

    public void Init(ITokenReciever tokenReciever)
    {
        _operationsEnumerable = new OperationEnumerable(_operations);
        _operationsEnumerable.Init(tokenReciever);

        _disposable = tokenReciever?.GetToken();
        _operationsEnumerable.OnCompleted += Completed;
    }
    
    public void StartOperation()
    {
        _operationsEnumerable.StartOperation();
    }
}

public interface IOperationBehaviour
{
    event Action OnCompleted;
    void Init(ITokenReciever tokenReciever);
    void StartOperation();
    void Completed();

#if UNITY_EDITOR
    int Order { get; }
#endif

}

public abstract class BaseOperation : MonoBehaviour, IOperationBehaviour
{

#if UNITY_EDITOR
    [SerializeField]
    private int _order;
    public int Order => _order;
#endif
    protected TokenMachine _tokenMachine;

    protected IDisposable _token = null;

    public virtual event Action OnCompleted;
    public abstract void Completed();
    public abstract void Init(ITokenReciever tokenReciever);
    public abstract void StartOperation();
}
