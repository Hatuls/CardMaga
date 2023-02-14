using CardMaga.Core;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
namespace CardMaga.Rewards
{
    public class RewardManager : MonoSingleton<RewardManager>
    {
        [SerializeField]
        private SceneLoader _sceneLoader;

        [SerializeField]
        private RewardsData _rewardsData;

        public IRewardsData RewardsData => _rewardsData;

        public override void Awake()
        {
            base.Awake();
            _rewardsData = new RewardsData();
        }

        public void OpenRewardsScene(params BaseRewardFactorySO[] rewards)
        {
            _rewardsData.AddRewards(rewards);
            OpenRewardsScene();
        }
       
        public void OpenRewardsScene()
        {
            _sceneLoader.LoadScene();
        }


        #region Editor
        [Header("Editor:"), SerializeField]
        private BaseRewardFactorySO[] rewards;


        [Button]
        private void Test()
        {
            OpenRewardsScene(rewards);
        }
        #endregion

    }

    public interface IRewardsData
    {
  
        bool HasRewards { get; }
        IReadOnlyList<BaseRewardFactorySO> Rewards { get; }

        void ClearRewards();
        void AddRewards(params BaseRewardFactorySO[] rewards);
        void RemoveReward(params BaseRewardFactorySO[] rewards);

    }
#if UNITY_EDITOR
    [System.Serializable]
#endif
    public class RewardsData : IRewardsData
    {
        [ReadOnly, ShowInInspector]
        private readonly List<BaseRewardFactorySO> _rewards;
        public RewardsData()
        {
            _rewards = new List<BaseRewardFactorySO>();
        }
        public IReadOnlyList<BaseRewardFactorySO> Rewards => _rewards;
        public bool HasRewards => _rewards.Count > 0;


        public void AddRewards(params BaseRewardFactorySO[] rewards)
        {
            if (rewards == null || rewards.Length == 0)
                return;

            for (int i = 0; i < rewards.Length; i++)
                _rewards.Add(rewards[i]);
        }
        public void RemoveReward(params BaseRewardFactorySO[] rewards)
        {
            if (rewards == null || rewards.Length == 0)
                return;

            for (int i = 0; i< rewards.Length; i++)
                _rewards.Remove(rewards[i]);

        }
        public void ClearRewards() => _rewards.Clear();

    
    }
}