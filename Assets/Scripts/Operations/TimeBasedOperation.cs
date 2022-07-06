using ReiTools.TokenMachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Rei.Utilities;

public class TimeBasedOperation : BaseOperation
{
    [SerializeField, EventsGroup]
    private UnityTokenMachineEvent OnOperationStarting;
    [SerializeField, EventsGroup]
    UnityEvent OnOperationFinished;

    [SerializeField,MinMaxSlider(0,20f), Tooltip("The delay before the operation will be executed")]
    private Vector2 _delayBeforeOperation;
    [SerializeField, MinMaxSlider(0, 20f), Tooltip("The delay after the operation was executed\nNote: Will not have effect if the operation that will take the token wont release it before this delay")]
    private Vector2 _delayAfterOperation;

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
            float delayBefore = _delayBeforeOperation.GetRandomValue();
            if (delayBefore > 0)
                yield return new WaitForSeconds(delayBefore);

            OnOperationStarting.Invoke(_tokenMachine);

            float delayAfter = _delayAfterOperation.GetRandomValue();
            if (delayAfter > 0)
                yield return new WaitForSeconds(delayAfter);
        }
    }


}
