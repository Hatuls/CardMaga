using CardMaga.Rewards;
using CardMaga.Rewards.Factory.Handlers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
namespace CardMaga.CSV
{
    public class CSVToCharacterReward : CSVAbst
    {
        const int FirstIndex = 2;
        const int IDIndex = 0;
        const int NameIndex = 1;
        const int CharacterIDIndex = 2;



        bool IsFinished;
        public async override Task StartCSV(string data)
        {
            WebRequests.Get(data, (x) => { Debug.LogError("CSV To Character Reward\n" + x); }, DownloadedCSV);
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

            List<CharacterRewardFactorySO> factories = new List<CharacterRewardFactorySO>();

            handler.SetID(RewardType.Character);
            for (int i = FirstIndex; i < rows.Length; i++)
            {
                string[] row = rows[i].Replace('"', ' ').Replace('/', ' ').Split(',');
                if (!int.TryParse(row[IDIndex], out int resultID))
                    break;
                var instance = ScriptableObject.CreateInstance<CharacterRewardFactorySO>();

                string name = row[NameIndex];

                if (!int.TryParse(row[CharacterIDIndex], out int characterID))
                    throw new Exception($"CSVToCurrenciesReward: Could not convert Character CoreID to int\nID - {resultID}\nInput Recieved - {row[CharacterIDIndex]}");




                instance.AssignValues(resultID, name, RewardType.Character);
                instance.Init(characterID);
                AssetDatabase.CreateAsset(instance, $"Assets/Resources/Rewards/Factories/Characters/{name} Factory.asset");
                factories.Add(instance);
            }

            handler.Init(factories.ToArray());
            AssetDatabase.CreateAsset(handler, $"Assets/Resources/Rewards/Factories/Handlers/CharacterRewardFactoryHandlerSO.asset");
            AssetDatabase.SaveAssets();
            CSVManager.RewardFactoryManager.Add(handler);
            IsFinished = true;
        }
    }
}
