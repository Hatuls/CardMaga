using Account;
using Battle.Data;
using CardMaga.Core;
using CardMaga.Input;
using CardMaga.Tutorial;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private UnityEvent OnEndTutorial;


    //[SerializeField] private List<TutorialConfigSO> _tutorialConfig;
    [SerializeField] private BattleData _battleDataPrefab;
    [SerializeField] private TutorialBadge[] _badges;
    [SerializeField] private SceneLoader _matchMakingSceneLoader;
    [SerializeField] private Button returnToMainMenuButton;
    private AccountTutorialData _accountTutorialData;
    private TutorialConfigSO _selectedTutorialConfig;

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
            AccountManager.Instance.Data.AccountTutorialData.AssignedData(_accountTutorialData.TutorialProgress + 1, false);
            AccountManager.Instance.UpdateDataOnServer();
            UpdateBadgesForNotCompletedTutorialAccount();
        }
    }

    private void UpdateEndingTutorialBattleConfig()
    {
        AccountManager.Instance.Data.AccountTutorialData.AssignedData(_accountTutorialData.TutorialProgress + 1, true);
        AccountManager.Instance.UpdateDataOnServer();
        UpdateBadgesForCompletedTutorialAccount();
        OnEndTutorial?.Invoke();
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

            if (_accountTutorialData.TutorialProgress >= 2)
                returnToMainMenuButton.gameObject.SetActive(true);
        }

        else
            UpdateBadgesForCompletedTutorialAccount();
    }

    #endregion
}
