using System;
using Account;
using UnityEngine;

namespace CardMaga.MetaData.AccoutData
{
    public class AccountDataAccess : MonoBehaviour // need to remove mono
    {
        [NonSerialized]
        private AccountData _accountData;

        private MetaAccountData _metaAccountData;

        public MetaAccountData AccountData => _metaAccountData;

        private void Start()
        {
            _metaAccountData = new MetaAccountData(AccountManager.Instance.Data);//plaster!!!!! need to not by mono and get the data from AccountDataAccess
        }

        /*public AccountDataAccess(AccountData accountData)
        {
            _metaAccountData = new MetaAccountData(accountData);

            _accountData = accountData;
        }*/
    }
}