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
     

        for (int i = startingRow; i < rows.Length; i++)
        {
            string[] line = rows[i].Replace('"', ' ').Replace('/', ' ').Split(',');

            if (line[firstElement].Length == 1 || line[firstElement].Length == 0)
                break;
           CreateUpgradesSO(line[firstElement].Split('&'));


            string[] dismentalCost = new string[line.Length-1];
            for (int j = 1; j < line.Length; j++)
                dismentalCost[j-1] += line[j];

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
    private void CreateUpgradesSO (string[] csv)
    {
        CSVManager._upgradeCardCostSO = ScriptableObject.CreateInstance<CardUpgradeCostSO>();
        CSVManager._upgradeCardCostSO.Init(csv);
        AssetDatabase.CreateAsset(CSVManager._upgradeCardCostSO, $"Assets/Resources/MetaGameData/UpgradeCostSO.asset");

    }


}