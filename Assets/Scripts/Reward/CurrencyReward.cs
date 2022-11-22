using CardMaga.Rewards.Bundles;
using Newtonsoft.Json;
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

        [SerializeField] 
        private string _name;
        [SerializeField]
        ResourcesCost _resourcesCost;

        public event Action OnServerSuccessfullyAdded;
        public event Action OnServerFailedToAdded;
        private IDisposable _token;
        public string Name => _name;

        public void TryRecieveReward(ITokenReciever tokenMachine)
        {
      
            AddToDevicesData();
            Account.AccountManager instance = Account.AccountManager.Instance;
            instance.SendAccountData(tokenMachine);
           // instance.UpdateDataOnServer();
            //  bool isEXP = _resourcesCost.CurrencyType == CurrencyType.Account_EXP;
            //string json = isEXP ? JsonUtility.ToJson(Account.AccountManager.Instance.Data.AccountLevel) : JsonConvert.SerializeObject(Account.AccountManager.Instance.Data.AccountResources);
            //var request = new ExecuteCloudScriptRequest()
            //{
            //    FunctionName = isEXP ? "AddEXP" : "AddResources",
            //    FunctionParameter = new
            //    {
            //        Value = json
            //    }
            //};

            //   Account.AccountManager.Instance.UpdateDataOnServer();

            //    PlayFabClientAPI.ExecuteCloudScript(request, OnRewardReceived, OnFailedToReceived);



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
            switch (_resourcesCost.CurrencyType)
            {
                case CurrencyType.Gold:
                case CurrencyType.Diamonds:
                case CurrencyType.Chips:
                    Account.AccountManager.Instance.Data.AccountResources.AddResource(_resourcesCost.CurrencyType, (int)_resourcesCost.Amount);
                    break;
                case CurrencyType.Account_EXP:
                    Account.AccountManager.Instance.Data.AccountLevel.Exp += (int)_resourcesCost.Amount;
                    break;
                default:
                    break;
            }
        }

#if UNITY_EDITOR
        public void Init( string name, ResourcesCost resourcesCost)
        {
            _name = name;
            _resourcesCost= resourcesCost;
        }

 

#endif
    }
}