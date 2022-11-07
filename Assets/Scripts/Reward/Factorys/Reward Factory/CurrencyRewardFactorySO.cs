
using CardMaga.Rewards.Bundles;
using UnityEngine;


namespace CardMaga.Rewards
{
    [CreateAssetMenu(fileName = "New Currency Reward Factory", menuName = "ScriptableObjects/Rewards/Currency/New Currency Reward")]
    public class CurrencyRewardFactorySO : BaseRewardFactorySO
    {
        [SerializeField]
        private CurrencyReward _resourcesReward;
        public override IRewardable GenerateReward()
        => _resourcesReward;


#if UNITY_EDITOR
        public void AssignResources(ResourcesCost resourcesCost)
        {
            _resourcesReward = new CurrencyReward();
            _resourcesReward.Init(Name, resourcesCost);
        }
#endif
    }
}
