using Rewards;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
namespace CardMaga.CSV
{
    public class CSVToPackReward : CSVAbst
    {
        public static bool IsFinished;
        public async override Task StartCSV(string data)
        {
            WebRequests.Get(data, (x) => { Debug.LogError("CSV To Pack Rewards\n" + x); }, DownloadedCSV);
            IsFinished = false;
            do
            {
                await Task.Yield();
            } while (!IsFinished);

        }

        private void DownloadedCSV(string csv)
        {
            CSVManager._packRewardsCollectionSO = ScriptableObject.CreateInstance<PackRewardsCollectionSO>();
            List<PackRewardSO> packs = new List<PackRewardSO>();
            const int startPoint = 1;
            string[] rows = csv.Replace("\r", "").Split('\n');
            for (int i = startPoint; i < rows.Length; i++)
            {
                string[] line = rows[i].Replace('"', ' ').Replace('/', ' ').Split(',');
                var pack = CreatePack(line);
                if (pack == null)
                    break;
                else
                    packs.Add(pack);
            }
            CSVManager._packRewardsCollectionSO.Init(packs.ToArray());

            AssetDatabase.CreateAsset(CSVManager._packRewardsCollectionSO, $"Assets/Resources/Collection SO/PackRewardCollection.asset");

            AssetDatabase.SaveAssets();
            IsFinished = true;
        }

        private PackRewardSO CreatePack(string[] line)
        {
            const int ID = 0;
            if (line[ID] != "-")
            {
                var rewards = ScriptableObject.CreateInstance<PackRewardSO>();

                if (rewards.Init(line))
                {
                    rewards.LoadRewardCards(CSVManager._cardCollection);
                    AssetDatabase.CreateAsset(rewards, $"Assets/Resources/Rewards/PackRewards/{rewards.PackName}PackRewardSO.asset");
                    return rewards;
                }
                else
                    return null;
            }
            else
                return null;
        }
    }
}