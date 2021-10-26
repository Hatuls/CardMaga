﻿using System;
namespace Account.GeneralData
{
    [Serializable]
    public class AccountInfoData
    {
        public AccountInfoData(DateTime? lastLogin, string accountName = "User",uint accountID = 0)
        {
            _lastLogin =new DateTimeStat((lastLogin == null) ?DateTime.Now : lastLogin.Value);
            _accountID = accountID;
            _accountName = accountName;
            _timePlayerToday = new TimeSpanStat(TimeSpan.Zero);
        }

        private Stat<DateTime> _lastLogin;
        public Stat<DateTime> LastLogin { get => _lastLogin; private set => _lastLogin = value; }


        private Stat<TimeSpan> _timePlayerToday;
        public Stat<TimeSpan> TimePlayerToday { get => _timePlayerToday; private set => _timePlayerToday = value; }


        private string _accountName;
        public string AccountName { get => _accountName;private set => _accountName = value; }

        
        
        private uint _accountID;
        public uint AccountID { get => _accountID;private set => _accountID = value; }

    }
}