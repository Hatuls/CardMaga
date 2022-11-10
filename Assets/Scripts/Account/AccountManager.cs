using Account.GeneralData;
using CardMaga.Rewards;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
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

        [NonSerialized, Sirenix.OdinInspector.ShowInInspector]
        private AccountData _accountData;
        public LoginResult LoginResult { get; private set; }

        private IDisposable _loginDisposable;
        private IDisposable _requestFromServerDisposable;

        public string SessionTicket => LoginResult.SessionTicket;
        public string EntityID => LoginResult.EntityToken.Entity.Id;

        public AccountData Data => _accountData;

        private void Awake()
        {
            _instance = this;
        }

        private void OnError(PlayFabError playFabError)
        {
            Debug.LogError($"{playFabError.ErrorMessage}");
            _requestFromServerDisposable?.Dispose();
            _loginDisposable?.Dispose();
        }
        public void RequestAccoundData(ITokenReciever tokenReciever = null)
        {
            _requestFromServerDisposable = tokenReciever?.GetToken();
            PlayFabClientAPI.GetUserData(new GetUserDataRequest
            {
                PlayFabId = LoginResult.PlayFabId,
                Keys = null,

            }, UserDataSucess, OnError);
        }

        private void UserDataSucess(GetUserDataResult obj)
        {
            _accountData = new AccountData(obj.Data);
            _requestFromServerDisposable?.Dispose();
        }

        public void SendAccountData(ITokenReciever tokenReciever = null)
        {

            UpdateDataOnServer();
            if (tokenReciever != null)
                _loginDisposable = tokenReciever.GetToken();
        }

        private void OnDataRecieved(UpdateUserDataResult obj)
        {
            _loginDisposable?.Dispose();
        }
        [Sirenix.OdinInspector.Button()]
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

        public void UpdatePlayName(string name)
        {
            {
                PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
                {
                    DisplayName = name
                }, result =>
                {
                    Debug.Log("The player's display name is now: " + result.DisplayName);
                }, OnError);
            }
        }

        public void OnLogin(LoginResult loginResult)
        {
            LoginResult = loginResult;

            if (LoginResult.NewlyCreated)
            {
                _accountData = new AccountData();
                UpdatePlayName("New User");
            }
            else
                _accountData = new AccountData(loginResult.InfoResultPayload.UserData);

            _accountData.DisplayName = loginResult.InfoResultPayload.PlayerProfile?.DisplayName ?? "New User";

            UpdateRank(null);
            SendAccountData();
        }


        [SerializeField]
        CardsPackRewardFactorySO _factory;
        [ContextMenu("Add Cards")]
        public void AddCards() {
            _factory.GenerateReward().TryRecieveReward(null);


        }
    }

    [System.Serializable]
    public class AccountData
    {
        [SerializeField] private string _displayName = "New User";
        [SerializeField] private AccountGeneralData _accountGeneralData;
        [SerializeField] private CharactersData _charactersData;

        [SerializeField] private LevelData _accountLevel;
        [SerializeField] private AccountResources _accountResources;

        [SerializeField] private ArenaData _arenaData;
        [SerializeField] private AccountCards _accountCards;
        [SerializeField] private AccountCombos _accountCombos;
        private AccountTutorialData _accountTutorialData;

        public AccountTutorialData AccountTutorialData
        {
            get => _accountTutorialData;
        }
        public bool IsFirstTimeUser { get; private set; }
        public string DisplayName { get => _displayName; set => _displayName = value; }
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
            CreateNewArenaData();
            CreateNewResourcesData();
            CreateNewLevelData();
            CreateNewCharacterData();
            CreateNewGeneralData();
            CreateNewCombosData();
            CreateNewCardsData();
            _accountTutorialData = new AccountTutorialData(0, false);
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
            _accountTutorialData = new AccountTutorialData(0, false);//temp_Test
            foreach (var item in data)
                convertedDict.Add(item.Key, item.Value?.Value);

            AssignValues(convertedDict);
        }

        private void AssignValues(Dictionary<string, string> data)
        {
            string result;

            // Account General Data
            if (data.TryGetValue(AccountGeneralData.PlayFabKeyName, out result))
            {
                _accountGeneralData = JsonUtility.FromJson<AccountGeneralData>(result);
                if (_accountGeneralData == null || !_accountGeneralData.IsValid())
                    CreateNewGeneralData();
            }
            else
                CreateNewGeneralData();

            if (data.TryGetValue(ArenaData.PlayFabKeyName, out result))
            {
                _arenaData = JsonUtility.FromJson<ArenaData>(result);
                if (_arenaData == null || !_arenaData.IsValid())
                    CreateNewArenaData();
            }
            else
                CreateNewArenaData();

            // Characters & Deck
            if (data.TryGetValue(CharactersData.PlayFabKeyName, out result))
            {
                _charactersData = JsonUtility.FromJson<CharactersData>(result);

                if (CharactersData == null || !CharactersData.IsValid())
                    CreateNewCharacterData();
            }
            else
                CreateNewCharacterData();

            // Levels
            if (data.TryGetValue(LevelData.PlayFabKeyName, out result))
            {
                _accountLevel = JsonUtility.FromJson<LevelData>(result);
                if (_accountLevel == null || !_accountLevel.IsValid())
                    CreateNewLevelData();
            }
            else
                CreateNewLevelData();

            // Resources
            if (data.TryGetValue(AccountResources.PlayFabKeyName, out result))
            {
                _accountResources = JsonUtility.FromJson<AccountResources>(result);
                if (_accountResources == null || !_accountResources.IsValid())
                    CreateNewResourcesData();
            }
            else
                CreateNewResourcesData();

            // Combos  
            if (data.TryGetValue(AccountCombos.PlayFabKeyName, out result))
            {
                _accountCombos = JsonUtility.FromJson<AccountCombos>(result);
                if (_accountCombos == null || !_accountCombos.IsValid())
                    CreateNewResourcesData();
            }
            else
                CreateNewCombosData();

            // Cards
            if (data.TryGetValue(AccountCards.PlayFabKeyName, out result))
            {
                _accountCards = JsonUtility.FromJson<AccountCards>(result);
                if (_accountCards == null || !_accountCards.IsValid())
                    CreateNewResourcesData();
            }
            else
                CreateNewCardsData();


            IsFirstTimeUser = false;
        }

        #endregion

        public UpdateUserDataRequest GetUpdateRequest()
        {
            string cards = JsonUtility.ToJson(_accountCards);
            return new UpdateUserDataRequest()
            {

                Data = new Dictionary<string, string>()
                {
                    { AccountGeneralData.PlayFabKeyName,     JsonUtility.ToJson(_accountGeneralData) },
                    { CharactersData.PlayFabKeyName,         JsonUtility.ToJson(CharactersData) },
                    { LevelData.PlayFabKeyName,              JsonUtility.ToJson(_accountLevel) },
                    { AccountResources.PlayFabKeyName,       JsonUtility.ToJson(_accountResources) },
                    { ArenaData.PlayFabKeyName,              JsonUtility.ToJson(_arenaData) },
                    { AccountCombos.PlayFabKeyName,          JsonUtility.ToJson(_accountCombos) },
                    { AccountCards.PlayFabKeyName,           cards}
                },
                Permission = UserDataPermission.Public
            };
        }

        #region Create Account Data
        private void CreateNewCardsData()
        {
            _accountCards = new AccountCards();
            foreach (var character in CharactersData.Characters)
                foreach (var deck in character.Deck)
                    foreach (var card in deck.Cards)
                        _accountCards.AddCard(card);
        }
        private void CreateNewCombosData()
        {
            _accountCombos = new AccountCombos();
            foreach (var character in CharactersData.Characters)
                foreach (var deck in character.Deck)
                    foreach (var combo in deck.Combos)
                        _accountCombos.AddCombo(combo);
        }
        private void CreateNewArenaData()
        {
            _arenaData = new ArenaData();
        }
        private void CreateNewResourcesData()
        {
            _accountResources = new AccountResources();
        }
        private void CreateNewLevelData()
        {
            _accountLevel = new LevelData();
        }
        private void CreateNewCharacterData()
        {
            _charactersData = new CharactersData();
            Battle.CharacterSO characterSO = Factory.GameFactory.Instance.CharacterFactoryHandler.GetCharacterSO(CharacterEnum.Chiara);
            var firstCharacter = new Character(characterSO);
            firstCharacter.AddNewDeck(characterSO.Deck, characterSO.Combos);
            CharactersData.AddCharacter(firstCharacter);
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
    [SerializeField]
    private List<CoreID> _cardsIDs = new List<CoreID>();

 
    public IReadOnlyList<CoreID> CardsIDs => _cardsIDs;

    public void AddCard(CoreID cardID) => _cardsIDs.Add(cardID);
    public void RemoveCard(CoreID cardID) => _cardsIDs.Remove(cardID);
    internal bool IsValid() => true;


}
[Serializable]
public class AccountCombos
{
    public const string PlayFabKeyName = "Combos";
    [SerializeField]
    private List<ComboCore> _combosIDs = new List<ComboCore>();
    
    public IReadOnlyList<ComboCore> CombosIDs => _combosIDs;

    public void AddCombo(ComboCore cardID) => _combosIDs.Add(cardID);
    public void RemoveCombo(ComboCore cardID) => _combosIDs.Remove(cardID);
    internal bool IsValid()
    {
        return true;
    }
}