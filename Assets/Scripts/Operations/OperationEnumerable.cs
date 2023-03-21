using ReiTools.TokenMachine;
using System;
using System.Collections;
using System.Collections.Generic;

public class OperationEnumerable : IEnumerator<IOperationBehaviour>, IOperationBehaviour
{
    public event Action OnMoveNext = null;
    public event Action OnCompleted = null;
    IDisposable _disposable = null;
    int _current = -1;
    public IReadOnlyList<IOperationBehaviour> WaveOperations { get; private set; }
    public IOperationBehaviour Current => WaveOperations[_current];

    object IEnumerator.Current => Current;


    public int Order { get; private set; }

    public OperationEnumerable(IReadOnlyList<IOperationBehaviour> list,int order = 0)
    {
        WaveOperations = list;
        Order = order;
    }

    #region IOperationBehaviour Implementation
    public void Init(ITokenReceiver tokenReciever)
    {
        _disposable = tokenReciever.GetToken();

        for (int i = 0; i < WaveOperations.Count; i++)
        {
            var _currentWave = WaveOperations[i];
            _currentWave.Init(tokenReciever);
            _currentWave.OnCompleted += StartOperation;
        }
        Reset();
    }

    public void StartOperation()
    {
        if (!MoveNext())
            Completed();
    }

    public void Completed()
    {
        Dispose();
        OnCompleted?.Invoke();
        for (int i = 0; i < WaveOperations.Count; i++)
            WaveOperations[i].OnCompleted -= StartOperation;
    }
    #endregion

    #region IEnumerator Implementation
    public bool MoveNext()
    {
        _current++;
        bool isValid = _current < WaveOperations.Count;
    
        if (isValid)
        {
            OnMoveNext?.Invoke();
            Current.StartOperation();
        }

        return isValid;
    }

    public void Reset()
    => _current = -1;

    public void Dispose()
    => _disposable?.Dispose();
    #endregion
}
