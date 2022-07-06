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
        public AccountLevelData(int exp = 0, int maxExp = 10, int lvl = 1)
        {
            _maxEXP = new MaxEXPStat(maxExp);
            _level = new LevelStat(lvl, _maxEXP);
            _exp = new EXPStat(exp, _level, _maxEXP);
        }

        public void NewLoad()
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
    public class EXPStat : IntStat
    {
        public static Action<int, int> OnGainEXP;
        [SerializeField]
        LevelStat _level;
        [SerializeField]
        MaxEXPStat _maxExp;

        public override bool AddValue(int value)
        {
            if (value > 0)
            {
                base.AddValue(value);
                if (Value >= _maxExp.Value)
                {
                    _level.AddValue(1);
                    _value = 0;
                    int remain = value - _maxExp.Value;


                    if (remain > 0)
                        AddValue(remain);
                }
                OnGainEXP?.Invoke(_value, _maxExp.Value);
            }
            return true;
        }
        public EXPStat(int val, LevelStat level, MaxEXPStat maxExp) : base(val)
        {
            _level = level;
            _maxExp = maxExp;
        }
    }
    [Serializable]
    public class LevelStat : IntStat
    {
        public static Action<int> OnLevelUp;
        [SerializeField]
        private MaxEXPStat _maxExp;
        public override bool AddValue(int value)
        {
            OnLevelUp?.Invoke(_value + value);
            _maxExp.AddValue((_value + value) * 10);
            return base.AddValue(value);
        }
        public LevelStat(int val, MaxEXPStat maxExp) : base(val)
        {
            this._maxExp = maxExp;
        }
    }
}