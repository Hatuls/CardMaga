using ReiTools.TokenMachine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Managers
{
    public class OperationTasks : OperationHandler<OperationTask>
    {
        public void Add(Action<ITokenReciever> tokenReceiver, int order = 0)
        => Add(new OperationTask(tokenReceiver, order));
        public override void Add(OperationTask item)
        {
            base.Add(item);
            item.OnDispose += RemoveTask;

            void RemoveTask()
            {
               Remove(item);
               item.OnDispose -= RemoveTask;
            }
        }
        
    }
    public class OperationHandler<T> : IEnumerator<T>, ICollection<T>
        where T : ISequenceOperation
    {
        #region Fields

        private List<T> _operations = new List<T>();

        private int _index = -1;
        #endregion

        #region Interface Implementations

        public T Current => _operations[_index];


        public int Count => _operations.Count;

        public bool IsReadOnly => false;
        object IEnumerator.Current => Current;

        public void Dispose()
        {
            _operations.Clear();
        }

        public bool MoveNext()
        => _index < Count;

        public void Reset()
        {
            _index = -1;
        }

        public virtual void Add(T item)
        {
            _operations.Add(item);
            _operations.Sort();
        }

        public void Clear()
        => _operations.Clear();


        public bool Contains(T item)
   => _operations.Contains(item);
        public void CopyTo(T[] array, int arrayIndex)
      => _operations.CopyTo(array, arrayIndex);

        public bool Remove(T item)
      => _operations.Remove(item);

        public IEnumerator<T> GetEnumerator()
      => _operations.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        => _operations.GetEnumerator();
        #endregion
    }

    public interface ISequenceOperation : IComparable<ISequenceOperation>, IDisposable
    {
        int Order { get; }

        void Invoke(ITokenReciever tokenMachine);
    }

    public class OperationTask : ISequenceOperation
    {
        public event Action OnDispose;

        public event Action<ITokenReciever> OnOperationExecute;
        public int Order { get; private set; }
        public OperationTask(Action<ITokenReciever> operation, int order)
        {
            OnOperationExecute = operation;
            Order = order;
        }

        public void Invoke(ITokenReciever tokenMachine) => OnOperationExecute.Invoke(tokenMachine);

        public int CompareTo(ISequenceOperation other)
        {
            if (Order < other.Order)
                return -1;
            else if (Order > other.Order)
                return 1;
            else
                return 0;
        }

        public void Dispose() => OnDispose?.Invoke();
        
    }
}
