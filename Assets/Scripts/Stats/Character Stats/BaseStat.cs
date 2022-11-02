using CardMaga.Keywords;
using System;

namespace Characters.Stats
{
    public abstract class BaseStat
    {
        public event Action<int, KeywordType> OnStatsUpdated;
        public event Action<int> OnValueChanged;
        private int _amount;
        public abstract KeywordType Keyword { get; }
        public int Amount
        {
            get => _amount; 
            protected set
            {
                if (_amount != value)
                {
                    _amount = value;
                    InvokeStatUpdate();
                }

            }
        }

        public virtual bool IsEmpty => Amount == 0;

        public BaseStat(int amount)
        {
            Amount = amount;
            InvokeStatUpdate();
        }
        protected void InvokeStatUpdate()
        {
            OnValueChanged?.Invoke(Amount);
            OnStatsUpdated?.Invoke(Amount, Keyword);
        }
        public virtual void Add(int amount)
        {
            Amount += amount;
            InvokeStatUpdate();
        }
        public virtual void Reduce(int amount)
        {
            Amount -= amount;
            InvokeStatUpdate();
        }
        public virtual bool HasValue() => Amount > 0;
        public virtual bool HasValue(int amount) => Amount > amount;
        public virtual void Reset(int value = 0)
        {
            Amount = value;
            InvokeStatUpdate();
        }
    }

}