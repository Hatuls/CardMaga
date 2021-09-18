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
        CardsCollectionSO cardCollections = Resources.Load<CardsCollectionSO>("Collection SO/CardCollection"); ;

        float timer = 0;
        do
        {
            cardCollections= Resources.Load<CardsCollectionSO>("Collection SO/CardCollection");
            timer += 1f;
        } while (cardCollections == null && timer < 1000000f);


        CSVToCardSO.DestroyWebGameObjects();


        string[] rows = txt.Replace("\r", "").Split('\n');

        if (cardCollections == null || cardCollections.GetAllCards.Length == 0)
            Debug.LogError("Card Collection Is empty make sure you have cards in the Card Collection SO at \"Resources\\Collection SO\\CardCollection\"");

        Collections.RelicsSO.ComboCollectionSO comboCollection = ScriptableObject.CreateInstance<Collections.RelicsSO.ComboCollectionSO>();
      
        List<Combo.ComboSO> combosRecipe = new List<Combo.ComboSO>();

        for (int i = 1; i < rows.Length; i++)
        {
            string[] line  =rows[i].Replace('"', ' ').Replace('/', ' ').Split(',');

            var recipe = CreateComboRecipe(line, cardCollections);

            if (recipe == null )
                break;
            else
                combosRecipe.Add(recipe);
            
        }

        comboCollection.Init(combosRecipe.ToArray());
        AssetDatabase.CreateAsset(comboCollection, $"Assets/Resources/Collection SO/RecipeCollection.asset");
        AssetDatabase.SaveAssets();
     
        Debug.Log("Recipe Update Complete!");
    }


    private static Combo.ComboSO CreateComboRecipe(string[] row, CardsCollectionSO cardCollection)
    {
        const int ID = 0;
        const int RecipeCardName = 1; // <- return ID
        const int GoesToWhenCrafted = 2;
        const int GoldCost = 3;
        const int AmountBodyPartsAndType = 4;
        const int BodyPartsAndType = 5;

        if (row[ID] == "-")
            return null;

        Combo.ComboSO recipe = ScriptableObject.CreateInstance<Combo.ComboSO>();
        recipe.ID = int.Parse(row[ID]);


        // crafted card + recipe name + recipe Image
        if (int.TryParse(row[RecipeCardName], out int craftedCardsID))
        {
            foreach (var card in cardCollection.GetAllCards)
            {
                if (card.ID == craftedCardsID)
                {
                    recipe.CraftedCard = card;
                    recipe.ComboName = card.CardName + " Recipe";
                    recipe.Image = card.CardSprite;
                    break;
                }
            }
            if (recipe.CraftedCard == null)
            {
                Debug.LogError($"Could Not find the ID {row[RecipeCardName]} in the card collection please check if its matching correctly");
                return null;
            }

        }
        else
        {
            Debug.LogError($"RecipeCardName is not an valid int! -> {row[RecipeCardName]}");
            return null;
        }
        //desination
        if (int.TryParse(row[GoesToWhenCrafted], out int locationInt))
            recipe.GoToDeckAfterCrafting = (Battles.Deck.DeckEnum)locationInt;
        else
        {
            Debug.LogError($"Coulmne C Row {row[ID]} - Goes to when crafted is not a valid int!");
            return null;
        }


        //gold
        if (int.TryParse(row[GoldCost], out int cost))
        {
            if (cost < 0)
            {
                cost = 0;
                Debug.LogWarning($"<a>Warning</a> Recipe Cost is below 0! ");
            }
            recipe.Cost = cost;
        }
        else
        {
            Debug.LogError($"Cost Was not an int : {row[GoldCost]}");
            return null;
        }




        //body parts
        if (int.TryParse(row[AmountBodyPartsAndType], out int bodyPartAmount))
        {
            const int bodyPartIndex = 0;
            const int cardTypeIndex = 1;

            string[] bodyPartsAndType = row[BodyPartsAndType].Split('&');
            recipe.ComboSequance = new Cards.CardTypeData[bodyPartAmount];
            for (int i = 0; i < bodyPartAmount; i++)
            {
                string[] bodyPartAndTypeSeperation = bodyPartsAndType[i].Split('^');

                recipe.ComboSequance[i] = new Cards.CardTypeData()
                {
                    BodyPart = int.TryParse(bodyPartAndTypeSeperation[bodyPartIndex], out int b) ? (Cards.BodyPartEnum)b : Cards.BodyPartEnum.None,
                    CardType = int.TryParse(bodyPartAndTypeSeperation[cardTypeIndex], out int t) ? (Cards.CardTypeEnum)t : Cards.CardTypeEnum.None,
                };
            }
        }
        else
        {
            Debug.LogError($"Coulmne E Row {recipe.ID} is not an intiger!");
            return null;
        }







        AssetDatabase.CreateAsset(recipe, $"Assets/Resources/Recipe SO/{recipe.ComboName}.asset");
        return recipe;
    }






}