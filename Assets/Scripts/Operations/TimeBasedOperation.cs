using ReiTools.TokenMachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Rei.Utilities;

public class TimeBasedOperation : BaseOperation
{
    public override event Action OnCompleted;
    
    [SerializeField, EventsGroup]
    private UnityTokenMachineEvent OnOperationStarting;
    [SerializeField, EventsGroup]
    UnityEvent OnOperationFinished;
    [SerializeField, EventsGroup]
    UnityEvent OnOperationCancel;

    [SerializeField,MinMaxSlider(0,20f), Tooltip("The delay before the operation will be executed")]
    private Vector2 _delayBeforeOperation;
    [SerializeField, MinMaxSlider(0, 20f), Tooltip("The delay after the operation was executed\nNote: Will not have effect if the operation that will take the token wont release it before this delay")]
    private Vector2 _delayAfterOperation;

    private bool _isCancelled;
    private bool _isExecute;

    public float DelayBeforeOperation
    {
        set
        {
            _delayBeforeOperation.x = value;
            _delayBeforeOperation.y = value;
        } 
    }

    public override void Completed()
    {
        if (_isCancelled)
            return;
        
        _token?.Dispose();
        OnOperationFinished?.Invoke();
        OnCompleted?.Invoke();
    }

    public override void Init(ITokenReceiver tokenReciever)
    {
        _token = tokenReciever.GetToken();
        _tokenMachine = new TokenMachine(Completed);
        _isCancelled = false;
        _isExecute = false;
    }

    public void CancelOperation()
    {
        if (_isExecute)
            return;
        
        _isCancelled = true;
        OnOperationCancel?.Invoke();
        StopAllCoroutines();
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

            if (_isCancelled)
            {
                yield break;
            }
            
            OnOperationStarting.Invoke(_tokenMachine);
            _isExecute = true;

            float delayAfter = _delayAfterOperation.GetRandomValue();
            if (delayAfter > 0)
                yield return new WaitForSeconds(delayAfter);
        }
    }


}
