using ReiTools.TokenMachine;
using System;
using UnityEngine;

public class TaskBasedOperation : BaseOperation
{
    public override event Action OnCompleted;
    [SerializeField]
    private UnityTokenMachineEvent _tokenEvent;
    public override void Completed()
    {
        _token.Dispose();
       OnCompleted?.Invoke();
    }

    public override void Init(ITokenReciever tokenReciever)
    {
        _token = tokenReciever.GetToken();
        _tokenMachine = new TokenMachine(Completed);
        //StartOperation();
    }

    public override void StartOperation()
    {
        _tokenEvent?.Invoke(_tokenMachine);
    }
}
