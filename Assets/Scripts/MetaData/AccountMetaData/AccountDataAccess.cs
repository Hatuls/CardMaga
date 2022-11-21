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

        private void Awake()
        {
            _metaAccountData = new MetaAccountData(AccountManager.Instance.Data);//plaster!!!!! need to not by mono and get the data from AccountDataAccess
        }

        public void UpdateDeck(MetaDeckData metaDeckData)
        {
            _metaAccountData.CharacterDatas.CharacterData.UpdateDeck(metaDeckData,0);
        }
    }
}