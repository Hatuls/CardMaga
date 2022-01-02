using Account.GeneralData;
using Battles;
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
        [SerializeField]

        private RewardGift _rewardGift;

        //[HideInInspector]

        [SerializeField]
        private AccountData _accountData;
        [SerializeField]
        bool _needToDoTutorial = true;
        public bool IsDoneTutorial { get => _needToDoTutorial; set => _needToDoTutorial = value; }
        SaveManager.FileStreamType saveType = SaveManager.FileStreamType.FileStream;
        string path = "Account/";

        #endregion
        #region Properties
        public AccountCharacters AccountCharacters => _accountData.AccountCharacters;
        public AccountCards AccountCards => _accountData.AccountCards;
        public AccountCombos AccountCombos => _accountData.AccountCombos;
        public AccountSettingsData AccountSettingsData => _accountData.AccountSettingsData;
        public AccountGeneralData AccountGeneralData => _accountData.AccountGeneralData;
        public BattleData BattleData => _accountData.BattleData;
        public SceneHandler.ScenesEnum CurrentScene => _accountData.CurrentScene;
        public RewardGift RewardGift { get => _rewardGift; }

        #endregion
        #region Private Methods
        private void Start()
        {

            SceneHandler.onFinishLoadingScene += UpdateLastScene;
        }

        public void EnterMainMenu()
        {
            ReturnLoadingScene.GoToScene = _accountData.CurrentScene;
            SceneHandler.LoadScene(SceneHandler.ScenesEnum.MainMenuScene);

        }

        private void UpdateLastScene(SceneHandler.ScenesEnum scene)
        {
            _accountData.CurrentScene = scene;

            SaveAccount();
        }
        #endregion
        #region Public Methods
        public async void Init()
        {
            if (SaveManager.CheckFileExists(AccountData.SaveName, saveType, true, path))
            {
                await LoadAccount();
            }
            else
            {
                await CreateNewAccount();
            }
            RewardLoad();

            OnFinishLoading?.Invoke();

            if (!_needToDoTutorial)
                EnterMainMenu();
            else
                EnterTutorial(); 
        }

        private static void EnterTutorial()
        {
            const string eventName = "start_tutorial";
            UnityAnalyticHandler.SendEvent(eventName);
            FireBaseHandler.SendEvent(eventName);

            SceneHandler.LoadSceneWithNoLoadingScreen(SceneHandler.ScenesEnum.LoreScene);
        }

        private async Task LoadAccount()
        {
            _accountData = SaveManager.Load<AccountData>(AccountData.SaveName, saveType, "txt", true, path);
            Debug.Log("Loading Data From " + saveType);
            Factory.GameFactory.Instance.CardFactoryHandler.RegisterAccountLoadedCardsInstanceID(_accountData.AccountCards.CardList);
            _accountData.AccountSettingsData.Refresh();


            if (PlayerPrefs.HasKey("Tutorial"))
                _needToDoTutorial = bool.Parse(PlayerPrefs.GetString("Tutorial"));
        }

        private void RewardLoad()
        {
            if (SaveManager.CheckFileExists("Reward", saveType))
                _rewardGift = SaveManager.Load<RewardGift>("Reward", saveType);
        }

        public void ResetAccount() => _ = CreateNewAccount();


        #endregion

#if UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
        private void AddDiamonds() => _accountData.AccountGeneralData.AccountResourcesData.Diamonds.AddValue(10);

        [Sirenix.OdinInspector.Button]
        private void AddGold() => _accountData.BattleData.Player.CharacterData.CharacterStats.Gold += 100;

        [Sirenix.OdinInspector.Button]
        private void AddEnergy() => _accountData.AccountGeneralData.AccountEnergyData.Energy.AddValue(10);
        [Sirenix.OdinInspector.Button]
        private void ResetAccountSave() => SaveManager.DeleteFile(AccountData.SaveName, "txt", saveType, path, true);// PlayerPrefs.DeleteKey(AccountData.SaveName);

        [Sirenix.OdinInspector.Button]
        private void AddEXP() => _accountData.AccountGeneralData.AccountLevelData.Exp.AddValue(5);
#endif

        private void OnDisable()
        {
            SaveAccount();
        }


        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                SaveAccount();
        }


        private void OnApplicationQuit()
        {
            SaveAccount();
            SceneHandler.onFinishLoadingScene -= UpdateLastScene;
            Debug.Log("Saving Account Data");
        }
        public void SaveAccount()
        {
            SaveManager.SaveFile(_accountData, AccountData.SaveName, saveType, true, "txt", path);
            SaveManager.SaveFile(RewardGift, "Reward", saveType);

            PlayerPrefs.SetString("Tutorial", string.Concat(_needToDoTutorial));
            PlayerPrefs.Save();
        }

        private async Task CreateNewAccount()
        {
            _accountData = new AccountData();
            await _accountData.NewLoad();
            _needToDoTutorial = true;
            SceneHandler.LoadSceneWithNoLoadingScreen(SceneHandler.ScenesEnum.LoreScene);
        }


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
        public static string SaveName = "AccountData";



        [SerializeField] AccountGeneralData _accountGeneralData;

        [SerializeField] AccountCards _accountCards;

        [SerializeField] AccountCombos _accountCombos;

        [SerializeField] AccountSettingsData _accountSettingsData;

        [SerializeField] AccountCharacters _accountCharacters;

        [SerializeField] BattleData _battleData;

        public AccountCharacters AccountCharacters { get => _accountCharacters; private set => _accountCharacters = value; }
        public AccountCards AccountCards { get => _accountCards; private set => _accountCards = value; }
        public AccountCombos AccountCombos { get => _accountCombos; private set => _accountCombos = value; }
        public AccountSettingsData AccountSettingsData { get => _accountSettingsData; private set => _accountSettingsData = value; }
        public AccountGeneralData AccountGeneralData { get => _accountGeneralData; private set => _accountGeneralData = value; }
        public BattleData BattleData { get => _battleData; set => _battleData = value; }
        public SceneHandler.ScenesEnum CurrentScene { get => _lastScene; set => _lastScene = value; }

        [SerializeField]
        SceneHandler.ScenesEnum _lastScene;

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
            _battleData = new BattleData();
            _battleData.ResetData();
        }
    }
    [System.Serializable]
    public class RewardGift
    {
        public bool NeedToBeRewarded { get => _needToBeRewarded; set => _needToBeRewarded = value; }

        [SerializeField] bool _needToBeRewarded = false;
    }
}
