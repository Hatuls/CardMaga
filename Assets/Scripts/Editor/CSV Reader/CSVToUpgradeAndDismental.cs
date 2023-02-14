using CardMaga.MetaData;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
namespace CardMaga.CSV
{
    internal class CSVToUpgradeAndDismental : CSVAbst
    {
        public static bool IsFinished;
        public override async Task StartCSV(string data)
        {
            WebRequests.Get(data, (x) => { Debug.LogError("CSV To Upgrades And Dismental \n" + x); }, DownloadedCSV);
            IsFinished = false;


            do
            {
                await Task.Yield();
            } while (!IsFinished);
        }
        private void DownloadedCSV(string csv)
        {
            string[] rows = csv.Replace("\r", "").Split('\n');
            const int startingRow = 1;
            CreateUpgradesSO(rows[startingRow]);
            CreateDismentalCostSO(rows[startingRow + 1]);
            AssetDatabase.SaveAssets();

            IsFinished = true;
        }
        private void CreateDismentalCostSO(string row)
        {
            CSVManager._dismentalCardCostSO = ScriptableObject.CreateInstance<CurrencyPerRarityCostSO>();
            CSVManager._dismentalCardCostSO.Init(row.Replace('"', ' ').Replace('/', ' ').Split(','));
            AssetDatabase.CreateAsset(CSVManager._dismentalCardCostSO, $"Assets/Resources/MetaGameData/DismentalCostSO.asset");

        }
        private void CreateUpgradesSO(string row)
        {
            CSVManager._upgradeCardCostSO = ScriptableObject.CreateInstance<CurrencyPerRarityCostSO>();
            CSVManager._upgradeCardCostSO.Init(row.Replace('"', ' ').Replace('/', ' ').Split(','));
            AssetDatabase.CreateAsset(CSVManager._upgradeCardCostSO, $"Assets/Resources/MetaGameData/UpgradeCostSO.asset");

        }


    }

    internal class CSVToLevelData : CSVAbst
    {
        public static bool IsFinished;
        public override async Task StartCSV(string data)
        {
            WebRequests.Get(data, (x) => { Debug.LogError("CSV To Level And Data \n" + x); }, DownloadedCSV);
            IsFinished = false;


            do
            {
                await Task.Yield();
            } while (!IsFinished);
        }
        private void DownloadedCSV(string csv)
        {
            string[] rows = csv.Replace("\r", "").Split('\n');
            const int startingRow = 1;
            const int MAX_EXP_INDEX = 1;
            const int GIFT_REWARD_ID_INDEX = 2;
            CSVManager._levelDataSO = ScriptableObject.CreateInstance<LevelUpRewardsSO>();
            List<int> maxEXP = new List<int>();
            List<int> giftRewardID= new List<int>();
            int counter = 0;
            for (int i = startingRow; i < rows.Length; i++)
            {
                string[] cells = rows[i].Split(',');
                for (int j = 0; j < cells.Length; j++)
                {
            
                if (!int.TryParse(cells[MAX_EXP_INDEX], out counter))
                    throw new System.Exception("CSVToLevelData: Max EXP is not a number\nInput " + cells[MAX_EXP_INDEX]);
                    maxEXP.Add(counter);
                if (!int.TryParse(cells[GIFT_REWARD_ID_INDEX],out counter))
                    throw new System.Exception("CSVToLevelData: Gift Reward ID is not a number\nInput " + cells[GIFT_REWARD_ID_INDEX]+"\nID: "+ cells[i]);
                    giftRewardID.Add(counter);
                }
                counter++;
            }
            CSVManager._levelDataSO.Init(maxEXP.ToArray(), giftRewardID.ToArray());
            AssetDatabase.CreateAsset(CSVManager._levelDataSO, $"Assets/Resources/MetaGameData/LevelDataSO.asset");
            AssetDatabase.SaveAssets();

            IsFinished = true;
        }



    }
}