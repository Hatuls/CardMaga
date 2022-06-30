using Battle.Combo;
using Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;



public class CSVTORecipeSO : CSVAbst
{



    static List<ComboSO> _rewardedCombos;

    static bool isCompleted;
    public async  override Task StartCSV(string data)
    {
        _rewardedCombos = new List<ComboSO>();
        isCompleted = false;
        WebRequests.Get(data, (x) => Debug.LogError($"Error On Loading Recipes {x} "), OnCompleteDownloadingRecipeCSV);
        while (isCompleted == false)
        {
            await Task.Yield();
        }
        _rewardedCombos = null;
        await Task.Yield();
    }

    private static async void OnCompleteDownloadingRecipeCSV(string txt)
    {

        string[] rows = txt.Replace("\r", "").Split('\n');

        if (CSVManager._cardCollection == null || CSVManager._cardCollection.GetAllCards.Length == 0)
            Debug.LogError("Card Collection Is empty make sure you have cards in the Card Collection SO at \"Resources\\Collection SO\\CardCollection\"");

        CSVManager._comboCollection = ScriptableObject.CreateInstance<ComboCollectionSO>();

        List<ComboSO> combosRecipe = new List<ComboSO>();
        const int frameRate = 5;
        for (int i = 1; i < rows.Length; i++)
        {
            string[] line = rows[i].Replace('"', ' ').Replace('/', ' ').Split(',');

            var recipe = CreateComboRecipe(line, CSVManager._cardCollection);

            if (recipe == null)
                break;
            else
           
                combosRecipe.Add(recipe);
            if (i % frameRate == 0)
                await Task.Yield();

        }

        CSVManager._comboCollection.Init(combosRecipe.ToArray(), _rewardedCombos.ToArray());
        AssetDatabase.CreateAsset(CSVManager._comboCollection, $"Assets/Resources/Collection SO/RecipeCollection.asset");
        AssetDatabase.SaveAssets();

        Debug.Log("Recipe Update Complete!");

        isCompleted = true;

    }


    private static ComboSO CreateComboRecipe(string[] row, CardsCollectionSO cardCollection)
    {
        const int ID = 0;
        const int RecipeCardName = 1; // <- return ID
        const int GoesToWhenCrafted = 2;
        const int GoldCost = 3;
        const int AmountBodyPartsAndType = 4;
        const int BodyPartsAndType = 5;
        const int IsBattleRewarded = 6;

        if (row[ID] == "-")
            return null;

        ComboSO recipe = ScriptableObject.CreateInstance<ComboSO>();
        recipe.ID = ushort.Parse(row[ID]);


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
            recipe.GoToDeckAfterCrafting = (Battle.Deck.DeckEnum)locationInt;
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
            recipe.ComboSequence = new Cards.CardTypeData[bodyPartAmount];
            for (int i = 0; i < bodyPartAmount; i++)
            {
                string[] bodyPartAndTypeSeperation = bodyPartsAndType[i].Split('^');

                recipe.ComboSequence[i] = new Cards.CardTypeData()
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


        if (int.TryParse(row[IsBattleRewarded], out int outcome))
        {
            if (outcome == 1)
                _rewardedCombos.Add(recipe);
        }



        AssetDatabase.CreateAsset(recipe, $"Assets/Resources/Recipe SO/{recipe.ComboName}.asset");
        return recipe;
    }

}