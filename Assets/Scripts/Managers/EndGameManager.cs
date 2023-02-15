using Battle.Data;
using CardMaga.BattleConfigSO;
using CardMaga.Rewards;
using CardMaga.Rewards.Bundles;
using CardMaga.UI;
using ReiTools.TokenMachine;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class EndGameManager : MonoBehaviour
{
    [SerializeField] private UnityEvent _moveToMainMenu;
    [SerializeField] private UnityEvent _moveToTutorial;
    
    [SerializeField] private VictoryAndDefeatHandler _victoryAndDefeat;
    [SerializeField] private RewardScreenUIHandler _rewardScreen;
    [SerializeField] private TextMeshProUGUI _titleText;
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

       var reward =   rewardFactory.GenerateReward();
        SetCurrencyText(reward);
        reward.TryRecieveReward(_rewardTokenMachine);
    }

    private void SetCurrencyText(IRewardable reward)
    {
        string v = string.Empty;
        _titleText.text = GetAllRewards(ref v,reward);
        // Sort it and insert it to each screen
        string GetAllRewards(ref string val, params IRewardable[] rewardables)
        {
           
            for (int i = 0; i < rewardables.Length; i++)
            {
                IRewardable current = rewardables[i];
                RewardType rewardType = current.RewardType;

                if (rewardType.Contain(RewardType.Gift) || rewardType.Contain(RewardType.Bundle))
                    GetAllRewards(ref v,(current as GiftReward).Rewardables);
                else if(current is CurrencyReward c)
                {
                    val = string.Concat(c.Amount, " ").AddImageAfterOfText((int)c.ResourcesCost.CurrencyType -1); // Add Also the SO
                    break;
                  }
            }
            return val;
        }
    }

    public class CurrencyImage
    {
        public int Amount;
        public CurrencyType CurrencyType;

    }

    public void ShowReward()
    {
        if (_isLeftPlayerWon && _isInTutorial == false)
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
