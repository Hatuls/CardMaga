
using CardMaga.Rewards.Bundles;
using CardMaga.Rewards.Factory.Handlers;
using UnityEngine;


namespace CardMaga.Rewards
{
    [CreateAssetMenu(fileName = "New Gift Reward Factory", menuName = "ScriptableObjects/Rewards/Gift/New Gift Pack Reward")]
    public class GiftRewardFactorySO : BaseRewardFactorySO
    {
        [SerializeField]
        private RewardTypeAndID[] _rewardsIDs;

        [SerializeField] RewardFactoryManagerSO _rewardFactoyManager;
        public override IRewardable GenerateReward()
        {
            var factorys = _rewardFactoyManager.GetRewardFactorySOs(_rewardsIDs);
            IRewardable[] rewardables = new IRewardable[factorys.Length];

            for (int i = 0; i < factorys.Length; i++)
                rewardables[i] = factorys[i].GenerateReward();

            var reward = new GiftReward(rewardables);
 
            return reward;
        }
#if UNITY_EDITOR
        public void Init(RewardTypeAndID[] rewardsIDs)
        {
            _rewardsIDs = rewardsIDs; 
        }
#endif
    }
}
