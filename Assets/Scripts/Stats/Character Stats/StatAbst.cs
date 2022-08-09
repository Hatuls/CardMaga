﻿using Battle.UI;
using System;

namespace Characters.Stats
{
    public abstract class StatAbst
    {
        public static Action<bool, int, Keywords.KeywordTypeEnum> _updateUIStats;
        public event Action<int> OnValueChanged;
        protected bool isPlayer;
        public abstract Keywords.KeywordTypeEnum Keyword { get; }
        private int _amount;
        public int Amount
        {
            get => _amount; protected set
            {
                if(_amount != value)
                {
                    _amount = value;
                    OnValueChanged?.Invoke(_amount);
                }

            }
        }
        public StatAbst(bool isPlayer, int amount)
        {
            this.isPlayer = isPlayer;
            Amount = amount;
            if (_updateUIStats == null)
                _updateUIStats += BattleUiManager.Instance.UpdateUiStats;

            _updateUIStats?.Invoke(isPlayer, amount, Keyword);
        }
        ~StatAbst()
        {
            _updateUIStats -= BattleUiManager.Instance.UpdateUiStats;
        }

        public virtual void Add(int amount)
        {
            Amount += amount;
            _updateUIStats?.Invoke(isPlayer, Amount, Keyword);
        }
        public virtual void Reduce(int amount)
        {
            Amount -= amount;
            _updateUIStats?.Invoke(isPlayer, Amount, Keyword);
        }
        public virtual bool HasValue() => Amount > 0;
        public virtual bool HasValue(int amount) => Amount > amount;
        public virtual void Reset(int value = 0)
        {
            Amount = value;
            _updateUIStats?.Invoke(isPlayer, Amount, Keyword);
        }
    }

}