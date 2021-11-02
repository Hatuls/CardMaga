using Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;



public class CSVTORecipeSO : CSVAbst
{
    static bool isCompleted;
    public async  override Task StartCSV(string data)
    {
        isCompleted = false;
        WebRequests.Get(data, (x) => Debug.LogError($"Error On Loading Recipes {x} "), OnCompleteDownloadingRecipeCSV);
        while (isCompleted == false)
        {
            await Task.Yield();
        }
    }

    private static async void OnCompleteDownloadingRecipeCSV(string txt)
    {

        // CSVToCardSO.DestroyWebGameObjects();

        string[] rows = txt.Replace("\r", "").Split('\n');

        if (CSVManager._cardCollection == null || CSVManager._cardCollection.GetAllCards.Length == 0)
            Debug.LogError("Card Collection Is empty make sure you have cards in the Card Collection SO at \"Resources\\Collection SO\\CardCollection\"");

        CSVManager._comboCollection = ScriptableObject.CreateInstance<ComboCollectionSO>();

        List<Combo.ComboSO> combosRecipe = new List<Combo.ComboSO>();

        for (int i = 1; i < rows.Length; i++)
        {
            string[] line = rows[i].Replace('"', ' ').Replace('/', ' ').Split(',');

            var recipe = CreateComboRecipe(line, CSVManager._cardCollection);

            if (recipe == null)
                break;
            else
                combosRecipe.Add(recipe);

        }

        CSVManager._comboCollection.Init(combosRecipe.ToArray());
        AssetDatabase.CreateAsset(CSVManager._comboCollection, $"Assets/Resources/Collection SO/RecipeCollection.asset");
        AssetDatabase.SaveAssets();

        Debug.Log("Recipe Update Complete!");

        isCompleted = true;

    }
}