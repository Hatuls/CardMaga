using Account;
using Battle.Data;
using CardMaga.BattleConfigSO;
using CardMaga.Core;
using CardMaga.Tutorial;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CardMaga.Input;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private UnityEvent OnEndTutorial;

    private static int _currentPlayerTutorialIndex = 0;

    //[SerializeField] private List<TutorialConfigSO> _tutorialConfig;
    [SerializeField] private BattleData _battleDataPrefab;
    [SerializeField] private TutorialBadge[] _badges;
    [SerializeField] private SceneLoader _matchMakingSceneLoader;
    [SerializeField] private Button returnToMainMenuButton;
    private MainMenuTutorial _mainMenuTutorial;
    private AccountTutorialData _accountTutorialData;
    private TutorialConfigSO _selectedTutorialConfig;

    #region PublicFunction
    public void LoadTutorialBattle(TutorialConfigSO tutorialConfigSO)
    {
        _selectedTutorialConfig = tutorialConfigSO;
        if (_currentPlayerTutorialIndex == 1 && _selectedTutorialConfig.TutorialID==0)
            return;
        BattleData.Instance.AssginBattleTutorialData(_selectedTutorialConfig);
        _matchMakingSceneLoader.LoadScene();
    }
    #endregion

    #region PrivateFunction

    private void UpdateCurrentBattleConfig()
    {
        AccountManager.Instance.Data.AccountTutorialData.AssignedData(_currentPlayerTutorialIndex, false);
        AccountManager.Instance.UpdateDataOnServer();
        UpdateBadgesForNotCompletedTutorialAccount();
    }

    private void UpdateEndingTutorialBattleConfig()
    {
        AccountManager.Instance.Data.AccountTutorialData.AssignedData(_currentPlayerTutorialIndex, true);
        AccountManager.Instance.UpdateDataOnServer();
        UpdateBadgesForCompletedTutorialAccount();
        OnEndTutorial?.Invoke();
    }

    private void UpdateBattleConfig()
    {
        if (_accountTutorialData.IsCompletedTutorial)
            return;

        else
        {
            if (_currentPlayerTutorialIndex > _badges.Length - 1)
                UpdateEndingTutorialBattleConfig();

            else
                UpdateCurrentBattleConfig();
        }
    }

    private int GetBattleTutorialConfigIndex(BattleConfigSO battleConfigSo)
    {
        for (int i = 0; i < _badges.Length; i++)
        {
            if (_badges[i]._configSO.BattleConfig == battleConfigSo)
            {
                return i;
            }
        }

        throw new System.Exception("TutorialManager: BattleConfig now found");
    }

    private void UpdateBadgesForNewAccount()
    {
        for (int i = 0; i < _badges.Length; i++)
        {
            _badges[i].Init();
        }
        _badges[0].Open();
    }

    private void UpdateBadgesForNotCompletedTutorialAccount()
    {
        for (int i = 0; i < _badges.Length; i++)
        {
            if (i < _currentPlayerTutorialIndex)
            {
                _badges[i].Completed();
                continue;
            }

            if (i > _currentPlayerTutorialIndex)
            {
                _badges[i].Init();
                continue;
            }

            if (i == _currentPlayerTutorialIndex)
            {
                _badges[i].Open();
                continue;
            }
        }
    }

    private void UpdateBadgesForCompletedTutorialAccount()
    {
        for (int i = 0; i < _badges.Length; i++)
        {
            _badges[i].Completed();
        }
    }

    private void SubscribeTutorialBadges()
    {
        for (int i = 0; i < _badges.Length; i++)
        {
            _badges[i].OnBadgeClicked += LoadTutorialBattle;
        }
    }
    #endregion

    #region UnityCallback

    private void Start()
    {
        _accountTutorialData = AccountManager.Instance.Data.AccountTutorialData;
        if (!_accountTutorialData.IsCompletedTutorial)
        {
            if (BattleData.Instance != null)
            {
                _currentPlayerTutorialIndex = GetBattleTutorialConfigIndex(BattleData.Instance.BattleConfigSO);
                
                if (BattleData.Instance.IsPlayerWon)
                    _currentPlayerTutorialIndex++;

                UpdateBadgesForNotCompletedTutorialAccount();
            }
            else
            {
                _currentPlayerTutorialIndex = _accountTutorialData.TutorialProgress;
                _battleDataPrefab = Instantiate(_battleDataPrefab);
                UpdateBadgesForNewAccount();
            }

            if (_currentPlayerTutorialIndex >= 2)
                returnToMainMenuButton.gameObject.SetActive(true);
        }

        else
            UpdateBadgesForCompletedTutorialAccount();

        SubscribeTutorialBadges();
        UpdateBattleConfig();
    }

    #endregion
}
