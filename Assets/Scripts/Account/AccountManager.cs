using Account.GeneralData;
using PlayFab;
using PlayFab.ClientModels;
using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
using UnityEngine;

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
        [NonSerialized]
        private AccountData _accountData;
        public LoginResult LoginResult { get; private set; }
 
        private IDisposable _loginDisposable;

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

            if (tokenReciever != null)
                _loginDisposable = tokenReciever.GetToken();
        }

        private void OnDataRecieved(UpdateUserDataResult obj)
        {
            _loginDisposable.Dispose();
        }

    
        public void OnLogin(LoginResult loginResult)
        {
            LoginResult = loginResult;
            _accountData = new AccountData(loginResult.InfoResultPayload.UserData);
            _accountData.DisplayName = loginResult.InfoResultPayload.PlayerProfile?.DisplayName ?? "New Player";
        }
    }

    [System.Serializable]
    public class AccountData
    {
        [SerializeField] private string _displayName = "New Player";
        [SerializeField] private AccountGeneralData _accountGeneralData;
        [SerializeField] private CharactersData _charactersData;

        [SerializeField] private LevelData _accountLevel;
        [SerializeField] private AccountResources _accountResources;

        [SerializeField] private ArenaData _arenaData;

        public bool IsFirstTimeUser { get; private set; }
        public string DisplayName { get => _displayName; set => _displayName = value; }
        public AccountGeneralData AccountGeneralData { get => _accountGeneralData;}

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
            IsFirstTimeUser = true;
        }

        public AccountData(Dictionary<string, UserDataRecord> data)
        {
            UserDataRecord result;

            // Account General Data
            if (data.TryGetValue(AccountGeneralData.PlayFabKeyName, out result))
            {
                _accountGeneralData = JsonUtility.FromJson<AccountGeneralData>(result.Value);
                if(_accountGeneralData == null || !_accountGeneralData.IsValid())
                CreateNewGeneralData();
            }
            else
                CreateNewGeneralData();

            if (data.TryGetValue(ArenaData.PlayFabKeyName, out result))
            {
                _arenaData = JsonUtility.FromJson<ArenaData>(result.Value);
                if(_arenaData == null || !_arenaData.IsValid())
                CreateNewArenaData();
            }
            else
                CreateNewArenaData();

            // Characters & Deck
            if (data.TryGetValue(CharactersData.PlayFabKeyName, out result))
            {
                _charactersData = JsonUtility.FromJson<CharactersData>(result.Value);

                if (_charactersData  == null || !_charactersData.IsValid())
                CreateNewCharacterData();
            }
            else
                CreateNewCharacterData();

            // Levels
            if (data.TryGetValue(LevelData.PlayFabKeyName, out result))
            {
                _accountLevel = JsonUtility.FromJson<LevelData>(result.Value);
                if(_accountLevel == null || !_accountLevel.IsValid())
                    CreateNewLevelData();
            }
            else
                CreateNewLevelData();

            // Resources
            if (data.TryGetValue(AccountResources.PlayFabKeyName, out result))
            {
                _accountResources = JsonUtility.FromJson<AccountResources>(result.Value);
                if(_accountResources  == null ||!_accountResources.IsValid())
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
                    { CharactersData.PlayFabKeyName, JsonUtility.ToJson(_charactersData) },
                    { LevelData.PlayFabKeyName, JsonUtility.ToJson(_accountLevel) },
                    { AccountResources.PlayFabKeyName, JsonUtility.ToJson(_accountResources) },
                    { ArenaData.PlayFabKeyName, JsonUtility.ToJson(_arenaData) },
                }
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
            _charactersData.AddCharacter(new Character(firstCharacter));
        }
        private void CreateNewGeneralData()
        {
            _accountGeneralData = new AccountGeneralData();

        }
        #endregion
    }
}


