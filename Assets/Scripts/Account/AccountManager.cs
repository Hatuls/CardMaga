using Account.GeneralData;
using PlayFab;
using PlayFab.ClientModels;
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
        
        [NonSerialized,Sirenix.OdinInspector.ShowInInspector]
        private AccountData _accountData;
        public LoginResult LoginResult { get; private set; }

        private IDisposable _loginDisposable;

        public string SessionTicket => LoginResult.SessionTicket;
        public string EntityID => LoginResult.EntityToken.Entity.Id;

        public AccountData Data => _accountData;

        private void Awake()
        {
            _instance = this;
        }
        
        private void OnError(PlayFabError playFabError)
        {
            _loginDisposable.Dispose();
        }

        public void SendAccountData(ITokenReciever tokenReciever = null)
        {
            PlayFabClientAPI.UpdateUserData(_accountData.GetUpdateRequest(), OnDataRecieved, OnError);
            //.UpdateUserData(_accountData.GetUpdateRequest(), null, OnError);

            if (tokenReciever != null)
                _loginDisposable = tokenReciever.GetToken();
        }

        private void OnDataRecieved(UpdateUserDataResult obj)
        {
            _loginDisposable?.Dispose();
        }

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
                UpdatePlayName("New LeftPlayer");
            }
            else
                _accountData = new AccountData(loginResult.InfoResultPayload.UserData);
            
            _accountData.DisplayName = loginResult.InfoResultPayload.PlayerProfile?.DisplayName ?? "New LeftPlayer";

            UpdateRank(null);
            SendAccountData();
        }
    }

    [System.Serializable]
    public class AccountData
    {
        [SerializeField] private string _displayName = "New LeftPlayer";
        [SerializeField] private AccountGeneralData _accountGeneralData;
        [SerializeField] private CharactersData _charactersData;
        
        [SerializeField] private LevelData _accountLevel;
        [SerializeField] private AccountResources _accountResources;

        [SerializeField] private ArenaData _arenaData;

        private AccountTutorialData _accountTutorialData;

        public AccountTutorialData AccountTutorialData
        {
            get => _accountTutorialData;
        }
        public bool IsFirstTimeUser { get; private set; }
        public string DisplayName { get => _displayName; set => _displayName = value; }

        public AccountGeneralData AccountGeneralData { get => _accountGeneralData; }
        public LevelData AccountLevel { get => _accountLevel;}
        public CharactersData CharactersData { get => _charactersData; }
        public ArenaData ArenaData { get => _arenaData;  }

        //public void AddCharacter(CharacterSO newCharacter)
        //{
        //    if (_characterData.Find(x => x.Id == newCharacter.ID) == null)
        //        _characterData.Add(new Character(newCharacter));
        //}
        
        #region Constructors
        
        public AccountData()
        {
            CreateNewArenaData();
            CreateNewResourcesData();
            CreateNewLevelData();
            CreateNewCharacterData();
            CreateNewGeneralData();
            _accountTutorialData = new AccountTutorialData(0, false);
            IsFirstTimeUser = true;
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

            IsFirstTimeUser = false;
        }

        #endregion

        public UpdateUserDataRequest GetUpdateRequest()
        {
            return new UpdateUserDataRequest()
            {
                Data = new Dictionary<string, string>()
                {
                    { AccountGeneralData.PlayFabKeyName, JsonUtility.ToJson(_accountGeneralData) },
                    { CharactersData.PlayFabKeyName, JsonUtility.ToJson(CharactersData) },
                    { LevelData.PlayFabKeyName, JsonUtility.ToJson(_accountLevel) },
                    { AccountResources.PlayFabKeyName, JsonUtility.ToJson(_accountResources) },
                    { ArenaData.PlayFabKeyName, JsonUtility.ToJson(_arenaData) },
                }, Permission = UserDataPermission.Public
            };
        }
        
        #region Create Account Data
        
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
            Battle.CharacterSO firstCharacter = Factory.GameFactory.Instance.CharacterFactoryHandler.GetCharacterSO(CharacterEnum.Chiara);
            CharactersData.AddCharacter(new Character(firstCharacter));
        }
        private void CreateNewGeneralData()
        {
            _accountGeneralData = new AccountGeneralData();

        }
        
        #endregion
    }
}


