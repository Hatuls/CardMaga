using Account.GeneralData;
using Battles;
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
        private AccountData _accountData = new AccountData();
        public AccountData Data => _accountData;
        private void Awake()
        {
            _instance = this;
        }
        internal void Init(ITokenReciever tokenReciever)
        {
//PlayFab.PlayFabClientAPI.GetUserData(new PlayFab.ClientModels.GetUserDataRequest())          


        }
    }

    [System.Serializable]
    public class AccountData
    {
        [SerializeField] private string _displayName = "New Player";
        [SerializeField] private AccountGeneralData _accountGeneralData;
        [SerializeField] private List<CharacterData> _characterData;

        [SerializeField] private LevelData _accountLevel;
        [SerializeField] private AccountResources accountResources;

        [SerializeField] private ArenaData _arenaData;

        public string DisplayName { get => _displayName; set => _displayName = value; }
        public void AddCharacter(CharacterSO newCharacter)
        {
            if (_characterData.Find(x => x.Id == newCharacter.ID) == null)
                _characterData.Add(new CharacterData(newCharacter));
        }
        public AccountData()
        {
            _characterData = new List<CharacterData>();
            _accountGeneralData = new AccountGeneralData();
            _accountLevel = new LevelData();
        }
    }
    //    [SerializeField] AccountCards _accountCards;

    //    [SerializeField] AccountCombos _accountCombos;

    //    [SerializeField] AccountSettingsData _accountSettingsData;

    //    [SerializeField] AccountCharacters _accountCharacters;

    //    [SerializeField] BattleData _battleData;

    //    public AccountCharacters AccountCharacters { get => _accountCharacters; private set => _accountCharacters = value; }
    //    public AccountCards AccountCards { get => _accountCards; private set => _accountCards = value; }
    //    public AccountCombos AccountCombos { get => _accountCombos; private set => _accountCombos = value; }
    //    public AccountSettingsData AccountSettingsData { get => _accountSettingsData; private set => _accountSettingsData = value; }
    //    public AccountGeneralData AccountGeneralData { get => _accountGeneralData; private set => _accountGeneralData = value; }
    //    public BattleData BattleData { get => _battleData; set => _battleData = value; }
    //    public ScenesEnum CurrentScene { get => _lastScene; set => _lastScene = value; }

    //    [SerializeField]
    //    ScenesEnum _lastScene;

    //    public void NewLoad()
    //    {
    //        AccountSettingsData = new AccountSettingsData();
    //         AccountSettingsData.NewLoad();
    //        AccountGeneralData = new AccountGeneralData();
    //         AccountGeneralData.NewLoad();
    //        AccountCards = new AccountCards();
    //         AccountCards.NewLoad();
    //        AccountCombos = new AccountCombos();
    //         AccountCombos.NewLoad();
    //        AccountCharacters = new AccountCharacters();
    //         AccountCharacters.NewLoad();
    //        _battleData = new BattleData();
    //        _battleData.ResetData();

    //    }

    //    public bool IsCorrupted()
    //    {
    //        bool isCorrupted = false;
    //        isCorrupted |= AccountSettingsData.IsCorrupted();
    //        isCorrupted |= AccountGeneralData.IsCorrupted();
    //        isCorrupted |= AccountCards.IsCorrupted();
    //        isCorrupted |= AccountCombos.IsCorrupted();
    //        isCorrupted |= AccountCharacters.IsCorrupted();

    //        return isCorrupted;
    //    }
    //}
    //[System.Serializable]
    //public class RewardGift
    //{
    //    public bool NeedToBeRewarded { get => _needToBeRewarded; set => _needToBeRewarded = value; }

    //    [SerializeField] bool _needToBeRewarded = false;
    //}
}


