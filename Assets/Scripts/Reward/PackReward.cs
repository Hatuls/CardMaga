﻿using Account.GeneralData;
using PlayFab;
using PlayFab.ClientModels;
using ReiTools.TokenMachine;
using System;
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
            _token = tokenMachine.GetToken();
            AddToDevicesData();
            UpdateOnServer();
        }

        private void UpdateOnServer()
        {
    
            var request = new ExecuteCloudScriptRequest()
            {
                FunctionName = "AddCards",
                FunctionParameter = new
                {
                    Cards = JsonUtility.ToJson(Account.AccountManager.Instance.Data.AllCards)
                }
            };
            Account.AccountManager.Instance.UpdateDataOnServer();

            PlayFabClientAPI.ExecuteCloudScript(request, OnRewardReceived, OnFailedToReceived);
        }

        private void OnFailedToReceived(PlayFabError obj)
        {
            OnServerFailedToAdded?.Invoke();
            Debug.LogError(obj.ErrorMessage);
            _token.Dispose();

        }
        private void OnRewardReceived(ExecuteCloudScriptResult obj)
        {
            OnServerSuccessfullyAdded?.Invoke();
            Debug.LogError("Received in server!");
            Account.AccountManager.Instance.RequestAccoundData();
            _token.Dispose();
        }
        public void AddToDevicesData()
        {

            for (int i = 0; i < _cardsID.Length; i++)
                Account.AccountManager.Instance.Data.AllCards.AddCard(new CoreID(_cardsID[i]));

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
        void TryRecieveReward(ITokenReciever tokenMachine);//T reciever);
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
        Coins = 1,
        Diamonds = 2,
        Chips = 3,
        Account_EXP = 4,
        Free = 5
    }
}