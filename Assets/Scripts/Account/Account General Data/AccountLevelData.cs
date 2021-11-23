using System;
using UnityEngine;

namespace Account.GeneralData
{
    [Serializable]
    public class AccountLevelData : ILoadFirstTime
    {
        [SerializeField]
        EXPStat _exp;
        [SerializeField]
        MaxEXPStat _maxEXP;
        public UintStat MaxEXP => _maxEXP;
        public UintStat Exp => _exp;

        [SerializeField]
        LevelStat _level;
        public ByteStat Level => _level;
        public AccountLevelData()
        {

        }
        public AccountLevelData(uint exp = 0, uint maxExp = 10, byte lvl = 1)
        {
            _maxEXP = new MaxEXPStat(maxExp);
            _level = new LevelStat(lvl, _maxEXP);
            _exp = new EXPStat(exp, _level, _maxEXP);
        }

        public void NewLoad()
        {
            _maxEXP = new MaxEXPStat(10);
            _level = new LevelStat(1, _maxEXP);
            _exp = new EXPStat(0, _level, _maxEXP);
        }
    }
    [Serializable]
    public class EXPStat : UintStat
    {
        public static Action<int, int> OnGainEXP;
        [SerializeField]
        ByteStat _level;
        [SerializeField]
        UintStat _maxExp;

        public override bool AddValue(uint value)
        {

            base.AddValue(value);
            if (Value >= _maxExp.Value)
            {
                _value = 0;
                _level.AddValue(1);
            }
            OnGainEXP?.Invoke((int)_value, (int)_maxExp.Value);
            return true;
        }
        public EXPStat(uint val, ByteStat level, UintStat maxExp) : base(val)
        {
            _level = level;
            _maxExp = maxExp;
        }
    }
    [Serializable]
    public class LevelStat : ByteStat
    {
        public static Action<int> OnLevelUp;
        [SerializeField]
        private UintStat _maxExp;
        public override bool AddValue(byte value)
        {
            OnLevelUp?.Invoke(_value + value);
            _maxExp.AddValue((uint)(_value + value) * 10);
            return base.AddValue(value);
        }
        public LevelStat(byte val, UintStat maxExp) : base(val)
        {
            this._maxExp = maxExp;
        }
    }
}