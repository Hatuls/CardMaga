using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
namespace CardMaga.CSV
{
    public class CSVToPackRewardData : CSVAbst
    {
        public static bool IsFinished;

        const int FirstIndex = 2;
        const int CardIDIndex = 0;
        const int IsArenaIndex = 1;
        const int IsBasicPackRewardIndex = 2;
        const int IsSpecialPackIndex = 3;

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
     


            string[] rows = csv.Replace("\r", "").Trim().Split('\n');
            var cardCollection = CSVManager._cardCollection;

            bool isArenaReward = false;
            bool isBasicReward = false;
            bool isSpecialPack = false;
            for (int i = FirstIndex; i < rows.Length; i++)
            {
                string[] row = rows[i].Replace('"', ' ').Replace('/', ' ').Split(',');

                if (!int.TryParse(row[CardIDIndex], out int resultID))
                    break;

                Card.CardSO cardSO = cardCollection[resultID];
                if (cardSO == null)
                    throw new System.Exception($"ComboSo was not found!\nID - " + resultID);

                isArenaReward = IsArenaReward(row[IsArenaIndex], resultID);
                isSpecialPack = IsSpecialPack(row[IsSpecialPackIndex], resultID);
                isBasicReward = IsBasicPack(row[IsBasicPackRewardIndex], resultID);

                cardSO.AssignRewardData(resultID, isBasicReward, isSpecialPack, isArenaReward);
            }
            AssetDatabase.SaveAssets();
            IsFinished = true;
        }
        private static bool IsSpecialPack(string value, int resultID)
        {
            bool result;
            if (int.TryParse(value, out int binaryValue))
            {
                if (binaryValue != 1 && binaryValue != 0)
                    throw new System.Exception($"CSVToPackRewards - Could not convert IsSpecialPack to binaryNumber\nID = {resultID}\nValue Received = {value}");

                result = binaryValue == 1;
            }
            else
                throw new System.Exception($"CSVToPackRewards - Could not convert IsSpecialPack to binaryNumber\nID = {resultID}\nValue Received = {value}");
            return result;
        }
        private static bool IsBasicPack( string value, int resultID)
        {
            bool result;
            if (int.TryParse(value, out int binaryValue))
            {
                if (binaryValue != 1 && binaryValue != 0)
                    throw new System.Exception($"CSVToPackRewards - Could not convert IsBasicPack to binaryNumber\nID = {resultID}\nValue Received = {value}");

                result = binaryValue == 1;
            }
            else
                throw new System.Exception($"CSVToPackRewards - Could not convert IsBasicPack to binaryNumber\nID = {resultID}\nValue Received = {value}");
            return result;
        }

        private static bool IsArenaReward(string value, int resultID)
        {
            bool result;
            if (int.TryParse(value, out int binaryValue))
            {
                if (binaryValue != 1 && binaryValue != 0)
                    throw new System.Exception($"CSVToPackRewards - Could not convert IsArenaReward to binaryNumber\nID = {resultID}\nValue Received = {value}");

                result = binaryValue == 1;
            }
            else
                throw new System.Exception($"CSVToPackRewards - Could not convert IsArenaReward to binaryNumber\nID = {resultID}\nValue Received = {value}");
            return result;
        }

    }
}