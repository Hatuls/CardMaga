using System;
using UnityEngine;
namespace Account.GeneralData
{
    [Serializable]
 
    public class AccountLevelData
    {
        Stat<uint> _exp;
        public Stat<uint> Exp => _exp;

        Stat<byte> _level;
        public Stat<byte> Level => _level;

        public AccountLevelData(uint exp=0, byte lvl=1)
        {
            Debug.Log("Starting Account Level Data");
            _level = new ByteStat(lvl);
            _exp = new UintStat(exp);
        }
     
    }
}