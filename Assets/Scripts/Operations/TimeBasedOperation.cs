using ReiTools.TokenMachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TimeBasedOperation : BaseOperation
{
    [SerializeField, EventsGroup]
    private UnityTokenMachineEvent OnOperationStarting;
    [SerializeField, EventsGroup]
    UnityEvent OnOperationFinished;

    [SerializeField,Range(0f,10f), Tooltip("The delay before the operation will be executed")]
    private float _delayBeforeOperation =0f;
    [SerializeField,Range(0f,10f), Tooltip("The delay after the operation was executed\nNote: Will not have effect if the operation that will take the token wont release it before this delay")]
    private float _delayAfterOperation = 0f;

    public override event Action OnCompleted;
    public override void Completed()
    {
        _token?.Dispose();
        OnOperationFinished?.Invoke();
        OnCompleted?.Invoke();
    }

    public override void Init(ITokenReciever tokenReciever)
    {
        _token = tokenReciever.GetToken();
        _tokenMachine = new TokenMachine(Completed);
    }

    public override void StartOperation()
    {
        StartCoroutine(Delay());
    }
    protected virtual IEnumerator Delay()
    {
        using (_tokenMachine.GetToken())
        {
            if (_delayBeforeOperation > 0)
                yield return new WaitForSeconds(_delayBeforeOperation);

            OnOperationStarting.Invoke(_tokenMachine);

            if (_delayAfterOperation > 0)
                yield return new WaitForSeconds(_delayAfterOperation);
        }
    }
}