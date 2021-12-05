using Account.GeneralData;
using System.Threading.Tasks;
using UnityEngine;
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
        Task NewLoad();
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

            }
            else if (_instance != this)
                Destroy(this.gameObject);


            if (_instance == this)
                DontDestroyOnLoad(this.gameObject);
        }

        #endregion

        #region Fields
        [HideInInspector]
        [SerializeField]
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
        public async void Init()
        {
            if (PlayerPrefs.HasKey(AccountData.SaveName))
            {
                _accountData = SaveManager.Load<AccountData>(AccountData.SaveName, SaveManager.FileStreamType.PlayerPref);
                if (_accountData.AccountGeneralData.AccountEnergyData.MaxEnergy.Value == 0)
                {
                    await CreateNewAccount();

                }
                else
                {
                    Debug.Log("Loading Data From PlayerPref");
                    Factory.GameFactory.Instance.CardFactoryHandler.RegisterAccountLoadedCardsInstanceID(_accountData.AccountCards.CardList);
                }
            }
            else
            {
                await CreateNewAccount();
            }

            OnFinishLoading?.Invoke();
        }
        public void ResetAccount() => _ = CreateNewAccount();
        private async Task CreateNewAccount()
        {
            _accountData = new AccountData();
            await _accountData.NewLoad();
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

        [Sirenix.OdinInspector.Button]
        private void AddEXP() => _accountData.AccountGeneralData.AccountLevelData.Exp.AddValue(5);
#endif

        private void OnDisable()
        {
            SaveAccount();
        }
   

        void OnApplicationPause(bool pauseStatus)
        {
             if(pauseStatus)
                SaveAccount();
        }


        private void OnApplicationQuit()
        {
            SaveAccount();
            Debug.Log("Saving Account Data");
        }
        public void SaveAccount() => SaveManager.SaveFile(_accountData, AccountData.SaveName, SaveManager.FileStreamType.PlayerPref);
        private void OnDestroy()
        {
            if (Application.isPlaying)
            {
                SaveAccount();
                Debug.Log("Saving Account Data");
            }
        }

    }

    [System.Serializable]
    public class AccountData : ILoadFirstTime
    {
        public const string SaveName = "AccountData";
        [SerializeField] AccountGeneralData _accountGeneralData;

        [SerializeField] AccountCards _accountCards;

        [SerializeField] AccountCombos _accountCombos;

        [SerializeField] AccountSettingsData _accountSettingsData;

        [SerializeField] AccountCharacters _accountCharacters;

        public AccountCharacters AccountCharacters { get => _accountCharacters; private set => _accountCharacters = value; }
        public AccountCards AccountCards { get => _accountCards; private set => _accountCards = value; }
        public AccountCombos AccountCombos { get => _accountCombos; private set => _accountCombos = value; }
        public AccountSettingsData AccountSettingsData { get => _accountSettingsData; private set => _accountSettingsData = value; }
        public AccountGeneralData AccountGeneralData { get => _accountGeneralData; private set => _accountGeneralData = value; }


        public async Task NewLoad()
        {
            AccountSettingsData = new AccountSettingsData();
            await AccountSettingsData.NewLoad();
            AccountGeneralData = new AccountGeneralData();
            await AccountGeneralData.NewLoad();
            AccountCards = new AccountCards();
            await AccountCards.NewLoad();
            AccountCombos = new AccountCombos();
            await AccountCombos.NewLoad();
            AccountCharacters = new AccountCharacters();
            await AccountCharacters.NewLoad();
        }
    }
}
