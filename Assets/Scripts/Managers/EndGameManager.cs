using Battle.Data;
using CardMaga.BattleConfigSO;
using ReiTools.TokenMachine;
using System;
using UnityEngine;
using UnityEngine.Events;

public class EndGameManager : MonoBehaviour
{
    [SerializeField] private UnityEvent _moveToMainMenu;
    [SerializeField] private UnityEvent _moveToTutorial;
    
    [SerializeField] private VictoryAndDefeatHandler _victoryAndDefeat;
    [SerializeField] private RewardScreenUIHandler _rewardScreen;
    
    private bool _isInTutorial;
    private bool _isLeftPlayerWon;
    private ITokenReceiver _rewardTokenMachine;
    private IDisposable _rewardToken;
#if UNITY_EDITOR
    [Header("End Game Override")]
    [SerializeField] private bool _isLeftPlayerWonOverride;
    [SerializeField] private bool _isInTutorialOverride;
#endif

    private void Awake()
    {
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }
    private void Start()
    {
#if UNITY_EDITOR
        if (BattleData.Instance == null)
        {
            _isInTutorial = _isInTutorialOverride;
            _isLeftPlayerWon = _isLeftPlayerWonOverride;
            _rewardTokenMachine = new TokenMachine(MoveToNextScene);
            _victoryAndDefeat.OpenScreen(_isLeftPlayerWon);

            return;
        }
#endif
        _isInTutorial = !(BattleData.Instance.BattleConfigSO.BattleTutorial == null);
        _isLeftPlayerWon = BattleData.Instance.IsPlayerWon;
        _rewardTokenMachine = new TokenMachine(MoveToNextScene);
        _victoryAndDefeat.OpenScreen(_isLeftPlayerWon);

        GenerateReward(BattleData.Instance.BattleConfigSO);
    }

    private void GenerateReward(BattleConfigSO battleConfigSO)
    {
        CardMaga.Rewards.BaseRewardFactorySO rewardFactory = _isLeftPlayerWon ? battleConfigSO.WinReward : battleConfigSO.LoseReward;
        _rewardToken = _rewardTokenMachine.GetToken();

        if (rewardFactory == null)
            return;

        var reward = rewardFactory.GenerateReward();
        reward.TryRecieveReward(_rewardTokenMachine);
    }

    public void ShowReward()
    {
        if (_isLeftPlayerWon)
        {
            _victoryAndDefeat.gameObject.SetActive(false);
            _rewardScreen.gameObject.SetActive(true);
        }
        else
            ReleaseSceneToken();
    }
    public void ReleaseSceneToken() => _rewardToken?.Dispose();
    private void MoveToNextScene()
    {
        if (_isInTutorial)
            _moveToTutorial?.Invoke();
        else
            _moveToMainMenu?.Invoke();
    }
}
