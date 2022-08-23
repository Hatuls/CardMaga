﻿using Managers;
using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;

namespace Battle
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
                        if (_default == null)
                            _default = new OperationHandler<ISequenceOperation>();
                        return _default;

                    case OrderType.After:
                        if (_after == null)
                            _after = new OperationHandler<ISequenceOperation>();
                        return _after;

                    case OrderType.Before:
                    default:
                        if (_early == null)
                            _early = new OperationHandler<ISequenceOperation>();
                        return _early;
                }
            }
        }
        public void Register(ISequenceOperation sequenceOperation, OrderType to = OrderType.Default)
        => this[to].Add(sequenceOperation);

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
        public void ExecuteTask(ITokenReciever tokenMachine)
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
            foreach (var item in operation)
                item.ExecuteTask(tokenMachine);

        }

        #endregion
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


        public void Start(T data, Action OnComplete = null)
        {
            StartOperation(this[OrderType.Before], data, StartScene);
            void StartScene() => StartOperation(this[OrderType.Default], data, LateStartScene);
            void LateStartScene() => StartOperation(this[OrderType.After], data, OnComplete);
        }

        private void StartOperation(OperationHandler<ISequenceOperation<T>> operation, T data, Action OnComplete)
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
