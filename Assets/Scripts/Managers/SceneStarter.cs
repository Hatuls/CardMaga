using Managers;
using ReiTools.TokenMachine;
using System;

namespace Battle
{
    public class SceneStarter
    {
        public enum SequenceType { Early, Default, Late }
        private  OperationHandler<ISequenceOperation> _early = new OperationHandler<ISequenceOperation>();
        private  OperationHandler<ISequenceOperation> _default = new OperationHandler<ISequenceOperation>();
        private  OperationHandler<ISequenceOperation> _late = new OperationHandler<ISequenceOperation>();
        private OperationHandler<ISequenceOperation> Get(SequenceType type)
        {
            switch (type)
            {
                case SequenceType.Default:
                    return _default;
                case SequenceType.Late:
                    return _late;
                case SequenceType.Early:
                default:
                    return _early;
            }
        }



        public void Register(ISequenceOperation sequenceOperation, SequenceType to = SequenceType.Early)
        => Get(to).Add(sequenceOperation);

        public void Remove(ISequenceOperation sequenceOperation, SequenceType from = SequenceType.Early)
             => Get(from).Remove(sequenceOperation);


        public void Start(Action OnComplete = null)
        {
            StartOperation(Get(SequenceType.Early), StartScene);
            void StartScene() => StartOperation(Get(SequenceType.Default), LateStartScene);
            void LateStartScene() => StartOperation(Get(SequenceType.Late), OnComplete);
        }

        private void StartOperation(OperationHandler<ISequenceOperation> operation, Action OnComplete)
        {
            TokenMachine tokenMachine = new TokenMachine(OnComplete);
            using (tokenMachine.GetToken())
            {
                foreach (var item in operation)
                    item.Invoke(tokenMachine);
            }
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
}
