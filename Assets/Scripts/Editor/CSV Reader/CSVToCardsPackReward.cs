using CardMaga.Card;
using CardMaga.Rewards;
using CardMaga.Rewards.Factory.Handlers;
using Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
namespace CardMaga.CSV
{
    public class CSVToCardsPackReward : CSVAbst
    {
        const int FirstIndex = 2;
        const int IDIndex = 0;
        const int NameIndex = 1;
        const int CardAmountIndex = 2;
        const int SpecificCardIndex = 3;
        const int SpecificCardIDIndex = 4;
        const int PackChanceIndex = 5;

        const int BasicPackID = 0;
        const int SpecialPackID = 1;

        bool IsFinished;
        public async override Task StartCSV(string data)
        {
            WebRequests.Get(data, (x) => { Debug.LogError("CSV To Pack Reward\n" + x); }, DownloadedCSV);
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

            List<CardsPackRewardFactorySO> factories = new List<CardsPackRewardFactorySO>();

            handler.SetID(RewardType.Pack);
            for (int i = FirstIndex; i < rows.Length; i++)
            {
                string[] row = rows[i].Replace('"', ' ').Replace('/', ' ').Split(',');
                if (!int.TryParse(row[IDIndex], out int resultID))
                    break;
                var instance = ScriptableObject.CreateInstance<CardsPackRewardFactorySO>();

                string name = row[NameIndex];
                instance.AssignValues(resultID, name);
                CreateRarirtyChanceCardContainer(instance, row);
                switch (resultID)
                {
                    case BasicPackID:
                        CreateBasicPack(instance, row);
                        break;
                    case SpecialPackID:
                        CreateSpecialPack(instance, row);
                        break;
                    default:
                        CreateSpecificPack(instance, row);
                        break;
                }

                AssetDatabase.CreateAsset(instance, $"Assets/Resources/Rewards/Factories/Cards/{name} Factory.asset");
                factories.Add(instance);
            }

            handler.Init(factories.ToArray());
            AssetDatabase.CreateAsset(handler, $"Assets/Resources/Rewards/Factories/Handlers/CardRewardFactoryHandlerSO.asset");
            AssetDatabase.SaveAssets();
            CSVManager.RewardFactoryManager.Add(handler);
            IsFinished = true;
        }

        private void CreateSpecificPack(CardsPackRewardFactorySO instance, string[] row)
        {
            string[] cardsID = row[SpecificCardIDIndex].Split('&');
            List<int> cores = new List<int>();
            for (int i = 0; i < cardsID.Length; i++)
            {
                string[] cardIDText = cardsID[i].Split('^');


                if (!int.TryParse(cardIDText[0], out int cardID))
                    throw new Exception($"CSVToCardsPackReward: specific BattleCard coreID not an int {cardIDText[0]}\nPackID - {row[IDIndex]}");

                if (!int.TryParse(cardIDText[1], out int cardLevelID))
                    throw new Exception($"CSVToCardsPackReward: specific BattleCard coreID not an int {cardIDText[0]}\nPackID - {row[IDIndex]}");

                cores.Add(cardID + cardLevelID);


            }
            var results = SeperateCardsIDToRarirty(cores.ToArray());

            for (int i = 0; i < results.GetLength(0); i++)
            {
                var currentContainer = instance.RarirtyContainer[i];
                List<int> ids = new List<int>();
                for (int j = 0; j < results[i].Length; j++)
                {
                    ids.Add(results[i][j]);

                }
                currentContainer.PackCardsRewards.AssignCards(ids);
            }

        }

        private void CreateSpecialPack(CardsPackRewardFactorySO instance, string[] row)
        {
            if (!int.TryParse(row[CardAmountIndex], out int amount))
                throw new Exception($"CSVToCardsPackReward: Pack BattleCard Amount not an int {row[CardAmountIndex]}\nPackID - {row[IDIndex]}");

            var cardCollection = CSVManager._cardCollection;
            var allSpecialRewardsCards = cardCollection.AllCardsCoreInfo.Where(x => x.IsSpecialReward);
            var allSpecialRewardsCardsID = allSpecialRewardsCards.Select(x => x.CardCore.CoreID);

            var results = SeperateCardsIDToRarirty(allSpecialRewardsCardsID.ToArray());

            for (int i = 0; i < results.GetLength(0); i++)
            {
                var currentContainer = instance.RarirtyContainer[i];
                List<int> ids = new List<int>();

                for (int j = 0; j < results[i].Length; j++)
                    ids.Add(results[i][j]);

                currentContainer.PackCardsRewards.AssignCards(ids);
            }
        }




        private void CreateBasicPack(CardsPackRewardFactorySO instance, string[] row)
        {
            if (!int.TryParse(row[CardAmountIndex], out int amount))
                throw new Exception($"CSVToCardsPackReward: Pack BattleCard Amount not an int {row[CardAmountIndex]}\nPackID - {row[IDIndex]}");

            var cardCollection = CSVManager._cardCollection;
            var allSpecialRewardsCards = cardCollection.AllCardsCoreInfo.Where(x => x.IsBasicReward);
            var allSpecialRewardsCardsID = allSpecialRewardsCards.Select(x => x.CardCore.CoreID);

            var results = SeperateCardsIDToRarirty(allSpecialRewardsCardsID.ToArray());

            for (int i = 0; i < results.GetLength(0); i++)
            {
                var currentContainer = instance.RarirtyContainer[i];
                List<int> ids = new List<int>();

                for (int j = 0; j < results[i].Length; j++)
                    ids.Add(results[i][j]);

                currentContainer.PackCardsRewards.AssignCards(ids);
            }
        }

        private void CreateRarirtyChanceCardContainer(CardsPackRewardFactorySO instance, string[] row)
        {

            if (!int.TryParse(row[CardAmountIndex], out int amount))
                throw new Exception($"CSVToCardsPackReward: Pack BattleCard Amount not an int {row[CardAmountIndex]}\nPackID - {row[IDIndex]}");

            string[] packChance = row[PackChanceIndex].Split('&');
            List<RarityChanceCardContainer> rarityChanceCardContainers = new List<RarityChanceCardContainer>();

            for (int i = 0; i < packChance.Length; i++)
            {
                if (!float.TryParse(packChance[i], out float chance))
                    throw new Exception($"CSVToCardsPackReward: Pack Chance  not a float {packChance[i]}\nPackID - {row[IDIndex]}");

                var rarityChanceContainer = new RarityChanceCardContainer();
                var rarityCardContainer = new RarityCardsContainer();

                rarityCardContainer.AssignRarity((Card.RarityEnum)(i + 1));
                rarityChanceContainer.InitChance(chance, rarityCardContainer);

                rarityChanceCardContainers.Add(rarityChanceContainer);
            }
            instance.Init(amount, rarityChanceCardContainers.ToArray());
        }


        private int[][] SeperateCardsIDToRarirty(params int[] cardIDS)
        {
            int rarirtyTypes = Enum.GetValues(typeof(RarityEnum)).Length - 1;

            var cardCollection = CSVManager._cardCollection;
            var orderdCards = cardIDS.OrderBy((x) => cardCollection[x].Rarity);
            List<List<int>> _rarityCardsLists = new List<List<int>>(rarirtyTypes);

            for (int i = 0; i < rarirtyTypes; i++)
                _rarityCardsLists.Add(new List<int>());

            foreach (var card in orderdCards)
                _rarityCardsLists[(int)(cardCollection[card].Rarity) - 1].Add(card);

            int[][] values = new int[rarirtyTypes][];
            for (int i = 0; i < values.GetLength(0); i++)
                values[i] = _rarityCardsLists[i].ToArray();

            return values;
        }
    }
}
