using ReiTools.TokenMachine;
using System;
using UnityEngine;
namespace CardMaga.Rewards.Bundles
{

    [Serializable]
    public class BundleReward : GiftReward
    {
        [SerializeField]
        private ResourcesCost _resourcesCost;

        public ResourcesCost Resources => _resourcesCost;
        public BundleReward(ResourcesCost resourcesCost,RewardType rewardType, IRewardable[] rewards) : base(rewardType, rewards)
        {
            _resourcesCost = resourcesCost;
        }


    }

    [Serializable]
    public class GiftReward : IRewardable
    {
        public event Action OnServerFailedToAdded;
        public event Action OnServerSuccessfullyAdded;
        [SerializeField]
        protected string _name;

        private RewardType _rewardType;
        private IDisposable _token;
        protected IRewardable[] _rewardables;






        public string Name => _name;
        public IRewardable[] Rewardables => _rewardables;
        public RewardType RewardType => _rewardType;
        public virtual void TryRecieveReward(ITokenReciever tokenMachine)
        {
            _token = tokenMachine.GetToken();
            var giftTokenMachine = new TokenMachine(Finished);

            AddToDevicesData();

            Account.AccountManager.Instance.SendAccountData(giftTokenMachine);
            void Finished()
            {
                OnServerSuccessfullyAdded?.Invoke();
                _token.Dispose();
            }
        }

        public void AddToDevicesData()
        {
            for (int i = 0; i < _rewardables.Length; i++)
                _rewardables[i].AddToDevicesData();
        }

        public GiftReward(RewardType rewardType,IRewardable[] rewardables)
        {
            _rewardables = rewardables;
            _rewardType = rewardType;
        }


    }

    [Serializable]
    public class ResourcesCost
    {
        [SerializeField]
        private CurrencyType _currencyType;
        [SerializeField]
        private float _amount;

        public ResourcesCost(CurrencyType currencyType, float amount)
        {
            _currencyType = currencyType;
            _amount = amount;
        }


        public float Amount => _amount;
        public CurrencyType CurrencyType => _currencyType;

#if UNITY_EDITOR
        public void Init(CurrencyType currencyType, float amount)
        {
            _currencyType = currencyType;
            _amount = amount;
        }
#endif
        public ResourcesCost()
        {

        }
    }
}
