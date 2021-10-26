using UnityEngine;
using Account.GeneralData;

public enum CharacterEnum
{
    Enemy = 0,
    Chiara = 1,
    TestSubject007 = 2,
}

namespace Account
{

    public class AccountManager : MonoBehaviour
    {
        #region Singleton
        private static AccountManager _instance;
        public static AccountManager Instance
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
            Init();
        }
        #endregion
        #region Fields
        [SerializeField]
        AccountCharacters _accountCharacters;
        [SerializeField]
        AccountCards _accountCards;
        [SerializeField]
        AccountCombos _accountCombos;
        [SerializeField]
        AccountSettingsData _accountSettingsData;
 
        #endregion
        #region Properties
        public AccountCharacters AccountCharacters => _accountCharacters;
        public AccountCards AccountCards => _accountCards;
        public AccountCombos AccountCombos => _accountCombos;
        public AccountSettingsData AccountSettingsData => _accountSettingsData;

        #endregion
        #region Private Methods
        #endregion
        #region Public Methods
        public void Init()
        {
            _accountCharacters = new AccountCharacters();
            _accountCards = new AccountCards();
            _accountCombos = new AccountCombos();
            _accountSettingsData = new AccountSettingsData();
        }
        public void DownloadDataFromServer()
        {
        }
        #endregion
    }
}
