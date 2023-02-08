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

        public event Action OnServerSuccessfullyAdded;
        public event Action OnServerFailedToAdded;

        private IDisposable _token;
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

        public ResourcesCost(CurrencyType currencyType, float amount)
        {
            _currencyType = currencyType;
            _amount = amount;
        }
   

        public float Amount => _amount;
        public CurrencyType CurrencyType => _currencyType;

        public void AddAmount(float amount) => _amount += amount;
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
