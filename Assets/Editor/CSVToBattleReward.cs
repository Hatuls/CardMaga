using Rewards;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class CSVToBattleReward : CSVAbst
{
    private static bool _isCompleted = false;
    public async override Task StartCSV(string data)
    {
        _isCompleted = false;
        WebRequests.Get(data, (x) => Debug.LogError($"Error On Loading BattleRewards {x} "), OnCompleteDownloadingBattleRewardCSV);

        while (_isCompleted == false)
        {
            await Task.Yield();
        }
    }
    private static void OnCompleteDownloadingBattleRewardCSV(string data)
    {
        // CSVToCardSO.DestroyWebGameObjects();
       CSVManager._battleRewards = ScriptableObject.CreateInstance<BattleRewardCollectionSO>();

        List<BattleRewardSO> battleRewards = new List<BattleRewardSO>();

        const int row = 2;
        string[] rows = data.Replace("\r", "").Split('\n');

        for (int i = row; i < rows.Length; i++)
        {
            string[] line = rows[i].Replace('"', ' ').Replace('/', ' ').Split(',');

            var reward = CreateBattleReward(line);

            if (reward == null)
                break;
            else
                battleRewards.Add(reward);

        }

        CSVManager._battleRewards.Init(battleRewards.ToArray());

        AssetDatabase.CreateAsset(CSVManager._battleRewards, $"Assets/Resources/Collection SO/BattleRewardsCollection.asset");

        AssetDatabase.SaveAssets();
        _isCompleted = true;
    }

    private static BattleRewardSO CreateBattleReward(string[] line)
    {
        const int ID = 0;
        if (ushort.TryParse(line[ID], out ushort characterID))
        {
            var rewards = ScriptableObject.CreateInstance<BattleRewardSO>();

            if (rewards.Init(line))
            {
                AssetDatabase.CreateAsset(rewards, $"Assets/Resources/Rewards/BattleRewards/{rewards.CharacterDifficultyEnum}BattleRewardSO.asset");
                return rewards;
            }
            else
                return null;
        }
        else
            return null;
    }
}

