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
        public MaxEXPStat MaxEXP => _maxEXP;
        public EXPStat Exp => _exp;

        [SerializeField]
        LevelStat _level;
        public LevelStat Level => _level;
        public AccountLevelData()
        {
            _maxEXP = new MaxEXPStat(10);
            _level = new LevelStat((ushort)DefaultVersion._gameVersion.Level, _maxEXP);
            _exp = new EXPStat((ushort)DefaultVersion._gameVersion.EXP, _level, _maxEXP);
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
            _level = new LevelStat((ushort)DefaultVersion._gameVersion.Level, _maxEXP);
            _exp = new EXPStat((ushort)DefaultVersion._gameVersion.EXP, _level, _maxEXP);
        }

        public bool IsCorrupted()
        {
            bool isCorrupted = false;
            isCorrupted |= _level.Value <= 0;
            isCorrupted |= _maxEXP.Value <= 0;
            return isCorrupted;
        }
    }


    [Serializable]
    public class EXPStat : UintStat
    {
        public static Action<int, int> OnGainEXP;
        [SerializeField]
        LevelStat _level;
        [SerializeField]
        MaxEXPStat _maxExp;

        public override bool AddValue(uint value)
        {
            if (value > 0)
            {
                base.AddValue(value);
                if (Value >= _maxExp.Value)
                {
                    _level.AddValue(1);
                    _value = 0;
                    int remain = (int)value - (int)_maxExp.Value;


                    if (remain > 0)
                        AddValue((uint)remain);
                }
                OnGainEXP?.Invoke((int)_value, (int)_maxExp.Value);
            }
            return true;
        }
        public EXPStat(uint val, LevelStat level, MaxEXPStat maxExp) : base(val)
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
        private MaxEXPStat _maxExp;
        public override bool AddValue(ushort value)
        {
            OnLevelUp?.Invoke(_value + value);
            _maxExp.AddValue((uint)(_value + value) * 10);
            return base.AddValue(value);
        }
        public LevelStat(ushort val, MaxEXPStat maxExp) : base(val)
        {
            this._maxExp = maxExp;
        }
    }
}