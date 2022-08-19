using Managers;
using ReiTools.TokenMachine;
using System;

namespace Battle
{
    public enum OrderType { Before, Default, After }
    public class SequenceHandler : ISequenceOperation
    {
        private OperationHandler<ISequenceOperation> _early = new OperationHandler<ISequenceOperation>();
        private OperationHandler<ISequenceOperation> _default = new OperationHandler<ISequenceOperation>();
        private OperationHandler<ISequenceOperation> _late = new OperationHandler<ISequenceOperation>();

        public OrderType Order { get; private set; }

        public int Priority { get; private set; }

        public OperationHandler<ISequenceOperation> this[OrderType type]
        {
            get
            {
                switch (type)
                {
                    case OrderType.Default:
                        return _default;
                    case OrderType.After:
                        return _late;
                    case OrderType.Before:
                    default:
                        return _early;
                }
            }
        }

        public SequenceHandler(int priorty = 0, OrderType orderType = OrderType.Default)
        {
            Order = orderType;
            Priority = priorty;
        }

        public void Register(ISequenceOperation sequenceOperation, OrderType to = OrderType.Default)
        => this[to].Add(sequenceOperation);

        public void Remove(ISequenceOperation sequenceOperation, OrderType from = OrderType.Default)
             => this[from].Remove(sequenceOperation);


        public void Start(Action OnComplete = null)
        {
            StartOperation(this[OrderType.Before], StartScene);
            void StartScene() => StartOperation(this[OrderType.Default], LateStartScene);
            void LateStartScene() => StartOperation(this[OrderType.After], OnComplete);
        }
        public void ExecuteTask(ITokenReciever tokenMachine)
        {
            IDisposable token = tokenMachine?.GetToken();
            Start(token.Dispose);
        }
        private void StartOperation(OperationHandler<ISequenceOperation> operation, Action OnComplete)
        {
            if(operation.Count == 0)
            {
                OnComplete?.Invoke();
                return;
            }

            TokenMachine tokenMachine = new TokenMachine(OnComplete);
            operation.Sort();
                foreach (var item in operation)
                    item.ExecuteTask(tokenMachine);
            
        }
        public void OnDestroy()
        {
            Reset(_early);
            Reset(_default);
            Reset(_late);
            _early = null;
            _default = null;
            _late = null;

            void Reset(OperationHandler<ISequenceOperation> operation)
            {
                operation.Clear();
                operation.Dispose();
            }
        }
    }



    public class SequenceHandler<T>
    {
        private OperationHandler<ISequenceOperation<T>> _early = new OperationHandler<ISequenceOperation<T>>();
        private OperationHandler<ISequenceOperation<T>> _default = new OperationHandler<ISequenceOperation<T>>();
        private OperationHandler<ISequenceOperation<T>> _late = new OperationHandler<ISequenceOperation<T>>();
        public OperationHandler<ISequenceOperation<T>> this[OrderType type]
        {
            get
            {
                switch (type)
                {
                    case OrderType.Default:
                        return _default;
                    case OrderType.After:
                        return _late;
                    case OrderType.Before:
                    default:
                        return _early;
                }
            }
        }



        public void Register(ISequenceOperation<T> sequenceOperation, OrderType to = OrderType.Default)
        => this[to].Add(sequenceOperation);

        public void Remove(ISequenceOperation<T> sequenceOperation, OrderType from = OrderType.Default)
             => this[from].Remove(sequenceOperation);


        public void Start(T data,Action OnComplete = null)
        {
            StartOperation(this[OrderType.Before], data, StartScene);
            void StartScene() => StartOperation(this[OrderType.Default], data, LateStartScene);
            void LateStartScene() => StartOperation(this[OrderType.After], data, OnComplete);
        }

        private void StartOperation(OperationHandler<ISequenceOperation<T>> operation,T data, Action OnComplete)
        {
            if (operation.Count == 0)
            {
                OnComplete?.Invoke();
                return;
            }

            TokenMachine tokenMachine = new TokenMachine(OnComplete);
            operation.Sort();
            foreach (var item in operation)
                item.ExecuteTask(tokenMachine, data);

        }
        public void OnDestroy()
        {
            Reset(_early);
            Reset(_default);
            Reset(_late);
            _early = null;
            _default = null;
            _late = null;

            void Reset(OperationHandler<ISequenceOperation<T>> operation)
            {
                operation.Clear();
                operation.Dispose();
            }
        }
    }
}
