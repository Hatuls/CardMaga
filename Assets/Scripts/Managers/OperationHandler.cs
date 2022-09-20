using ReiTools.TokenMachine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CardMaga.Sequence
{
    public class OperationHandler<T> : IEnumerator<T>, ICollection<T>, IComparer<T>
        where T : IOrderable
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
            Sort();
        }

        public void Clear()
        => _operations.Clear();
        public void Sort()
            => _operations.Sort(this);

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

        public int Compare(T x, T y)
        {
            if (x.Priority < y.Priority)
                return -1;
            else if (x.Priority > y.Priority)
                return 1;
            else
                return 0;
        }
        #endregion
    }


    #region Operation Tasks 
    public abstract class BaseOperationTask : IDisposable
    {
        public event Action OnDispose;

        public int Priority { get; private set; }
        public OrderType Order { get; private set; }

        public BaseOperationTask(int priority = 0, OrderType orderType = OrderType.Default)
        {
            Priority = priority;
            Order = orderType;
        }
        public void Dispose() => OnDispose?.Invoke();

    }
    public class OperationTask<T> : BaseOperationTask, ISequenceOperation<T>
    {
        public event Action<ITokenReciever,T> OnExecuteTask;
        public OperationTask(Action<ITokenReciever,T> onExecuteTask, int priority = 0, OrderType orderType = OrderType.Default) : base(priority, orderType)
        {
            OnExecuteTask = onExecuteTask;
        }

        public void ExecuteTask(ITokenReciever tokenMachine, T data)
        => OnExecuteTask?.Invoke(tokenMachine,data);
    }
    public class OperationTask : BaseOperationTask, ISequenceOperation
    {
        public  event Action<ITokenReciever> OnExecuteTask;
        public OperationTask(Action<ITokenReciever> onExecuteTask,int priority = 0, OrderType orderType = OrderType.Default):base(priority, orderType)
        {
            OnExecuteTask = onExecuteTask;
        }

        public void ExecuteTask(ITokenReciever tokenMachine)
        => OnExecuteTask?.Invoke(tokenMachine);
    }

    #endregion

    #region Interfaces
    public interface IOrderable
    {
        /// <summary>
        ///  The priority of the order it will be executed in the current timeline
        ///  <br></br>
        ///  <Example>-1 will be executed before all bigger than -1</Example>
        /// </summary>
        int Priority { get; }
    }
    public interface ISequenceOperation<T> : IOrderable
    {
        /// <summary>
        /// Sequence is seperated to 3 timelines<br></br>
        /// Before   <br></br>
        /// Default<br></br>
        /// After<br></br>
        /// Note* -> the above is also the order of execution
        /// </summary>
       // Battle.OrderType Order { get; }
        /// <summary>
        /// The function that will be executed when called
        /// </summary>
        /// <param name="tokenMachine"></param>
        /// <param name="data">data that will be passed for this execution</param>
        /// 
        void ExecuteTask(ITokenReciever tokenMachine, T data);
    }
    public interface ISequenceOperation : IOrderable
    {        /// <summary>
             /// Sequence is seperated to 3 timelines<br></br>
             /// Before   <br></br>
             /// Default<br></br>
             /// After<br></br>
             /// Note* -> the above is also the order of execution
             /// </summary>
      //  Battle.OrderType Order { get; }
        /// <summary>
        /// The function that will be executed when called
        /// </summary>
        /// <param name="tokenMachine"></param>
        void ExecuteTask(ITokenReciever tokenMachine);
    }
    #endregion
}










