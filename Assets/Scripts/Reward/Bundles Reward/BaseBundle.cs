using System;
using UnityEngine;
namespace CardMaga.Rewards.Bundles
{

        [Serializable]
    public class BundleReward : GiftReward
    {
        [SerializeField]
        private ResourcesCost _resourcesCost;

        public BundleReward(ResourcesCost resourcesCost,IRewardable[] rewards) : base( rewards)
        {
            _resourcesCost = resourcesCost;
        }

    
    }

        [Serializable]
    public class GiftReward : IRewardable
    {
        [SerializeField]
        protected string _name;
        public string Name => _name;

        protected IRewardable[] _rewardables;

        public virtual bool TryRecieveReward()
        {
            throw new NotImplementedException();
        }
        public GiftReward(IRewardable[] rewardables)
           => _rewardables = rewardables;
 
                
    }

    [Serializable]
    public class ResourcesCost
    {
        [SerializeField]
        private CurrencyType _currencyType;
        [SerializeField]
        private float _amount;
        public float Amount => _amount;
        public CurrencyType CurrencyType => _currencyType;

#if UNITY_EDITOR
        public void Init(CurrencyType currencyType, float amount)
        {
            _currencyType = currencyType;
            _amount = amount;
        }
#endif
    }
}
