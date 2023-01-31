
using CardMaga.Rewards.Bundles;
using CardMaga.Rewards.Factory.Handlers;
using System.Collections.Generic;
using UnityEngine;


namespace CardMaga.Rewards
{
    [CreateAssetMenu(fileName = "New Bundle Reward Factory", menuName = "ScriptableObjects/Rewards/Bundle/New Bundle Pack Reward")]
    public class BundleRewardFactorySO : BaseRewardFactorySO
    {
        [SerializeField]
        private BaseRewardFactorySO[] _rewardsIDs;

        [SerializeField]
        private ResourcesCost _resourcesCost;

        public override IRewardable GenerateReward()
        {

            IRewardable[] rewardables = new IRewardable[_rewardsIDs.Length];

            for (int i = 0; i < _rewardsIDs.Length; i++)
                rewardables[i] = _rewardsIDs[i].GenerateReward();

            var reward = new BundleReward(_resourcesCost, RewardTypes, rewardables);

            return reward;
        }

    
#if UNITY_EDITOR
        public void Init(ResourcesCost cost, BaseRewardFactorySO[] rewardsIDs)
        {
            _rewardsIDs = rewardsIDs;
            for (int i = 0; i < _rewardsIDs.Length; i++)
                _tags |= _rewardsIDs[i].RewardTypes;
            _tags= _tags.Remove(RewardType.Gift);
            _resourcesCost = cost;
        }
#endif
    }
}
