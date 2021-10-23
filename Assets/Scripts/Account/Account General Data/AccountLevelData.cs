﻿using System;
namespace Account.GeneralData
{
    [Serializable]
 
    public class AccountLevelData
    {
        Stat<uint> _exp;
        public Stat<uint> Exp => _exp;

        Stat<byte> _level;
        public Stat<byte> Level => _level;

        public AccountLevelData(uint exp=0, byte lvl=0)
        {
            _level = new Stat<byte>(lvl);
            _exp = new Stat<uint>(exp);
        }
     
    }
}