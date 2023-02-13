using System;
using UnityEngine;
namespace Account.GeneralData
{

    [Serializable]
    public enum AccountType
    {
        Normal = 0,
        Payed = 1,
        Tester = 2,
        Admin = 3,
    }

    [Serializable]
    public class AccountGeneralData
    {
        [NonSerialized]
        public const string PlayFabKeyName = "GeneralData";
        public int Rank;
        public AccountType AccountType;
        public int ImageID;
        public AccountGeneralData()
        {
            Rank = 0;
            AccountType = AccountType.Normal;
        }

        internal bool IsValid()
        {
            return true;
        }
    }
    [Serializable]
    public class LevelData
    {
        [NonSerialized]
        public const string PlayFabKeyName = "LevelData";


        public int Level;
        public int Exp;

        public LevelData()
        {
            Level = 1;
            Exp = 0;
        }

 

        public void AddEXP(int amount)
        {
            Exp += amount;
        }

        internal bool IsValid()
        => Level >= 0 && Exp >= 0;
    }
}