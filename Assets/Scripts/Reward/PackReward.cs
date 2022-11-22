using Account;
using Account.GeneralData;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace CardMaga.Rewards
{
    [Serializable]
    public class PackReward : IRewardable
    {

        public event Action OnServerSuccessfullyAdded;
        public event Action OnServerFailedToAdded;

        [SerializeField]
        private string _packName;

        [SerializeField]
        private int[] _cardsID;
        private IDisposable _token;


        public string Name => _packName;

        public void TryRecieveReward(ITokenReciever tokenMachine)
        {
            AddToDevicesData();

            Account.AccountManager.Instance.SendAccountData(tokenMachine);
            //AddToDevicesData();
            //UpdateOnServer();
        }

        private void UpdateOnServer()
        {

            //string json = JsonConvert.SerializeObject(accountCards);

            ////   json = json.Replace("\"", "").Trim();
            //   Debug.Log(json);
            //   var request = new ExecuteCloudScriptRequest()
            //   {
            //       FunctionName = "AddCards",
            //       FunctionParameter = new
            //       {
            //           Cards = json
            //       }
            //   };
            //   //  

            //   PlayFabClientAPI.ExecuteCloudScript(request, OnRewardReceived, OnFailedToReceived);



        }

        private void OnFailedToReceived(PlayFabError obj)
        {
            OnServerFailedToAdded?.Invoke();
            Debug.LogError(obj.ErrorMessage);
            _token?.Dispose();

        }
        private void OnRewardReceived(ExecuteCloudScriptResult obj)
        {
            OnServerSuccessfullyAdded?.Invoke();
            Debug.LogError("Received in server!" + obj.ToJson());

            Array.ForEach(obj.Logs.ToArray(), x => Debug.LogError(x.ToJson() +"\n"));
            Account.AccountManager.Instance.RequestAccoundData();
            _token?.Dispose();
        }
        public void AddToDevicesData()
        {
            var allcards = AccountManager.Instance.Data.AllCards;
            for (int i = 0; i < _cardsID.Length; i++)
            {
                allcards.AddCard(new CoreID(_cardsID[i]));
            }
            //for (int i = 0; i < _cardsID.Length; i++)
            //    Account.AccountManager.Instance.Data.AllCards.AddCard(new CoreID(_cardsID[i]));

        }
        public PackReward(string name, int[] cardsID)
        {
            _packName = name;
            _cardsID = cardsID;
        }
#if UNITY_EDITOR
        public void Init(string name, int[] cardsID)
        {

        }


#endif
    }



    public interface IRewardable//<T>
    {
        event Action OnServerSuccessfullyAdded;
        event Action OnServerFailedToAdded;
        string Name { get; }
        void TryRecieveReward(ITokenReciever tokenMachine);
        void AddToDevicesData();
    }


    public enum RewardType
    {
        Currency = 0,
        Character = 1,
        Pack = 2,
        Gift = 3,
        Bundle = 4,
        Arena = 5,
        Arena_Skin = 6,
        Character_Skin = 7,
        Character_Color = 8,
        Account_Icons = 9,
    }
    public enum CurrencyType
    {
        None = 0,
        Gold = 1,
        Diamonds = 2,
        Chips = 3,
        Account_EXP = 4,
        Free = 5
    }
}