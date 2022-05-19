using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
using UnityEngine;

public class OperationManager : MonoBehaviour, IOperationBehaviour
{
 
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
        _operationsEnumerable.OnCompleted += Completed;
        StartOperation();
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

}

public abstract class BaseOperation : MonoBehaviour, IOperationBehaviour
{

    protected IDisposable _token = null;
    public virtual event Action OnCompleted;

    public abstract void Completed();
    public abstract void Init(ITokenReciever tokenReciever);
    public abstract void StartOperation();
}
