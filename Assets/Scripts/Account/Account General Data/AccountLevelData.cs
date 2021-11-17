using System;
using UnityEngine;
namespace Account.GeneralData
{
    [Serializable]
 
    public class AccountLevelData : ILoadFirstTime
    {
     [SerializeField]
        UintStat _exp;
        public UintStat Exp => _exp;

        [SerializeField]
        ByteStat _level;
        public ByteStat Level => _level;
        public AccountLevelData()
        {

        }
        public AccountLevelData(uint exp=0, byte lvl=1)
        {

            _level = new ByteStat(lvl);
            _exp = new UintStat(exp);
        }

        public void NewLoad()
        {
            _level = new ByteStat(1);
            _exp = new UintStat(0);
        }
    }
}