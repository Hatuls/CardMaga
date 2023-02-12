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
}