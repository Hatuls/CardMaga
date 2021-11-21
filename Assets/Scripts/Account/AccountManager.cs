using UnityEngine;
using Account.GeneralData;
using UnityEngine.Events;

[System.Serializable]
public enum CharacterEnum
{
    Enemy = 0,
    Chiara = 1,
    TestSubject007 = 2,
}

namespace Account
{
    public interface ILoadFirstTime
    {
        void NewLoad();
    }
    public class AccountManager : MonoBehaviour
    {
        [SerializeField] UnityEvent OnFinishLoading;
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
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
            }else if (_instance != this)
                Destroy(this.gameObject);

            
         
        }
     
        #endregion

        #region Fields
  
        private AccountData _accountData;

        #endregion
        #region Properties
        public AccountCharacters AccountCharacters => _accountData.AccountCharacters;
        public AccountCards AccountCards => _accountData.AccountCards;
        public AccountCombos AccountCombos => _accountData.AccountCombos;
        public AccountSettingsData AccountSettingsData => _accountData.AccountSettingsData;
        public AccountGeneralData AccountGeneralData => _accountData.AccountGeneralData;

        #endregion
        #region Private Methods
        #endregion
        #region Public Methods
        public void Init()
        {
            if (PlayerPrefs.HasKey(AccountData.SaveName))
            {
                _accountData = SaveManager.Load<AccountData>(AccountData.SaveName, SaveManager.FileStreamType.PlayerPref);
                Debug.Log("Loading Data From PlayerPref");
            }
            else
            {
                _accountData = new AccountData();
                _accountData.NewLoad();
            }

            OnFinishLoading?.Invoke();
        }
        public void DownloadDataFromServer()
        {
        }
        #endregion

        #if UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
        private void AddDiamonds() => _accountData.AccountGeneralData.AccountResourcesData.Diamonds.AddValue(10);

        [Sirenix.OdinInspector.Button]
        private void AddGold() => _accountData.AccountGeneralData.AccountResourcesData.Gold.AddValue(10);

        [Sirenix.OdinInspector.Button]
        private void AddEnergy() => _accountData.AccountGeneralData.AccountEnergyData.Energy.AddValue(10);
        [Sirenix.OdinInspector.Button]
        private void ResetAccountSave() => PlayerPrefs.DeleteKey(AccountData.SaveName);
#endif

        private void OnDestroy()
        {

                SaveManager.SaveFile(_accountData, AccountData.SaveName, SaveManager.FileStreamType.PlayerPref);
            Debug.Log("Saving Account Data");
        }
        private void OnDisable()
        {
      
                SaveManager.SaveFile(_accountData, AccountData.SaveName, SaveManager.FileStreamType.PlayerPref);
            Debug.Log("Saving Account Data");
        }
    }

    [System.Serializable]
    public class AccountData : ILoadFirstTime
    {
        public const string SaveName = "AccountData";
        [SerializeField]  AccountGeneralData _accountGeneralData;

        [SerializeField]  AccountCards _accountCards;

        [SerializeField]  AccountCombos _accountCombos;

        [SerializeField] AccountSettingsData _accountSettingsData;

        [SerializeField] AccountCharacters _accountCharacters; 

        public AccountCharacters AccountCharacters { get => _accountCharacters;private set => _accountCharacters = value; }
        public AccountCards AccountCards { get => _accountCards; private set => _accountCards = value; }
        public AccountCombos AccountCombos { get => _accountCombos; private set => _accountCombos = value; }
        public AccountSettingsData AccountSettingsData { get => _accountSettingsData; private set => _accountSettingsData = value; }
        public AccountGeneralData AccountGeneralData { get => _accountGeneralData; private set => _accountGeneralData = value; }
        public AccountData()
        {
            Debug.Log("Creating New Account Data");
        }


        public void NewLoad()
        {
            AccountCharacters = new AccountCharacters();
            AccountCharacters.NewLoad();
            AccountCards = new AccountCards();
            AccountCards.NewLoad();
             AccountCombos = new AccountCombos();
            AccountCombos.NewLoad();
            AccountSettingsData = new AccountSettingsData();
            AccountSettingsData.NewLoad();
            AccountGeneralData = new AccountGeneralData();
            AccountGeneralData.NewLoad();
        }
    }
}
