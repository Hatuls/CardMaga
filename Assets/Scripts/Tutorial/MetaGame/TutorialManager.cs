using Account;
using Battle.Data;
using CardMaga.Core;
using CardMaga.Input;
using CardMaga.Rewards;
using CardMaga.Tutorial;
using DG.Tweening;
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

    
        }

        else
            UpdateBadgesForCompletedTutorialAccount();

        if (_accountTutorialData.TutorialProgress >= _minTutorialsToExitToMainMenu)
            returnToMainMenuButton.gameObject.SetActive(true);

        _tutorialRewards.Init();
    }
    private void OnDestroy()
    {
        _tutorialRewards.Dispose();
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
    [SerializeField]
    private UnityEngine.UI.Button _packRewardBtn;
    [SerializeField] 
    private BaseRewardFactorySO[] _tutorialRewards;

    [SerializeField] 
    private BaseRewardFactorySO _finalTutorialPackReward;
    [SerializeField]
    private float _floatingButtonHeight = 0.2f;
    [SerializeField]
    private float _floatingButtonDuration = 0.2f;
    [SerializeField]
    private AnimationCurve _curve ;
    private Sequence _sequenece;
    private float _startPos;
    public void Init()
    {
        _packRewardBtn.onClick.AddListener(TryAddCard);
        SetAnimation();

        RefreshPackVisablity();
    }

    private void SetAnimation()
    {
        _startPos = _packRewardBtn.transform.position.y;
        _sequenece = DOTween.Sequence();

        _sequenece.Append(_packRewardBtn.transform.DOMoveY(_startPos + _floatingButtonHeight, _floatingButtonDuration).SetEase(_curve));
        _sequenece.Append(_packRewardBtn.transform.DOMoveY(_startPos, _floatingButtonDuration).SetEase(_curve));
        _sequenece.SetLoops(-1, LoopType.Yoyo);
    }

    public void Dispose()
    {
        _sequenece.Kill();
        _packRewardBtn.onClick.RemoveListener(TryAddCard);
    }

    public void RefreshPackVisablity()
    {
        var tutorial = AccountManager.Instance.Data.AccountTutorialData;
        _packRewardBtn.gameObject.SetActive(!tutorial.IsTutorialRewardTaken);
    }
    private void TryAddCard()
    {
        var tutorial = AccountManager.Instance.Data.AccountTutorialData;
        if(tutorial.IsCompletedTutorial && !tutorial.IsTutorialRewardTaken)
        {
            tutorial.IsTutorialRewardTaken = true;
            RefreshPackVisablity();
            RewardManager.Instance.OpenRewardsScene(_finalTutorialPackReward);
        }
    }


    public void ReceiveReward(int currentTutorialLevel)
    {
        RewardManager.Instance.OpenRewardsScene(_tutorialRewards[currentTutorialLevel]);
    }
}