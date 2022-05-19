using ReiTools.TokenMachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TimeBasedOperation : BaseOperation
{
    [SerializeField,EventsGroup]
    private UnityTokenMachineEvent OnOperationStarting;
    [SerializeField,EventsGroup]
    UnityEvent OnOperationFinished;
    [SerializeField]
    private float _delayTime;

    public override event Action OnCompleted;
    protected TokenMachine _timeBasedTokenMachine;
    public override void Completed()
    {
        _token?.Dispose();
        OnOperationFinished?.Invoke();
        OnCompleted?.Invoke();
    }

    public override void Init(ITokenReciever tokenReciever)
    {
        _token = tokenReciever.GetToken();
        _timeBasedTokenMachine = new TokenMachine(Completed);
    }

    public override void StartOperation()
    {
        StartCoroutine(Delay());
    }
    protected virtual IEnumerator Delay()
    {
        using (_timeBasedTokenMachine.GetToken())
        {
            OnOperationStarting.Invoke(_timeBasedTokenMachine);
            yield return new WaitForSeconds(_delayTime);
        }
    }
}