using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CSVToCharacterSO
{
    const string _driveURLOfComboSO = "https://docs.google.com/spreadsheets/d/1R1mP6Bk_rplQTWiIapxpgYIezIZWsVI7z-m2up1Ck88/export?format=csv&gid=371699274";
    [MenuItem("Google Drive/Update Character SO")]
    public static void GenerateRecipe()
    {
        WebRequests.Get(_driveURLOfComboSO, (x) => Debug.Log("Error " + x), OnCompleteDownloadingCharacterSO);

    }
   private static void OnCompleteDownloadingCharacterSO(string data)
    { 
            CSVToCardSO.DestroyWebGameObjects();
        var cardsPictures = Resources.LoadAll<Sprite>("Art/CardsPictures");

        var recipeCollection = Resources.Load<Collections.ComboRecipeCollectionSO>("Collection SO/RecipeCollection");
        if (recipeCollection == null || recipeCollection.GetComboSO.Length == 0)
            Debug.LogError("Recipe Collection Is empty make sure you have cards in the Card Collection SO at \"Resources\\Collection SO\\RecipeCollection\"");

        var cardCollections = Resources.Load<CardsCollectionSO>("Collection SO/CardCollection");
        if (cardCollections == null || cardCollections.GetAllCards.Length == 0)
            Debug.LogError("Card Collection Is empty make sure you have cards in the Card Collection SO at \"Resources\\Collection SO\\CardCollection\"");

        string[] rows = data.Replace("\r", "").Split('\n');

        List<Battles.CharacterSO> character = new List<Battles.CharacterSO>();

        for (int i = 1; i < rows.Length; i++)
        {
            string[] line = rows[i].Replace('"', ' ').Replace('/', ' ').Split(',');

            var recipe = Battles.CharacterSO.Init(line, cardCollections,recipeCollection);

            if (recipe == null)
                break;
            else
                character.Add(recipe);
        }
    }


    private static Battles.CharacterSO CreateCharacterSO(string[] row,CardIconCollectionSO cardCollection, Collections.ComboRecipeCollectionSO comboRecipeCollectionSO)
    {
        const int ID = 0;
        if (row[ID] == "-" || row[ID] == "")
            return null;

        Battles.CharacterSO Character = ScriptableObject.CreateInstance<Battles.CharacterSO>();

        AssetDatabase.CreateAsset(comboCollection, $"Assets/Resources/Character SO/RecipeCollection.asset");





        AssetDatabase.SaveAssets();


    }
}
