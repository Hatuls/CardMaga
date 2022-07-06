using Battle.UI;
using System;

namespace Characters.Stats
{
    public abstract class StatAbst
    {
        public static Action<bool, int, Keywords.KeywordTypeEnum> _updateUIStats;
        protected bool isPlayer;
        public abstract Keywords.KeywordTypeEnum Keyword { get; }
        public int Amount { get; protected set; }
        public StatAbst(bool isPlayer, int amount)
        {
            this.isPlayer = isPlayer;
            Amount = amount;
            if (_updateUIStats == null)
                _updateUIStats += BattleUiManager.Instance.UpdateUiStats;

            _updateUIStats?.Invoke(isPlayer, amount,Keyword);
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