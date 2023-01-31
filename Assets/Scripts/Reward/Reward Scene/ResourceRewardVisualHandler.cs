using CardMaga.SequenceOperation;
using CardMaga.UI;
using ReiTools.TokenMachine;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
namespace CardMaga.Rewards
{
    public class ResourceRewardVisualHandler : BaseUIElement , ISequenceOperation ,IComparable<ResourceRewardVisualHandler>
    {
 
        [SerializeField]
        private CurrencyType _currencyType;
        [SerializeField]
        private int _priority;

        [ShowInInspector,ReadOnly]
        private int _value;
        
        public int Amount { get => _value; }
        public CurrencyType CurrencyType { get => _currencyType; }
        public bool HasValue => _value > 0;

        public int Priority => _priority;

        public void AddValue(int amount)
        {
            _value += amount;
        }

        public int CompareTo(ResourceRewardVisualHandler other)
        {
            if (Priority < other.Priority)
                return -1;
            else if (Priority == other.Priority)
                return 0;
            else
                return 1;
        }
        public void ExecuteTask(ITokenReciever tokenMachine)
        {
            Show();
        }
    }


}