
using CardMaga.Rewards.Bundles;
using CardMaga.Rewards.Factory.Handlers;
using UnityEngine;


namespace CardMaga.Rewards
{
    [CreateAssetMenu(fileName = "New Bundle Reward Factory", menuName = "ScriptableObjects/Rewards/Bundle/New Bundle Pack Reward")]
    public class BundleRewardFactorySO : BaseRewardFactorySO
    {
        [SerializeField]
        private RewardTypeAndID[] _rewardsIDs;

        [SerializeField]
        private ResourcesCost _resourcesCost;

        [SerializeField] RewardFactoryManagerSO _rewardFactoyManager;
        public override IRewardable GenerateReward()
        {
            var factorys = _rewardFactoyManager.GetRewardFactorySOs(_rewardsIDs);
            IRewardable[] rewardables = new IRewardable[factorys.Length];

            for (int i = 0; i < factorys.Length; i++)
                rewardables[i] = factorys[i].GenerateReward();

            var reward = new BundleReward(_resourcesCost,rewardables);

            return reward;
        }
#if UNITY_EDITOR
        public void Init(ResourcesCost cost, RewardTypeAndID[] rewardsIDs)
        {
            _resourcesCost = cost;
            _rewardsIDs = rewardsIDs;
        }
#endif
    }
}
