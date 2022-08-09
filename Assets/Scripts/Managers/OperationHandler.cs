using ReiTools.TokenMachine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Managers
{
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

        public void Add(T item)
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

    public interface ISequenceOperation : IComparable<int>
    {
        int Order { get; }

        void Invoke(ITokenReciever tokenMachine);
    }

    public class SequenceOperation : ISequenceOperation
    {
        public SequenceOperation(Action<ITokenReciever> operation, int order)
        {
            Operation = operation;
            Order = order;
        }

        public event Action<ITokenReciever> Operation;
        public int Order { get; private set; }

        public int CompareTo(int other)
        {
            if (Order < other)
                return -1;
            else if (Order > other)
                return 1;
            else
                return 0;
        }

        public void Invoke(ITokenReciever tokenMachine)
       => Operation.Invoke(tokenMachine);
    }
}
