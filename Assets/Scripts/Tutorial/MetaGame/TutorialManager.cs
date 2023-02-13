using Account;
using Battle.Data;
using CardMaga.Core;
using CardMaga.Input;
using CardMaga.Rewards;
using CardMaga.Tutorial;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private UnityEvent OnEndTutorial;


    //[SerializeField] private List<TutorialConfigSO> _tutorialConfig;
    [SerializeField] private int _minTutorialsToExitToMainMenu;
    [SerializeField] private BattleData _battleDataPrefab;
    [SerializeField] private TutorialBadge[] _badges;
    [SerializeField] private SceneLoader _matchMakingSceneLoader;
    [SerializeField] private Button returnToMainMenuButton;
    [SerializeField] private TutorialRewards _tutorialRewards;
    private AccountTutorialData _accountTutorialData;
    private TutorialConfigSO _selectedTutorialConfig;

    #region UnityCallback

    private void Start()
    {
        _accountTutorialData = AccountManager.Instance.Data.AccountTutorialData;
        SubscribeTutorialBadges();

        if (!_accountTutorialData.IsCompletedTutorial)
        {
            UpdateBattleConfig();
            if (BattleData.Instance != null)
                UpdateBadgesForNotCompletedTutorialAccount();

            else
            {
                _battleDataPrefab = Instantiate(_battleDataPrefab);
                if (_accountTutorialData.TutorialProgress == 0)
                    UpdateBadgesForNewAccount();

                else
                    UpdateBadgesForNotCompletedTutorialAccount();
            }

            if (_accountTutorialData.TutorialProgress >= _minTutorialsToExitToMainMenu)
                returnToMainMenuButton.gameObject.SetActive(true);
        }

        else
            UpdateBadgesForCompletedTutorialAccount();
    }

    #endregion

    #region PublicFunction

    public void LoadTutorialBattle(TutorialConfigSO tutorialConfigSO)
    {
        _selectedTutorialConfig = tutorialConfigSO;
        if (_accountTutorialData.TutorialProgress == 1 && _selectedTutorialConfig.TutorialID == 0)
            return;
        BattleData.Instance.AssginBattleTutorialData(_selectedTutorialConfig);
        _matchMakingSceneLoader.LoadScene();
    }

    #endregion

    #region PrivateFunction

    private void UpdateCurrentBattleConfig()
    {
        if (BattleData.Instance.IsPlayerWon)
        {
            UpdateData(false);
            UpdateBadgesForNotCompletedTutorialAccount();
        }
    }

    private void UpdateEndingTutorialBattleConfig()
    {
        UpdateData(true);
        UpdateBadgesForCompletedTutorialAccount();
        OnEndTutorial?.Invoke();
    }

    private void UpdateData(bool isFinished)
    {
        int tutorialProgress = _accountTutorialData.TutorialProgress;
        ReceiveRewards(tutorialProgress);
        AccountManager.Instance.Data.AccountTutorialData.AssignedData(tutorialProgress + 1, isFinished);
        AccountManager.Instance.UpdateDataOnServer();
    }

    private void ReceiveRewards(int tutorialProgress)
    {
        _tutorialRewards.ReceiveReward(tutorialProgress);
    }

    private void UpdateBattleConfig()
    {
        if (_accountTutorialData.IsCompletedTutorial) //Check if completed
            return;

        if (BattleData.Instance == null)
            return;



        for (int i = 0; i < _accountTutorialData.TutorialProgress; i++) //Check you coming back from previous tutorial
        {
            if (BattleData.Instance.BattleConfigSO == _badges[i]._configSO.BattleConfig)
                return;
        }

        //If not update your last config

        if (_accountTutorialData.TutorialProgress > _badges.Length - 1)
            UpdateEndingTutorialBattleConfig();

        else
            UpdateCurrentBattleConfig();

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
            if (i < _accountTutorialData.TutorialProgress)
            {
                _badges[i].Completed();
                continue;
            }

            if (i > _accountTutorialData.TutorialProgress)
            {
                _badges[i].Init();
                continue;
            }

            if (i == _accountTutorialData.TutorialProgress)
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
}
[System.Serializable]
public class TutorialRewards
{
    [SerializeField] private BaseRewardFactorySO[] _tutorialRewards;
    public void ReceiveReward(int currentTutorialLevel)
    {
        RewardManager.Instance.OpenRewardsScene(_tutorialRewards[currentTutorialLevel]);
    }
}