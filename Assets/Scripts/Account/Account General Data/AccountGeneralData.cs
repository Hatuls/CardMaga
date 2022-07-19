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

        [SerializeField] private int _rank = 0;
        [SerializeField] private bool _isFinishedTutorial = false;
        [SerializeField] private AccountType _accountType;

        public int Rank { get => _rank; set => _rank = value; }
        public bool IsFinishedTutorial { get => _isFinishedTutorial; set => _isFinishedTutorial = value; }
        public AccountType AccountType { get => _accountType; set => _accountType = value; }
        public AccountGeneralData()
        {
            _isFinishedTutorial = false;
            _rank = 0;
            _accountType = AccountType.Normal;

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
        [SerializeField] private int _level;
        [SerializeField] private int _exp;

        public int Level { get => _level; set => _level = value; }
        public int Exp { get => _exp; set => _exp = value; }

        public LevelData()
        {
            _level = 1;
            _exp = 0;
        }

        internal bool IsValid()
        => _level >= 0 && _exp >= 0;
    }
}