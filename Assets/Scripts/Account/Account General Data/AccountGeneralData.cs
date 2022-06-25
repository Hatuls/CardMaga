using System;

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
        public const string PlayFabKeyName = "GeneralData";
        private int _rank = 0;
        private bool _isFinishedTutorial = false;
        private AccountType _accountType;


        public int Rank { get => _rank; set => _rank = value; }
        public bool IsFinishedTutorial { get => _isFinishedTutorial; set => _isFinishedTutorial = value; }
        public AccountType AccountType { get => _accountType; set => _accountType = value; }

    }
    [Serializable]
    public class LevelData
    {
        private int _level;
        private int _exp;

        public int Level { get => _level; set => _level = value; }
        public int Exp { get => _exp; set => _exp = value; }
    }
}