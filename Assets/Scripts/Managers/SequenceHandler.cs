using System;
using System.Collections.Generic;
using ReiTools.TokenMachine;

namespace CardMaga.SequenceOperation
{
    public enum OrderType { Before, Default, After }
    
    public class SequenceHandler : ISequenceOperation
    {
        #region Fields
        private OperationHandler<ISequenceOperation> _early;
        private OperationHandler<ISequenceOperation> _default;
        private OperationHandler<ISequenceOperation> _after;
        #endregion
        #region Properties
        public IReadOnlyCollection<ISequenceOperation> Early => _early as IReadOnlyCollection<ISequenceOperation>;
        public IReadOnlyCollection<ISequenceOperation> Default => _default as IReadOnlyCollection<ISequenceOperation>;
        public IReadOnlyCollection<ISequenceOperation> After => _after as IReadOnlyCollection<ISequenceOperation>;

        public OrderType Order { get; private set; }
        public int Priority { get; private set; }
        #endregion

        #region Consturctor
        public SequenceHandler(int priorty = 0, OrderType orderType = OrderType.Default)
        {
            Order = orderType;
            Priority = priorty;
        }

        #endregion

        #region Public Functions
        public OperationHandler<ISequenceOperation> this[OrderType type]
        {
            get
            {
                switch (type)
                {
                    case OrderType.Default:
                        return _default;

                    case OrderType.After:
                        return _after;

                    case OrderType.Before:
                    default:
                        return _early;
                }
            }
        }
        public void Register(ISequenceOperation sequenceOperation, OrderType to = OrderType.Default)
        {
            if (this[to] == null)
                InitList(to);
            this[to].Add(sequenceOperation);
        }
        public void Register(Action<ITokenReceiver> token, int priority = 0, OrderType to = OrderType.Default) => Register(new OperationTask(token, priority, to));

        public bool Remove(ISequenceOperation sequenceOperation, OrderType from = OrderType.Default)
             => this[from].Remove(sequenceOperation);
        public void OnDestroy()
        {
            Reset(_early);
            Reset(_default);
            Reset(_after);
            _early = null;
            _default = null;
            _after = null;

            void Reset(OperationHandler<ISequenceOperation> operation)
            {
                operation?.Clear();
                operation?.Dispose();
            }
        }
        public void Start(OrderType operationType, Action OnComplete = null)
            => StartOperation(this[operationType], OnComplete);
        public void StartAll(Action OnComplete = null)
        {
            StartOperation(this[OrderType.Before], StartScene);
            void StartScene() => StartOperation(this[OrderType.Default], LateStartScene);
            void LateStartScene() => StartOperation(this[OrderType.After], OnComplete);
        }
        public void ExecuteTask(ITokenReceiver tokenMachine)
        {
            IDisposable token = tokenMachine?.GetToken();
            StartAll(token.Dispose);
        }
        public void ExecuteTask(Action onComplete = null)
        {
            IDisposable token = new TokenMachine(onComplete).GetToken();
            StartAll(token.Dispose);
        }
        #endregion

        #region Private Functions
        private void StartOperation(OperationHandler<ISequenceOperation> operation, Action OnComplete)
        {
            if (operation == null || operation.Count == 0)
            {
                OnComplete?.Invoke();
                return;
            }

            TokenMachine tokenMachine = new TokenMachine(OnComplete);
            operation.Sort();
            IDisposable t = tokenMachine.GetToken();
            using (t)
            {
                foreach (var item in operation)
                    item.ExecuteTask(tokenMachine);
            }

        }
        private void InitList(OrderType type)
        {
            switch (type)
            {
                case OrderType.Default:
                    if (_default == null)
                        _default = new OperationHandler<ISequenceOperation>();
                    break;

                case OrderType.After:
                    if (_after == null)
                        _after = new OperationHandler<ISequenceOperation>();
                    break;

                case OrderType.Before:
                default:
                    if (_early == null)
                        _early = new OperationHandler<ISequenceOperation>();
                    break;
            }
        }
        #endregion
    }
    
    public class SequenceHandler<T> : ISequenceOperation<T>, IDisposable
    {
        private OperationHandler<ISequenceOperation<T>> _early  ;
        private OperationHandler<ISequenceOperation<T>> _default;
        private OperationHandler<ISequenceOperation<T>> _after  ;
        private OrderType _order;
        private int _priority;
        public OrderType Order { get => _order; private set => _order = value; }

        public int Priority { get => _priority; private set => _priority = value; }
        public SequenceHandler(int priorty = 0, OrderType orderType = OrderType.Default)
        {
            Order = orderType;
            Priority = priorty;
        }
     
        public OperationHandler<ISequenceOperation<T>> this[OrderType type]
        {
            get
            {
                switch (type)
                {
                    case OrderType.Default:
                        return _default;
                    case OrderType.After:
                        return _after;
                    case OrderType.Before:
                    default:
                        return _early;
                }
            }
        }



        public void Register(ISequenceOperation<T> sequenceOperation, OrderType to = OrderType.Default)
        {
            if (this[to] == null)
                InitList(to);
            this[to].Add(sequenceOperation);
        }



        public bool Remove(ISequenceOperation<T> sequenceOperation, OrderType from = OrderType.Default)
             => this[from].Remove(sequenceOperation);
        
        public void StartAll(T data, Action OnComplete = null)
        {
            StartOperation(this[OrderType.Before], data, StartScene);
            void StartScene() => StartOperation(this[OrderType.Default], data, LateStartScene);
            void LateStartScene() => StartOperation(this[OrderType.After], data, OnComplete);
        }
        public void ExecuteTask(ITokenReceiver tokenMachine, T data)
        {
            IDisposable token = tokenMachine?.GetToken();
            StartAll(data, token.Dispose);
        }
        public void ExecuteTask(T data, Action onComplete = null)
        {
            IDisposable token = new TokenMachine(onComplete).GetToken();
            StartAll(data, token.Dispose);
        }
        private void StartOperation(OperationHandler<ISequenceOperation<T>> operation, T data, Action OnComplete)
        {
            if (operation == null|| operation.Count == 0)
            {
                OnComplete?.Invoke();
                return;
            }

            TokenMachine tokenMachine = new TokenMachine(OnComplete);
            operation.Sort();
            using (tokenMachine.GetToken())
            {
                foreach (var item in operation)
                    item.ExecuteTask(tokenMachine, data);
            }
        }
      
        private void InitList(OrderType type)
        {
            switch (type)
            {
                case OrderType.Default:
                    if (_default == null)
                        _default = new OperationHandler<ISequenceOperation<T>>();
                    break;

                case OrderType.After:
                    if (_after == null)
                        _after = new OperationHandler<ISequenceOperation<T>>();
                    break;

                case OrderType.Before:
                default:
                    if (_early == null)
                        _early = new OperationHandler<ISequenceOperation<T>>();
                    break;
            }
        }

        public void Dispose()
        {
            Reset(_early);
            Reset(_default);
            Reset(_after);
            _early = null;
            _default = null;
            _after = null;

            void Reset(OperationHandler<ISequenceOperation<T>> operation)
            {
                operation.Clear();
                operation.Dispose();
            }
        }
    }
}
