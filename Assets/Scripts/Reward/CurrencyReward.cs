using CardMaga.Rewards.Bundles;
using PlayFab;
using PlayFab.ClientModels;
using ReiTools.TokenMachine;
using System;
using UnityEngine;
namespace CardMaga.Rewards
{
    [Serializable]
    public class CurrencyReward : IRewardable
    {
        public event Action OnServerSuccessfullyAdded;
        public event Action OnServerFailedToAdded;
        private IDisposable _token;

        [SerializeField]
        private string _name;
        [SerializeField]
        private ResourcesCost _resourcesCost;
        public string Name => _name;

        public RewardType RewardType => RewardType.Currency;

        public ResourcesCost ResourcesCost { get => _resourcesCost; }

        public void TryRecieveReward(ITokenReceiver tokenMachine)
        {

            AddToDevicesData();
            Account.AccountManager instance = Account.AccountManager.Instance;
            instance.SendAccountData(tokenMachine);
        }

        private void OnRewardReceived(ExecuteCloudScriptResult obj)
        {
            OnServerSuccessfullyAdded?.Invoke();
            _token?.Dispose();
        }

        private void OnFailedToReceived(PlayFabError obj)
        {
            OnServerFailedToAdded?.Invoke();
            _token?.Dispose();
        }

        public void AddToDevicesData()
        {
            switch (ResourcesCost.CurrencyType)
            {
                case CurrencyType.Gold:
                case CurrencyType.Diamonds:
                case CurrencyType.Chips:
                    Account.AccountManager.Instance.Data.AccountResources.AddResource(ResourcesCost.CurrencyType, (int)ResourcesCost.Amount);
                    break;
                case CurrencyType.Account_EXP:
                    Account.AccountManager.Instance.Data.AccountLevel.Exp += (int)ResourcesCost.Amount;
                    break;
                default:
                    break;
            }
        }

#if UNITY_EDITOR
        public void Init(string name, ResourcesCost resourcesCost)
        {
            _name = name;
            _resourcesCost = resourcesCost;
        }



#endif
    }
}