using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CSVToCharacterSO
{
    const string _driveURLOfCharacterSO = "https://docs.google.com/spreadsheets/d/1R1mP6Bk_rplQTWiIapxpgYIezIZWsVI7z-m2up1Ck88/export?format=csv&gid=945070348";
    [MenuItem("Google Drive/Update Character SO")]
    public static void GenerateRecipe()
    {
        WebRequests.Get(_driveURLOfCharacterSO, (x) => Debug.Log("Error " + x), OnCompleteDownloadingCharacterCSV);
    }
    private static void OnCompleteDownloadingCharacterCSV(string txt)
    {
        var recipeCollection = Resources.Load<Collections.RelicsSO.ComboCollectionSO>("Collection SO/RecipeCollection");
        float timer = 0;
        do
        {
            timer += Time.deltaTime;
        } while (recipeCollection == null && timer < float.MaxValue - 1f);

        var cardCollections = Resources.Load<CardsCollectionSO>("Collection SO/CardCollection");

         timer = 0;
        do
        {
            timer += Time.deltaTime;
        } while (cardCollections == null && timer < float.MaxValue-1f);

        CSVToCardSO.DestroyWebGameObjects();

        string[] rows = txt.Replace("\r", "").Split('\n');

        if (cardCollections == null || cardCollections.GetAllCards.Length == 0)
            Debug.LogError("Card Collection Is empty make sure you have cards in the Card Collection SO at \"Resources\\Collection SO\\CardCollection\"");
        if (recipeCollection == null || recipeCollection.GetComboSO.Length == 0)
            Debug.LogError("Card Collection Is empty make sure you have combos in the recipe Collection SO at \"Resources\\Collection SO\\RecipeCollection\"");

        Battles.CharacterCollection characterCollection = ScriptableObject.CreateInstance<Battles.CharacterCollection>();
List<Battles.CharacterSO> charactersList = new List<Battles.CharacterSO>();

        for (int i = 2; i < rows.Length; i++)
        {
            string[] line = rows[i].Replace('"', ' ').Replace('/', ' ').Split(',');

            var recipe = CreateCharacter(line, cardCollections, recipeCollection);

            if (recipe == null)
                break;
            else
                charactersList.Add(recipe);

        }


        characterCollection.Init(charactersList.ToArray());

        AssetDatabase.CreateAsset(characterCollection, $"Assets/Resources/Collection SO/CharacterCollection.asset");

        AssetDatabase.SaveAssets();
        Debug.Log("Character  Update Complete!");
    }

    private static Battles.CharacterSO CreateCharacter(string[] line, CardsCollectionSO cardCollections, Collections.RelicsSO.ComboCollectionSO recipeCollection)
    {
        const int ID = 0;
        if (int.TryParse(line[ID], out int characterID))
        {
            var character = ScriptableObject.CreateInstance<Battles.CharacterSO>();
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
