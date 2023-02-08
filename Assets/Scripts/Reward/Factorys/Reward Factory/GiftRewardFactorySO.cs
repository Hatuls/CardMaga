
using CardMaga.Battle.Players;
using CardMaga.Rewards.Bundles;
using CardMaga.Rewards.Factory.Handlers;
using System.Collections.Generic;
using UnityEngine;


namespace CardMaga.Rewards
{
    [CreateAssetMenu(fileName = "New Gift Reward Factory", menuName = "ScriptableObjects/Rewards/Gift/New Gift Pack Reward")]
    public class GiftRewardFactorySO : BaseRewardFactorySO
    {
        [SerializeField]
        private BaseRewardFactorySO[] _rewardsIDs;

    
        public override IRewardable GenerateReward()
        {

            IRewardable[] rewardables = new IRewardable[_rewardsIDs.Length];

            for (int i = 0; i < _rewardsIDs.Length; i++)
                    rewardables[i] = _rewardsIDs[i].GenerateReward();

            var reward = new GiftReward(RewardTypes, rewardables);
 
            return reward;
        }
#if UNITY_EDITOR
        public void Init(BaseRewardFactorySO[] rewardsIDs)
        {
            _rewardsIDs = rewardsIDs;

            for (int i = 0; i < rewardsIDs.Length; i++)
            {
                _tags |= _rewardsIDs[i].RewardTypes;
            }
        }
#endif
    }
}
