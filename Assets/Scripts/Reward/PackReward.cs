﻿using Account;
using Account.GeneralData;
using CardMaga.Battle.Players;
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
    public enum PackType
    {
        Standard,
        Special,
    }

    [Serializable]
    public class PackReward : IRewardable
    {

        public event Action OnServerSuccessfullyAdded;
        public event Action OnServerFailedToAdded;

        [SerializeField]
        private string _packName;

        [SerializeField]
        private PackType _packType;

        [SerializeField]
        private int[] _cardsID;
        private IDisposable _token;


        public string Name => _packName;

        public RewardType RewardType => RewardType.Pack;
        public PackType PackType => _packType;
        public IReadOnlyList<int> CardsID => _cardsID;

        public int Amount => _cardsID.Length;

        public void TryRecieveReward(ITokenReceiver tokenMachine)
        {
            AddToDevicesData();

            Account.AccountManager.Instance.SendAccountData(tokenMachine);
            //AddToDevicesData();
            //UpdateOnServer();
        }

        private void UpdateOnServer()
        {

         
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
        public PackReward(string name,PackType packType, int[] cardsID)
        {
            _packType = packType;
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
        int Amount { get; }
        RewardType RewardType { get; }
        void TryRecieveReward(ITokenReceiver tokenMachine);
        void AddToDevicesData();
    }


    [Flags]
    public enum RewardType
    {
        None = 0,
        Currency = 1,
        Character = 2,
        Pack = 4,
        Gift = 8,
        Bundle = 16,
        Arena = 32,
        Arena_Skin = 64,
        Character_Skin = 128,
        Character_Color = 256,
        Account_Icons = 512,
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

    public static class RewardTypeHelper
    {
        public static bool Contain(this RewardType @enum, RewardType enumValue)
        => (@enum & enumValue) == enumValue;

        public static RewardType Add(this RewardType @enum, RewardType rewardType)
            => @enum |= rewardType;
        public static RewardType Remove(this RewardType @enum, RewardType rewardType)
            => @enum &= ~rewardType;
    }
}