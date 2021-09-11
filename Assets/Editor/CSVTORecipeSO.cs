using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CSVTORecipeSO
{
    const string _driveURLOfRecipeSO = "https://docs.google.com/spreadsheets/d/1R1mP6Bk_rplQTWiIapxpgYIezIZWsVI7z-m2up1Ck88/export?format=csv&gid=371699274";
    [MenuItem("Google Drive/Update Recipe SO")]
    public static void GenerateRecipe()
    {
        WebRequests.Get(_driveURLOfRecipeSO, (x) => Debug.Log("Error " + x), OnCompleteDownloadingRecipeCSV);
    }
    private static void OnCompleteDownloadingRecipeCSV(string txt)
    {
        CSVToCardSO.DestroyWebGameObjects();

        var cardsPictures = Resources.LoadAll<Sprite>("Art/CardsPictures");

        var cardCollections = Resources.Load<CardsCollectionSO>("Collection SO/CardCollection");
        if (cardCollections == null || cardCollections.GetAllCards.Length == 0)
            Debug.LogError("Card Collection Is empty make sure you have cards in the Card Collection SO at \"Resources\\Collection SO\\CardCollection\"");


        string[] rows = txt.Replace("\r", "").Split('\n');

        Collections.RelicsSO.ComboCollectionSO comboCollection = ScriptableObject.CreateInstance<Collections.RelicsSO.ComboCollectionSO>();
        AssetDatabase.CreateAsset(comboCollection, $"Assets/Resources/Collection SO/RecipeCollection.asset");
        List<Combo.ComboSO> combosRecipe = new List<Combo.ComboSO>();
        for (int i = 0; i < rows.Length; i++)
        {
            string[] line  =rows[i].Replace('"', ' ').Replace('/', ' ').Split(',');

            var recipe = CreateComboRecipe(line, cardCollections);

            if (recipe == null)
                break;
            else
                combosRecipe.Add(recipe);
            
        }

        comboCollection.Init(combosRecipe.ToArray());

        AssetDatabase.SaveAssets();
    }


    private static Combo.ComboSO CreateComboRecipe(string[] row, CardsCollectionSO cardCollection)
    {
        const int ID = 0;
        const int RecipeCardName = 1;
        const int GoesToWhenCrafted = 2;
        const int GoldCost = 3;
        const int BodyPartsAndType = 4;

        if (row[ID] == "-")
            return null;

        Combo.ComboSO recipe = ScriptableObject.CreateInstance<Combo.ComboSO>();

        recipe.

        AssetDatabase.CreateAsset(recipe, $"Assets/Resources/Cards SO/{recipe.GetComboName}Reciie.asset");
        return card;
    }
}