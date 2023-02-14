using Account.GeneralData;
using CardMaga.MetaData;
using CardMaga.Rewards;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using ReiTools.TokenMachine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
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
        bool IsCorrupted();
        void NewLoad();
    }

    public class AccountManager : MonoBehaviour
    {
        public static event Action<AccountData> OnDataUpdated;
        public static event Action OnAccountDataAssigned;

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

        #endregion
        [SerializeField, ReadOnly]
        private string _displayName;
        [ShowInInspector]
        private AccountData _accountData;
        [ShowInInspector, ReadOnly]
        private LoginResult loginResult;
        private LevelManager _levelManager;
        [SerializeField]
        private LevelUpRewardsSO _levelUpRewardsSO;
        [SerializeField]
        GiftRewardFactorySO[] _startingGift;
        [SerializeField]
        GiftRewardFactorySO[] _additionToStartGift;


        private IDisposable _loginDisposable;
        private IDisposable _requestFromServerDisposable;
        public LoginResult LoginResult { get => loginResult; private set => loginResult = value; }
        public string DisplayName { get => _displayName; set => _displayName = value; }
        public string SessionTicket => LoginResult.SessionTicket;
        public string EntityID => LoginResult.EntityToken.Entity.Id;
        public string PlayfabID => loginResult.PlayFabId;
        public LevelManager LevelManager => _levelManager;
        public AccountData Data => _accountData;

        private void Awake()
        {
            if (_instance != null)
                Destroy(this.gameObject);
            _instance = this;
     
        }
        public void ResetAccount(ITokenReceiver tokenReciever)
        {
            _accountData = new AccountData(true);
            ReceiveStartingData();
            UpdateAccount();
            void UpdateAccount()
            {
                UpdateRank(null);
                SendAccountData(tokenReciever);
                OnAccountDataAssigned?.Invoke();
            }
        }
        private void OnError(PlayFabError playFabError)
        {
            Debug.LogError($"{playFabError.ErrorMessage}");
            _requestFromServerDisposable?.Dispose();
            _loginDisposable?.Dispose();
        }
        public void RequestAccoundData(ITokenReceiver tokenReciever = null)
        {
            _requestFromServerDisposable = tokenReciever?.GetToken();
            PlayFabClientAPI.GetUserData(new GetUserDataRequest
            {
                PlayFabId = LoginResult.PlayFabId,
                Keys = null,

            }, UserDataSuccess, OnError);
        }

        private void UserDataSuccess(GetUserDataResult obj)
        {
            _accountData = new AccountData(obj.Data);

            _requestFromServerDisposable?.Dispose();
        }

        public void SendAccountData(ITokenReceiver tokenReciever = null)
        {
            UpdateDataOnServer();
            if (tokenReciever != null)
                _loginDisposable = tokenReciever.GetToken();
        }

        private void OnDataRecieved(UpdateUserDataResult obj)
        {
            OnDataUpdated?.Invoke(Data);
            _loginDisposable?.Dispose();
        }
        [Button()]
        public void RequestDataFromServer() => RequestAccoundData(null);
        [Button()]
        public void UpdateDataOnServer()
        => PlayFabClientAPI.UpdateUserData(_accountData.GetUpdateRequest(), OnDataRecieved, OnError);

        public void UpdateRank(Action<UpdatePlayerStatisticsResult> OnCompletedSuccessfully)
        {
            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>()
                {
                    new StatisticUpdate
                    {
                        StatisticName = "Rank",
                        Value = _accountData.AccountGeneralData.Rank
                    }
                }
            };

            PlayFabClientAPI.UpdatePlayerStatistics(request, OnCompletedSuccessfully, OnError);
        }


        public void OnLogin(LoginResult loginResult)
        {
            LoginResult = loginResult;

            if (LoginResult.NewlyCreated)
            {
                _accountData = new AccountData(true);
                ReceiveStartingData();
            }
            else
            {
                _accountData = new AccountData(loginResult.InfoResultPayload.UserData);
            }

            _levelManager = new LevelManager(_levelUpRewardsSO,_accountData.AccountLevel); 

            UpdateAccount();

            void UpdateAccount()
            {
                SendAccountData();
                UpdateRank(null);
                OnAccountDataAssigned?.Invoke();
            }


        }

        private void ReceiveStartingData()
        {
            ReceiveAllCombos();

            for (int i = 0; i < _startingGift.Length; i++)
            {
                var reward = _startingGift[i].GenerateReward();
                reward.AddToDevicesData();
            }
            FirstGift();

            void FirstGift()
            {
                var character = _accountData.CharactersData.GetMainCharacter();
                var chiara = Factory.GameFactory.Instance.CharacterFactoryHandler.GetCharacterSO(CharacterEnum.Chiara);

                IReadOnlyList<CoreID> cardsIDs = _accountData.AllCards.CardsIDs;
                CardInstance[] cards = new CardInstance[cardsIDs.Count];
                var cardFactory = Factory.GameFactory.Instance.CardFactoryHandler;

                for (int i = 0; i < cards.Length; i++)
                    cards[i] = cardFactory.CreateCardInstance(cardsIDs[i]);

                List<ComboCore> combosIDs = _accountData.AllCombos.CombosIDs;

                const int BarrierCombo = 2;
                const int JabCrossCombo = 3;
                const int PushKickCombo = 4;

                ComboCore[] _startingCombos = new ComboCore[]
                {
                    combosIDs.First(x => x.CoreID == BarrierCombo),
                    combosIDs.First(x => x.CoreID == JabCrossCombo),
                    combosIDs.First(x => x.CoreID == PushKickCombo),
                };

                character.AddNewDeck(cards, _startingCombos);


                Array.ForEach(_additionToStartGift, x => x.GenerateReward().AddToDevicesData());

            }
            void ReceiveAllCombos()
            {
                var comboFactory = Factory.GameFactory.Instance.ComboFactoryHandler;
                var allCombos = comboFactory.ComboCollection;
                foreach (var combo in allCombos.AllCombos)
                    _accountData.AllCombos.AddCombo(new ComboCore(combo, 0));
            }

        }


#if UNITY_EDITOR
        [Header("Editor:")]
        [SerializeField]
        CardsPackRewardFactorySO _factory;


        [ContextMenu("Add Cards")]
        public void AddCards()
        {
            _factory.GenerateReward().TryRecieveReward(null);


        }

#endif
    }

    [Serializable]
    public class AccountData
    {

        [SerializeField] private AccountGeneralData _accountGeneralData;
        [SerializeField] private CharactersData _charactersData;

        [SerializeField] private LevelData _accountLevel;
        [SerializeField] private AccountResources _accountResources;

        [SerializeField] private ArenaData _arenaData;
        [SerializeField] private AccountCards _accountCards;
        [SerializeField] private AccountCombos _accountCombos;
        [SerializeField] private AccountTutorialData _accountTutorialData;

        public AccountTutorialData AccountTutorialData => _accountTutorialData;

        public bool IsFirstTimeUser { get; private set; }

        public AccountCombos AllCombos => _accountCombos;
        public AccountCards AllCards => _accountCards;
        public AccountGeneralData AccountGeneralData { get => _accountGeneralData; }
        public LevelData AccountLevel { get => _accountLevel; }
        public CharactersData CharactersData { get => _charactersData; }
        public ArenaData ArenaData { get => _arenaData; }
        public AccountResources AccountResources { get => _accountResources; }


        #region Constructors

        public AccountData()
        {

        }
        public AccountData(bool createNewAccount)
        {
            if (!createNewAccount)
                return;
            CreateNewArenaData();
            CreateNewResourcesData();
            CreateNewLevelData();
            CreateNewCharacterData();
            CreateNewGeneralData();
            CreateNewCombosData();
            CreateNewCardsData();
            CreateNewTutorialData();
            IsFirstTimeUser = true;
            Debug.Log("Creating new Account");
        }



        public AccountData(Dictionary<string, string> data)
        {
            AssignValues(data);
        }

        public AccountData(Dictionary<string, UserDataRecord> data)
        {
            var convertedDict = new Dictionary<string, string>();

            foreach (var item in data)
                convertedDict.Add(item.Key, item.Value?.Value.ToString());

            AssignValues(convertedDict);
        }

        private void AssignValues(Dictionary<string, string> data)
        {
            string result;
            //   var jsonHandler = PlayFab.PluginManager.GetPlugin<ISerializerPlugin>(PluginContract.PlayFab_Serializer);
            // Account General Data
            if (data.TryGetValue(AccountGeneralData.PlayFabKeyName, out result))
            {
                _accountGeneralData = JsonConvert.DeserializeObject<AccountGeneralData>(result);
                if (_accountGeneralData == null || !_accountGeneralData.IsValid())
                    CreateNewGeneralData();
            }
            else
                CreateNewGeneralData();

            if (data.TryGetValue(ArenaData.PlayFabKeyName, out result))
            {

                _arenaData = JsonConvert.DeserializeObject<ArenaData>(result);
                if (_arenaData == null || !_arenaData.IsValid())
                {
                    _arenaData = PlayFabSimpleJson.DeserializeObject<ArenaData>(result);
                    if (_arenaData == null || !_arenaData.IsValid())
                        CreateNewArenaData();
                }
            }
            else
                CreateNewArenaData();

            // Characters & Deck
            if (data.TryGetValue(CharactersData.PlayFabKeyName, out result))
            {

                _charactersData = JsonConvert.DeserializeObject(result) as CharactersData;

                //
                if (CharactersData == null || !CharactersData.IsValid())
                {
                    _charactersData = PlayFabSimpleJson.DeserializeObject<CharactersData>(result);

                    if (CharactersData == null || !CharactersData.IsValid())
                        CreateNewCharacterData();
                }
            }
            else
                CreateNewCharacterData();

            // Levels
            if (data.TryGetValue(LevelData.PlayFabKeyName, out result))
            {
                _accountLevel = JsonConvert.DeserializeObject<LevelData>(result);
                if (_accountLevel == null || !_accountLevel.IsValid())
                {

                    _accountLevel = PlayFabSimpleJson.DeserializeObject<LevelData>(result);
                    if (_accountLevel == null || !_accountLevel.IsValid())
                        CreateNewLevelData();
                }
            }
            else
                CreateNewLevelData();

            // Resources
            if (data.TryGetValue(AccountResources.PlayFabKeyName, out result))
            {
                _accountResources = JsonConvert.DeserializeObject<AccountResources>(result);
                if (_accountResources == null || !_accountResources.IsValid())
                {
                    _accountResources = PlayFabSimpleJson.DeserializeObject<AccountResources>(result);
                    if (_accountResources == null || !_accountResources.IsValid())
                        CreateNewResourcesData();
                }
            }
            else
                CreateNewResourcesData();

            // Combos  
            if (data.TryGetValue(AccountCombos.PlayFabKeyName, out result))
            {
                _accountCombos = JsonConvert.DeserializeObject<AccountCombos>(result);
                if (_accountCombos == null || !_accountCombos.IsValid())
                {

                    _accountCombos = PlayFabSimpleJson.DeserializeObject<AccountCombos>(result);
                    if (_accountCombos == null || !_accountCombos.IsValid())
                        CreateNewResourcesData();
                }
            }
            else
                CreateNewCombosData();

            // Cards
            if (data.TryGetValue(AccountCards.PlayFabKeyName, out result))
            {
                _accountCards = JsonConvert.DeserializeObject<AccountCards>(result);
                if (_accountCards == null || !_accountCards.IsValid())
                {
                    _accountCards = PlayFabSimpleJson.DeserializeObject<AccountCards>(result);
                    if (_accountCards == null || !_accountCards.IsValid())
                        CreateNewResourcesData();
                }
            }
            else
                CreateNewCardsData();

            // Tutorial
            if (data.TryGetValue(AccountTutorialData.PlayFabKeyName, out result))
            {
                _accountTutorialData = JsonConvert.DeserializeObject<AccountTutorialData>(result);
                if (_accountTutorialData == null)
                {
                    _accountTutorialData = PlayFabSimpleJson.DeserializeObject<AccountTutorialData>(result);
                    if (_accountTutorialData == null)
                        CreateNewTutorialData();
                }
            }
            else
                CreateNewTutorialData();


            IsFirstTimeUser = false;
        }

        #endregion

        public UpdateUserDataRequest GetUpdateRequest()
        {

            return new UpdateUserDataRequest()
            {
                AuthenticationContext = AccountManager.Instance.LoginResult.AuthenticationContext,

                Data = new Dictionary<string, string>()
                {
                    { AccountGeneralData.PlayFabKeyName,     JsonConvert.SerializeObject(_accountGeneralData) },
                    { CharactersData.PlayFabKeyName,         JsonConvert.SerializeObject(CharactersData) },
                    { LevelData.PlayFabKeyName,              JsonConvert.SerializeObject(_accountLevel) },
                    { AccountResources.PlayFabKeyName,       JsonConvert.SerializeObject(_accountResources) },
                    { ArenaData.PlayFabKeyName,              JsonConvert.SerializeObject(_arenaData) },
                    { AccountCombos.PlayFabKeyName,          JsonConvert.SerializeObject(_accountCombos) },
                    { AccountCards.PlayFabKeyName,           JsonConvert.SerializeObject(_accountCards)},
                    { AccountTutorialData.PlayFabKeyName,    JsonConvert.SerializeObject(_accountTutorialData)}
                },
                Permission = UserDataPermission.Public
            };
        }

        #region Create Account Data
        private void CreateNewCardsData()
        {
            _accountCards = new AccountCards();
            //if (!_accountCards.IsValid())
            //    throw new Exception("Could not Generate Account Cards");
        }

        private void CreateNewCombosData()
        {
            _accountCombos = new AccountCombos();
            //if (!_accountCombos.IsValid())
            //    throw new Exception("Could not Generate Account Combos");
        }
        private void CreateNewArenaData()
        {
            _arenaData = new ArenaData();
            //if (!_arenaData.IsValid())
            //    throw new Exception("Could not Generate Arena Data");
        }
        private void CreateNewResourcesData()
        {
            _accountResources = new AccountResources();
            //if(!_accountResources.IsValid())
            //    throw new Exception("Could not Generate Account Resources");
        }
        private void CreateNewLevelData()
        {
            _accountLevel = new LevelData();
            //if(!_accountLevel.IsValid())
            //    throw new Exception("Could not Generate Account Level");

        }
        private void CreateNewCharacterData()
        {
            _charactersData = new CharactersData();
            //Battle.CharacterSO characterSO = Factory.GameFactory.Instance.CharacterFactoryHandler.GetCharacterSO(CharacterEnum.Chiara);
            //var firstCharacter = new Character(characterSO);

            //firstCharacter.AddNewDeck(characterSO.Deck, characterSO.Combos);
            //CharactersData.AddCharacter(firstCharacter);
        }
        private void CreateNewTutorialData()
        {
            _accountTutorialData = new AccountTutorialData();
            _accountTutorialData.Reset();
        }
        private void CreateNewGeneralData()
        {
            _accountGeneralData = new AccountGeneralData();

        }

        #endregion
    }
}

[Serializable]
public class AccountCards
{
    public const string PlayFabKeyName = "Cards";
    public List<CoreID> CardsIDs = new List<CoreID>();

    public void AddCard(CoreID cardID) => CardsIDs.Add(cardID);
    public void RemoveCard(CoreID cardID) => CardsIDs.Remove(cardID);
    internal bool IsValid() => CardsIDs.Count > 0;
}
[Serializable]
public class AccountCombos
{
    public const string PlayFabKeyName = "Combos";

    public List<ComboCore> CombosIDs = new List<ComboCore>();
    public void AddCombo(ComboCore cardID) => CombosIDs.Add(cardID);
    public void RemoveCombo(ComboCore cardID) => CombosIDs.Remove(cardID);
    internal bool IsValid()
    {
        return CombosIDs.Count > 0;
    }
}
