using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Meta
{
    enum CharacterEnum
    {
        Null = 0,
        Chiara = 1,
        TestSubject007 = 2,
    }

}

namespace Meta.Account
{
    public class AccountManager : MonoBehaviour
    {
        #region Singleton
        private static AccountManager _instance;
        public static AccountManager GetInstance
        {
            get
            {
                if (_instance == null)
                    Debug.LogError("AccountManager is null!");

                return _instance;
            }
        }
        private void Awake()
        {
            _instance = this;
        }
        #endregion
        #region Fields
        AccountCharacters _accountCharacters;
        AccountDecks _accountDecks;
        AccountCards _accountCards;
        AccountCombos _accountCombos;
        AccountSettingsData _accountSettingsData;
        #endregion
        #region Properties
        public AccountCharacters AccountCharacters => _accountCharacters;
        public AccountDecks AccountDecks => _accountDecks;
        public AccountCards AccountCards => _accountCards;
        public AccountCombos AccountCombos => _accountCombos;
        public AccountSettingsData AccountSettingsData => _accountSettingsData;


        #endregion
        #region PrivateMethods
        #endregion
        #region PublicMethods
        public void Init()
        {
            
        }
        public void DownloadDataFromServer()
        {

        }
        #endregion
    }
}
