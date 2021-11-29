using System;
using System.Threading.Tasks;
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
        public UshortStat Level => _level;
        public AccountLevelData()
        {

        }
        public AccountLevelData(uint exp = 0, uint maxExp = 10, ushort lvl = 1)
        {
            _maxEXP = new MaxEXPStat(maxExp);
            _level = new LevelStat(lvl, _maxEXP);
            _exp = new EXPStat(exp, _level, _maxEXP);
        }

        public async Task NewLoad()
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
        UshortStat _level;
        [SerializeField]
        UintStat _maxExp;

        public override bool AddValue(uint value)
        {
            if (value >= 0)
            {
                base.AddValue(value);
                if (Value >= _maxExp.Value)
                {
                    _level.AddValue(1);
                    AddValue(value - _maxExp.Value);
                }
                OnGainEXP?.Invoke((int)_value, (int)_maxExp.Value);
            }
            return true;
        }
        public EXPStat(uint val, UshortStat level, UintStat maxExp) : base(val)
        {
            _level = level;
            _maxExp = maxExp;
        }
    }
    [Serializable]
    public class LevelStat : UshortStat
    {
        public static Action<int> OnLevelUp;
        [SerializeField]
        private UintStat _maxExp;
        public override bool AddValue(ushort value)
        {
            OnLevelUp?.Invoke(_value + value);
            _maxExp.AddValue((uint)(_value + value) * 10);
            return base.AddValue(value);
        }
        public LevelStat(ushort val, UintStat maxExp) : base(val)
        {
            this._maxExp = maxExp;
        }
    }
}