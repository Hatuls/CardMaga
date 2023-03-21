using Account.GeneralData;
using CardMaga.MetaData;
using CardMaga.Rewards;
using CardMaga.Rewards.Factory.Handlers;
using System;
using System.Collections.Generic;

public class LevelManager
{
    public event Action OnLevelUp,OnLevelValueChanged;
    public event Action OnExpValueChanged;
    public event Action OnMaxEXPValueChanged;

    private readonly LevelUpRewardsSO _levelUpRewardsSO;
    private readonly LevelData _accountLevel;

    private List<int> _levelUps;
    public int MaxEXP => _levelUpRewardsSO.GetLevelData(Level).MaxEXP;
    public int Level
    {
        get => _accountLevel.Level;
        private set
        {
            _accountLevel.Level = value;
            OnLevelValueChanged?.Invoke();
        }
    }
    public int EXP
    {
        get => _accountLevel.Exp;
        private set
        {
            _accountLevel.Exp = value;
            OnExpValueChanged?.Invoke();
        }
    }
    public LevelManager(LevelUpRewardsSO levelUpRewardsSO, LevelData accountResources)
    {
        _levelUpRewardsSO = levelUpRewardsSO;
        _accountLevel = accountResources;
   
        _levelUps = new List<int>();

    }

    public void AddEXP(int amount)
    {
        int nextLevel = CalculateLevelsUp(EXP + amount, out int remainEXP);

        while (Level < nextLevel)
        {
            Level++;
            _levelUps.Add(Level);
        }
        OnLevelUp?.Invoke();
        EXP = remainEXP;
        OnMaxEXPValueChanged?.Invoke();
    }

    private int CalculateLevelsUp(int amount, out int remainEXP)
    {
        int levelUps = Level;
        remainEXP = RemainEXP(amount);
        return levelUps;

        int RemainEXP(int currentEXP)
        {

            int remain = currentEXP - _levelUpRewardsSO.GetLevelData(levelUps).MaxEXP;
            if (remain >= 0)
            {
                levelUps++;
                return RemainEXP(remain);
            }
            else
                return currentEXP;
        }
    }

  public BaseRewardFactorySO[] GetRewards()
    {
        BaseRewardFactorySO[] baseRewardFactorySOs = new BaseRewardFactorySO[_levelUps.Count];
        var rewardFactoryHandlerSO = Factory.GameFactory.Instance.RewardFactoryHandler.RewardFactoryHandlerSO;
        for (int i = 0; i < _levelUps.Count; i++)
        {
            int currentLevelsReward = _levelUpRewardsSO.GetLevelData(_levelUps[i]).RewardID;
            var reward = rewardFactoryHandlerSO.GetRewardFactory(currentLevelsReward);
            baseRewardFactorySOs[i]=reward;
        }
        _levelUps.Clear();
        return baseRewardFactorySOs;
    }
}