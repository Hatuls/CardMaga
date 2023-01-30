using CardMaga.Rewards;
using CardMaga.Rewards.Bundles;
using CardMaga.Rewards.Factory.Handlers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
namespace CardMaga.CSV
{
    public class CSVToCurrenciesReward : CSVAbst
    {
        const int FirstIndex = 1;
        const int IDIndex = 0;
        const int NameIndex = 1;
        const int CurrencyTypeIndex = 2;
        const int AmountIndex = 3;


        bool IsFinished;
        public async override Task StartCSV(string data)
        {
            WebRequests.Get(data, (x) => { Debug.LogError("CSV To Currencies Reward\n" + x); }, DownloadedCSV);
            IsFinished = false;
            do
            {
                await Task.Yield();
            } while (!IsFinished);
        }

        private void DownloadedCSV(string csv)
        {
            string[] rows = csv.Replace("\r", "").Trim().Split('\n');
            var handler = ScriptableObject.CreateInstance<RewardFactoryHandlerSO>();

            List<CurrencyRewardFactorySO> factories = new List<CurrencyRewardFactorySO>();

            handler.SetID(RewardType.Currency);
            for (int i = FirstIndex; i < rows.Length; i++)
            {
                string[] row = rows[i].Replace('"', ' ').Replace('/', ' ').Split(',');
                if (!int.TryParse(row[IDIndex], out int resultID))
                    break;
                var instance = ScriptableObject.CreateInstance<CurrencyRewardFactorySO>();

                string name = row[NameIndex];

                if (!int.TryParse(row[CurrencyTypeIndex], out int currencyType))
                    throw new Exception($"CSVToCurrenciesReward: Could not convert currencyType to int\nID - {resultID}\nInput Recieved - {row[CurrencyTypeIndex]}");

                CurrencyType currency = (CurrencyType)currencyType;


                if (!float.TryParse(row[AmountIndex], out float amount))
                    throw new Exception($"CSVToCurrenciesReward: Could not convert Amount to int\nID - {resultID}\nInput Recieved - {row[AmountIndex]}");

                var resource = new Rewards.Bundles.ResourcesCost();
                resource.Init(currency, amount);

                instance.AssignValues(resultID, name,RewardType.Currency);
                instance.AssignResources(resource);
                AssetDatabase.CreateAsset(instance, $"Assets/Resources/Rewards/Factories/Currencies/{name} Factory.asset");
                factories.Add(instance);
            }

            handler.Init(factories.ToArray());
            AssetDatabase.CreateAsset(handler, $"Assets/Resources/Rewards/Factories/Handlers/CurrencyRewardFactoryHandlerSO.asset");
            AssetDatabase.SaveAssets();
            CSVManager.RewardFactoryManager.Add(handler);
            IsFinished = true;
        }
    }


    public class CSVToGiftReward : CSVAbst
    {
        const int FirstRow = 2;
        const int IDIndex = 0;
        const int NameIndex = 1;
        const int CurrencyRewardTypeIndex = 2;
        const int CharacterRewardTypeIndex = 4;
        const int CardsPackRewardTypeIndex = 6;
        const int CurrencyRewardIndex = 3;
        const int CharacterRewardIndex = 5;
        const int CardsPackRewardIndex = 7;


        bool IsFinished;
        public async override Task StartCSV(string data)
        {
            WebRequests.Get(data, (x) => { Debug.LogError("CSV To Gifts Reward\n" + x); }, DownloadedCSV);
            IsFinished = false;
            do
            {
                await Task.Yield();
            } while (!IsFinished);
        }

        private void DownloadedCSV(string csv)
        {
            string[] rows = csv.Replace("\r", "").Trim().Split('\n');
            var handler = ScriptableObject.CreateInstance<RewardFactoryHandlerSO>();
            var allFactorys = CSVManager.RewardFactoryManager;
            List<GiftRewardFactorySO> factories = new List<GiftRewardFactorySO>();
            handler.SetID(RewardType.Gift);
            for (int i = FirstRow; i < rows.Length; i++)
            {
                string[] row = rows[i].Replace('"', ' ').Replace('/', ' ').Split(',');
                if (!int.TryParse(row[IDIndex], out int resultID))
                    break;
            List<BaseRewardFactorySO> otherFactories = new List<BaseRewardFactorySO>();
                var instance = ScriptableObject.CreateInstance<GiftRewardFactorySO>();

                string name = row[NameIndex];


                //Currencies
                int rewardType = int.Parse(row[CurrencyRewardTypeIndex]);
                int[] factoriesID = GenerateIDsFromFactories(row[CurrencyRewardIndex]);
                for (int j = 0; j < factoriesID.Length; j++)
                    otherFactories.Add(allFactorys.GetRewardFactory(rewardType, factoriesID[j]));


                //Characters
                 rewardType = int.Parse(row[CharacterRewardTypeIndex]);
                 factoriesID = GenerateIDsFromFactories(row[CharacterRewardIndex]);
                for (int j = 0; j < factoriesID.Length; j++)
                    otherFactories.Add(allFactorys.GetRewardFactory(rewardType, factoriesID[j]));

                //Pack Cards
                rewardType = int.Parse(row[CardsPackRewardTypeIndex]);
                factoriesID = GenerateIDsFromFactories(row[CardsPackRewardIndex]);
                for (int j = 0; j < factoriesID.Length; j++)
                    otherFactories.Add(allFactorys.GetRewardFactory(rewardType, factoriesID[j]));


                instance.AssignValues(resultID, name, RewardType.Gift);
                instance.Init(otherFactories.ToArray());
                AssetDatabase.CreateAsset(instance, $"Assets/Resources/Rewards/Factories/Gifts/{name} Factory.asset");
                factories.Add(instance);
            }

            handler.Init(factories.ToArray());
            AssetDatabase.CreateAsset(handler, $"Assets/Resources/Rewards/Factories/Handlers/GiftRewardFactoryHandlerSO.asset");
            AssetDatabase.SaveAssets();
            CSVManager.RewardFactoryManager.Add(handler);
            IsFinished = true;
        }

        private int[] GenerateIDsFromFactories(string v)
        {
            string[] rewardIDS = v.Split('^');
            List<int> ids = new List<int>();

            for (int i = 0; i < rewardIDS.Length; i++)
            {
                if (int.TryParse(rewardIDS[i], out int id))
                    ids.Add(id);
            }
            return ids.ToArray();
        }
    }


    public class CSVToBundleReward : CSVAbst
    {
        const int FirstIndex = 2;
        const int IDIndex = 0;
        const int NameIndex = 1;
        const int CostIndex = 2;
        const int CurrencyTypeIndex = 3;
        const int RewardTypeIndex = 4;
        const int GiftIndex = 5;
     
        bool IsFinished;
        public async override Task StartCSV(string data)
        {
            WebRequests.Get(data, (x) => { Debug.LogError("CSV To Bundles Reward\n" + x); }, DownloadedCSV);
            IsFinished = false;
            do
            {
                await Task.Yield();
            } while (!IsFinished);
        }

        private void DownloadedCSV(string csv)
        {
            string[] rows = csv.Replace("\r", "").Trim().Split('\n');
            var handler = ScriptableObject.CreateInstance<RewardFactoryHandlerSO>();
            var allFactorys = CSVManager.RewardFactoryManager;
            List<BundleRewardFactorySO> factories = new List<BundleRewardFactorySO>();
            handler.SetID(RewardType.Bundle);
            for (int i = FirstIndex; i < rows.Length; i++)
            {
                string[] row = rows[i].Replace('"', ' ').Replace('/', ' ').Split(',');
                if (!int.TryParse(row[IDIndex], out int resultID))
                    break;
                List<BaseRewardFactorySO> otherFactories = new List<BaseRewardFactorySO>();
                var instance = ScriptableObject.CreateInstance<BundleRewardFactorySO>();

                string name = row[NameIndex];

                ResourcesCost resourcesCost = new ResourcesCost();

                if (!int.TryParse(row[CostIndex], out int cost))
                    throw new Exception($"CSVToBundleRewards: Could not convert cost to int!\nID -{resultID}\nInput Received - {row[CostIndex]}");

                if (!int.TryParse(row[CurrencyTypeIndex], out int currencyType))
                    throw new Exception($"CSVToBundleRewards: Could not convert CurrencyTypeIndex to int!\nID -{resultID}\nInput Received - {row[CurrencyTypeIndex]}");


                CurrencyType currency = (CurrencyType)currencyType;

                resourcesCost.Init(currency, cost);

                //Gifts
                int rewardType = int.Parse(row[RewardTypeIndex]);
                int[] factoriesID = GenerateIDsFromFactories(row[GiftIndex]);
                for (int j = 0; j < factoriesID.Length; j++)
                    otherFactories.Add(allFactorys.GetRewardFactory(rewardType, factoriesID[j]));




                instance.AssignValues(resultID, name, RewardType.Bundle);
                instance.Init(resourcesCost, otherFactories.ToArray());
                AssetDatabase.CreateAsset(instance, $"Assets/Resources/Rewards/Factories/Bundles/{name} Factory.asset");
                factories.Add(instance);
            }

            handler.Init(factories.ToArray());
            AssetDatabase.CreateAsset(handler, $"Assets/Resources/Rewards/Factories/Handlers/BundlesRewardFactoryHandlerSO.asset");
            AssetDatabase.SaveAssets();
            CSVManager.RewardFactoryManager.Add(handler);
            IsFinished = true;
        }

        private int[] GenerateIDsFromFactories(string v)
        {
            string[] rewardIDS = v.Split('^');
            List<int> ids = new List<int>();

            for (int i = 0; i < rewardIDS.Length; i++)
            {
                if (int.TryParse(rewardIDS[i], out int id))
                    ids.Add(id);
            }
            return ids.ToArray();
        }
    }
}
