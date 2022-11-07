using Battle;
using Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CardMaga.CSV
{
    public class CSVToCharacterSO : CSVAbst
    {
        static bool isCompleted;
        public override async Task StartCSV(string url)
        {
            isCompleted = false;

            WebRequests.Get(url, (x) => Debug.Log("Error " + x), OnCompleteDownloadingCharacterCSV);

            while (isCompleted == false)
            {
                await Task.Yield();
            }
        }
        private async static void OnCompleteDownloadingCharacterCSV(string txt)
        {
            string[] rows = txt.Replace("\r", "").Split('\n');

            if (CSVManager._cardCollection == null || CSVManager._cardCollection.GetAllCardsSO.Length == 0)
                Debug.LogError("Card Collection Is empty make sure you have cards in the Card Collection SO at \"Resources\\Collection SO\\CardCollection\"");
            if (CSVManager._comboCollection == null || CSVManager._comboCollection.AllCombos.Length == 0)
                Debug.LogError("Card Collection Is empty make sure you have combos in the recipe Collection SO at \"Resources\\Collection SO\\RecipeCollection\"");

            CSVManager._characterCollection = ScriptableObject.CreateInstance<CharacterCollectionSO>();
            List<CharacterSO> charactersList = new List<CharacterSO>();

            for (int i = 2; i < rows.Length; i++)
            {
                string[] line = rows[i].Replace('"', ' ').Replace('/', ' ').Split(',');

                var character = CreateCharacter(line, CSVManager._cardCollection, CSVManager._comboCollection);

                if (character == null)
                    break;
                else
                    charactersList.Add(character);

            }

            await Task.Yield();

            CSVManager._characterCollection.Init(charactersList.ToArray());

            AssetDatabase.CreateAsset(CSVManager._characterCollection, $"Assets/Resources/Collection SO/CharacterCollection.asset");

            AssetDatabase.SaveAssets();

            //  WebRequests.Get(string.Concat(_driveURL, _driveURLOfBattleRewardSO), (x) => Debug.Log("Error " + x), OnCompleteDownloadingBattleRewardCSV);
            Debug.Log("Character  Update Complete!");
            isCompleted = true;
        }


        private static CharacterSO CreateCharacter(string[] line, CardsCollectionSO cardCollections, ComboCollectionSO recipeCollection)
        {
            const int ID = 0;
            if (ushort.TryParse(line[ID], out ushort characterID))
            {
                var character = ScriptableObject.CreateInstance<CharacterSO>();
                if (character.Init(characterID, line, cardCollections, recipeCollection))
                {
                    AssetDatabase.CreateAsset(character, $"Assets/Resources/Character SO/{character.CharacterName}.asset");
                    return character;
                }
                else
                    return null;
            }
            else
                return null;
        }

    }
}