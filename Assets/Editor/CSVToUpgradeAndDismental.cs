using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

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
        const int startingRow = 2;
        const int firstElement = 0;
        const int rarities = 5;

        for (int i = startingRow; i < rows.Length; i++)
        {
            string[] line = rows[i].Replace('"', ' ').Replace('/', ' ').Split(',');

            if (line[firstElement].Length == 1 || line[firstElement].Length == 0)
                break;

            string[] upradeByChips = line[firstElement].Split('&');
            string[] upgradesByGolds = new string[line.Length - 1];

            for (int j = (firstElement+1); j < rarities; j++)
            {
                upgradesByGolds[j] = line[i];
            }
            CreateUpgradesSO(upgradesByGolds, upgradesByGolds);


            string[] dismentalCost = new string[rarities];
            for (int j = 0; j < rarities; j++)
                dismentalCost[j] += line[rarities + 1];

            CreateDiscmentalCostSO(dismentalCost);
        }
        AssetDatabase.SaveAssets();

        IsFinished = true;
    }
    private void CreateDiscmentalCostSO(string[] csv)
    {
        CSVManager._dismentalCardCostSO = ScriptableObject.CreateInstance<DismentalCostsSO>();
        CSVManager._dismentalCardCostSO.Init(csv);
        AssetDatabase.CreateAsset(CSVManager._dismentalCardCostSO, $"Assets/Resources/MetaGameData/DismentalCostSO.asset");

    }
    private void CreateUpgradesSO (string[] MapupgradeCost,string[] upgradeChipCost)
    {
        CSVManager._upgradeCardCostSO = ScriptableObject.CreateInstance<CardUpgradeCostSO>();
        CSVManager._upgradeCardCostSO.Init(MapupgradeCost, upgradeChipCost);
        AssetDatabase.CreateAsset(CSVManager._upgradeCardCostSO, $"Assets/Resources/MetaGameData/UpgradeCostSO.asset");

    }


}