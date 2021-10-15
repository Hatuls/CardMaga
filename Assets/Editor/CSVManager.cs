using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CSVManager
{
    static Battles.CharacterCollection _characterCollection;
    static Collections.RelicsSO.ComboCollectionSO _comboCollection;
    static CardsCollectionSO _cardCollection;
    static Keywords.KeywordSO[] _keywordsSO;
    static Sprite[] cardsPictures;

    const string _driveURLOfCardSO = "https://docs.google.com/spreadsheets/d/1R1mP6Bk_rplQTWiIapxpgYIezIZWsVI7z-m2up1Ck88/export?format=csv&gid=1611461659";
    const string _driveURLOfRecipeSO = "https://docs.google.com/spreadsheets/d/1R1mP6Bk_rplQTWiIapxpgYIezIZWsVI7z-m2up1Ck88/export?format=csv&gid=371699274";
    const string _driveURLOfCharacterSO = "https://docs.google.com/spreadsheets/d/1R1mP6Bk_rplQTWiIapxpgYIezIZWsVI7z-m2up1Ck88/export?format=csv&gid=945070348";

    static string[] csv;
    [MenuItem("Google Drive/Update All")]
    public static void Update()
    {
      
        WebRequests.Get(_driveURLOfCardSO, (x) => Debug.Log("Error " + x), OnCompleteDownloadingCardCSV);
    }
    private static void OnCompleteDownloadingCardCSV(string txt)
    {
        DestroyWebGameObjects();

        _keywordsSO = Resources.LoadAll<Keywords.KeywordSO>("KeywordsSO");
        float timeout = 1000000;
        float timer = 0;
        while (_keywordsSO == null && timer < timeout)
        {
            _keywordsSO = Resources.LoadAll<Keywords.KeywordSO>("KeywordsSO");
            timer += 0.5f;
        }
        if (_keywordsSO == null)
            Debug.LogError("Keywords SO is null!!");

        cardsPictures = Resources.LoadAll<Sprite>("Art/CardsPictures");
        timer = 0;
        while (cardsPictures == null && timer < timeout)
        {
            cardsPictures = Resources.LoadAll<Sprite>("Art/CardsPictures");
            timer += 0.5f;
        }
        if (cardsPictures == null)
            Debug.LogError("CardPictures is null!!");

        SeperateFiles(txt);

        csv = null;


        WebRequests.Get(_driveURLOfRecipeSO, (x) => Debug.Log("Error " + x), OnCompleteDownloadingRecipeCSV);

    }
    private static void OnCompleteDownloadingRecipeCSV(string txt)
    {
    
        CSVToCardSO.DestroyWebGameObjects();

  



        string[] rows = txt.Replace("\r", "").Split('\n');

        if (_cardCollection == null || _cardCollection.GetAllCards.Length == 0)
            Debug.LogError("Card Collection Is empty make sure you have cards in the Card Collection SO at \"Resources\\Collection SO\\CardCollection\"");

        _comboCollection = ScriptableObject.CreateInstance<Collections.RelicsSO.ComboCollectionSO>();

        List<Combo.ComboSO> combosRecipe = new List<Combo.ComboSO>();

        for (int i = 1; i < rows.Length; i++)
        {
            string[] line = rows[i].Replace('"', ' ').Replace('/', ' ').Split(',');

            var recipe = CreateComboRecipe(line, _cardCollection);

            if (recipe == null)
                break;
            else
                combosRecipe.Add(recipe);

        }

        _comboCollection.Init(combosRecipe.ToArray());
        AssetDatabase.CreateAsset(_comboCollection, $"Assets/Resources/Collection SO/RecipeCollection.asset");
        AssetDatabase.SaveAssets();

        Debug.Log("Recipe Update Complete!");

        WebRequests.Get(_driveURLOfCharacterSO, (x) => Debug.Log("Error " + x), OnCompleteDownloadingCharacterCSV);

    }
    private static void OnCompleteDownloadingCharacterCSV(string txt)
    {
         CSVToCardSO.DestroyWebGameObjects();

     

        string[] rows = txt.Replace("\r", "").Split('\n');

        if (_cardCollection == null || _cardCollection.GetAllCards.Length == 0)
            Debug.LogError("Card Collection Is empty make sure you have cards in the Card Collection SO at \"Resources\\Collection SO\\CardCollection\"");
        if (_comboCollection == null || _comboCollection.GetComboSO.Length == 0)
            Debug.LogError("Card Collection Is empty make sure you have combos in the recipe Collection SO at \"Resources\\Collection SO\\RecipeCollection\"");

        _characterCollection = ScriptableObject.CreateInstance<Battles.CharacterCollection>();
        List<Battles.CharacterSO> charactersList = new List<Battles.CharacterSO>();

        for (int i = 2; i < rows.Length; i++)
        {
            string[] line = rows[i].Replace('"', ' ').Replace('/', ' ').Split(',');

            var recipe = CreateCharacter(line, _cardCollection, _comboCollection);

            if (recipe == null)
                break;
            else
                charactersList.Add(recipe);

        }


        _characterCollection.Init(charactersList.ToArray());

        AssetDatabase.CreateAsset(_characterCollection, $"Assets/Resources/Collection SO/CharacterCollection.asset");

        AssetDatabase.SaveAssets();
        Debug.Log("Character  Update Complete!");
    }

    #region cards
    private static void SeperateFiles(string data)
    {

        csv = data.Replace("\r", "").Split('\n');

         _cardCollection = ScriptableObject.CreateInstance<CardsCollectionSO>();


        List<Cards.CardSO> cardList = new List<Cards.CardSO>();


        //0 is the headers
        // 1 is example card
        const int firstCardsIndex = 2;

        for (int i = firstCardsIndex; i < csv.GetLength(0); i++)
        {
            string[] row = csv[i].Replace('"', ' ').Replace('/', ' ').Split(',');

            if (row[0] == "-")
                break;

            var cardCache = CreateCard(row);
            if (cardCache == null)
                continue;
            else
                cardList.Add(cardCache);
        }

        _cardCollection.Init(cardList.ToArray());
        AssetDatabase.CreateAsset(_cardCollection, $"Assets/Resources/Collection SO/CardCollection.asset");
        AssetDatabase.SaveAssets();
        Debug.Log("Cards Updated Completed!");
    }
    private static Cards.CardSO CreateCard(string[] cardSO)
    {
        //string input ="";

        //foreach (var item in cardSO)
        //    input += " " + item;

        //Debug.Log(input);



        // check if there is id to the card if we found '-' it means there is no need to check the rest of the lines
        const int ID = 0;
        if (CheckIfEmpty(cardSO[ID]))
            return null;

        // check if its the base card or if its a upgrade version of a previous card
        const int isCardUpgrade = 1;

        if (cardSO[isCardUpgrade] == "1")
            return null;
        else if (cardSO[isCardUpgrade] != "0")
            Debug.LogError($"CardID {cardSO[ID]} : Coulmne B {isCardUpgrade} value: {cardSO[isCardUpgrade]} is not valid boolean");




        // create card
        Cards.CardSO card = ScriptableObject.CreateInstance<Cards.CardSO>();

        // add indexes with name to the fields params
        const int CardName = 2;
        const int BodyPart = 3;
        const int CardType = 4;
        const int AttackAnimation = 5;
        const int ShieldAnimation = 6;
        const int GotHitAnimation = 7;



        const int CardDescription = 16;
        const int StaminaCost = 17;


        const int RarityLevel = 18;
        const int Cinematic = 19;
        // const int GoToDeckAfterCrafting = 20;
        const int PurchaseCost = 20;
        const int IDThatCraftMe = 22;
        const int UpgradeToCardID = 23;
        const int IsExhausted = 24;


        card.ID = ushort.Parse(cardSO[ID]);

        card.CardName = cardSO[CardName];
        card.CardSprite = GetCardImageByName(card.CardName);
        //Keywords
        card.CardSOKeywords = GetKeywordsData(cardSO);



        //Rarity
        if (int.TryParse(cardSO[RarityLevel], out int RarityInt))
            card.Rarity = (Cards.RarityEnum)RarityInt;
        else
            Debug.Log($"CardID {cardSO[ID]} : Coulmne S ({RarityLevel}) Rarity is not an int!");





        // Animations
        card.AnimationBundle = new Cards.AnimationBundle
        {
            _attackAnimation = (CheckIfEmpty(cardSO[AttackAnimation])) ? Cards.AttackAnimation.None : (Cards.AttackAnimation)Enum.Parse(typeof(Cards.AttackAnimation), cardSO[AttackAnimation].Replace(' ', '_')),
            _shieldAnimation = (CheckIfEmpty(cardSO[ShieldAnimation])) ? Cards.ShieldAnimation.None : (Cards.ShieldAnimation)Enum.Parse(typeof(Cards.ShieldAnimation), cardSO[ShieldAnimation].Replace(' ', '_')),
            _getHitAnimation = (CheckIfEmpty(cardSO[GotHitAnimation])) ? Cards.GetHitAnimation.None : (Cards.GetHitAnimation)Enum.Parse(typeof(Cards.GetHitAnimation), cardSO[GotHitAnimation].Replace(' ', '_')),
            IsCinemtaic = cardSO[Cinematic] == "1",
            //IsSlowMotion = bool.Parse
            BodyPartEnum = int.TryParse(cardSO[BodyPart], out int bodyPartIndex) ? (Cards.BodyPartEnum)bodyPartIndex : Cards.BodyPartEnum.None,

        };


        // Description
        card.CardDescription = cardSO[CardDescription];

        //Stamina Cost
        //card.StaminaCost = int.TryParse(cardSO[StaminaCost], out int cost) ? cost : -1;
        //if (card.StaminaCost <0)
        //    Debug.LogError($"CardID {cardSO[ID]} : Coulmne R {StaminaCost}  is not an int OR its less than 0");

        //Purchase Cost
        card.PurchaseCost = ushort.TryParse(cardSO[PurchaseCost], out ushort pCost) ? pCost : (ushort)0;
        if (card.PurchaseCost == 0)
            Debug.LogError($"CardID {cardSO[ID]} : Coulmne U :({PurchaseCost}) Value:({cardSO[PurchaseCost]}) is not an int OR its less than 0");

        //ToExhaust
        card.ToExhaust = cardSO[IsExhausted] == "0" ? false : true;
        card.StaminaCost = byte.Parse(cardSO[StaminaCost]);

        // id fuses from
        string[] idCrafts = cardSO[IDThatCraftMe].Split('&');
        if (idCrafts[0].Equals('0') == false)
        {
            ushort[] arr = new ushort[0];
            foreach (var id in idCrafts)
            {
                if (int.Parse(id) == 0)
                    break;
                Array.Resize(ref arr, arr.Length + 1);
                arr[arr.Length - 1] = ushort.Parse(id);
            }

            card.CardsFusesFrom = arr;
        }




        //Upgrades
        List<Cards.PerLevelUpgrade> _PerLevelUpgrade = new List<Cards.PerLevelUpgrade>();
        _PerLevelUpgrade.Add(new Cards.PerLevelUpgrade(GetCardsUpgrade(card, cardSO, StaminaCost, BodyPart, CardType)));
        string firstCardId = cardSO[UpgradeToCardID];
        do
        {
            if (int.TryParse(firstCardId, out int myUpgradeVersionID))
            {
                string[] getRow = GetRowFromCSVByID(myUpgradeVersionID);

                if (getRow.Length == 0)
                    Debug.LogError($"ID {myUpgradeVersionID} has no data in it!");

                _PerLevelUpgrade.Add(new Cards.PerLevelUpgrade(GetCardsUpgrade(card, getRow, StaminaCost, BodyPart, CardType)));
                firstCardId = getRow[UpgradeToCardID];
            }
            else
                break;


        } while (true);
        card.PerLevelUpgrade = _PerLevelUpgrade.ToArray();





        AssetDatabase.CreateAsset(card, $"Assets/Resources/Cards SO/{card.CardName}.asset");

        return card;
    }
    private static bool CheckIfEmpty(string toCheck) => toCheck == "-";
    private static Keywords.KeywordData[] GetKeywordsData(string[] cardSO)
    {
        /*
         * on each keywords type we check how much we need to create keyworddata from this type
         * and on this instance we insert based on the index his amount and animation index
         * 
         */



        const int KeywordType = 8;
        const int AmountOfTheSameKeywords = 9;
        const int Target = 12;
        const int Amount = 10;
        const int AnimationIndex = 11;

        List<Keywords.KeywordData> keywordsDataList = new List<Keywords.KeywordData>();

        string[] SKeywordsType = cardSO[KeywordType].Split('&');
        string[] SSeperationAmountKeywords = cardSO[AmountOfTheSameKeywords].Split('&');
        string[] SAmount = cardSO[Amount].Trim().Split('&');
        string[] SAnimationIndex = cardSO[AnimationIndex].Split('&');
        string[] STarget = cardSO[Target].Split('&');

        int IKeywordsType;
        int ISeperationAmountKeywords;
        int ITarget;
        Keywords.KeywordSO keywordSO;
        //check skeywords length
        int KeywordsAmount = 0;
        foreach (var item in SKeywordsType)
        {
            if (item.Length > 0 && item != "-")
                KeywordsAmount++;
        }

        for (int i = 0; i < KeywordsAmount; i++)
        {

            if (int.TryParse(SKeywordsType[i], out IKeywordsType))
            {
                if (IKeywordsType == 0)
                    continue;

                keywordSO = KeywordSOFromIndex(IKeywordsType);

                if (int.TryParse(SSeperationAmountKeywords[i], out ISeperationAmountKeywords))
                {
                    if (ISeperationAmountKeywords == 0)
                        continue;

                    if (int.TryParse(STarget[i], out ITarget))
                    {
                        string[] SAnimationIndexPerKeyword = SAnimationIndex[i].Split('^');
                        string[] SAmountIndexPerKeyword = SAmount[i].Split('^');

                        Keywords.KeywordData keywordDataCache;


                        for (int j = 0; j < ISeperationAmountKeywords; j++)
                        {
                            if (SAnimationIndexPerKeyword[j] == "-")
                                continue;


                            if (int.TryParse(SAnimationIndexPerKeyword[j], out int animationIndex))
                            {
                                if (int.TryParse(SAmountIndexPerKeyword[j], out int amount))
                                {
                                    keywordDataCache = new Keywords.KeywordData(
                                        keywordSO,
                                        (Keywords.TargetEnum)ITarget,
                                        amount,
                                        animationIndex
                                        );

                                    keywordsDataList.Add(keywordDataCache);
                                }
                                else
                                    Debug.LogError($"CardID {cardSO[0]} : Coulmne K - Amount Is not an integer!");

                            }
                            else
                                Debug.LogError($"CardID {cardSO[0]} : Coulmne L - Animation Index Is not an integer!");
                        }

                    }
                    else
                        Debug.LogError($"CardID {cardSO[0]} : Coulmne M {STarget[i]}- Target Is not an integer!");
                }
                else
                    Debug.LogError($"CardID {cardSO[0]} : Coulmne J {SSeperationAmountKeywords[i]} - Seperation Amount Keywords Is not an integer!");
            }
            else
                Debug.LogError($"CardID {cardSO[0]} : Coulmne I {SKeywordsType[i]} - KeywordType Is not an integer");

        }

        return keywordsDataList.ToArray();
    }
    private static Keywords.KeywordSO KeywordSOFromIndex(int KeywordSoIndex)
    {
        // checking if its valid enum and then check if the keyword so that were loadded has this enum in it


        if ((Keywords.KeywordTypeEnum)KeywordSoIndex == Keywords.KeywordTypeEnum.None)
            Debug.LogError("Keyword Type Is not In Range of KeywordTypeEnum");

        if (_keywordsSO.Length == 0)
            Debug.LogError("Keyword So Was Not Loaded From Assets/Resources/KeywordsSO");

        for (int j = 0; j < _keywordsSO.Length; j++)
        {
            if (_keywordsSO[j].GetKeywordType == (Keywords.KeywordTypeEnum)KeywordSoIndex)
                return _keywordsSO[j];
        }


        return null;
    }
    private static string[] GetRowFromCSVByID(int id)
    {
        for (int i = 0; i < csv.GetLength(0); i++)
        {
            string[] row = csv[i].Replace('"', ' ').Replace('/', ' ').Split(',');
            if (int.TryParse(row[0], out int ID))
            {
                if (id == ID)
                    return row;
            }
        }
        return null;
    }
    private static Cards.PerLevelUpgrade.Upgrade[] GetCardsUpgrade(Cards.CardSO card, string[] getRow, int StaminaCost, int BodyPart, int CardType)
    {
        List<Cards.PerLevelUpgrade.Upgrade> upgrades = new List<Cards.PerLevelUpgrade.Upgrade>();

        var keywords = GetKeywordsData(getRow);

        var stamina = int.TryParse(getRow[StaminaCost], out int sCost) ? sCost : -1;

        upgrades.Add(new Cards.PerLevelUpgrade.Upgrade(new Cards.CardTypeData()
        {
            BodyPart = int.TryParse(getRow[BodyPart], out int bodyPartIndex) ? (Cards.BodyPartEnum)bodyPartIndex : Cards.BodyPartEnum.None,
            CardType = int.TryParse(getRow[CardType], out int cardTypeIndex) ? (Cards.CardTypeEnum)cardTypeIndex : Cards.CardTypeEnum.None,
        }));

        upgrades.Add(new Cards.PerLevelUpgrade.Upgrade(Cards.LevelUpgradeEnum.Stamina, stamina));

        for (int j = 0; j < keywords.Length; j++)
            upgrades.Add(new Cards.PerLevelUpgrade.Upgrade(keywords[j]));




        return upgrades.ToArray();
    }
    private static Sprite GetCardImageByName(string name)
    {
        foreach (var picture in cardsPictures)
        {
            if (picture.name == name)
                return picture;
        }
        return null;
    }
    private static void DestroyWebGameObjects()
    {
        var gos = GameObject.FindGameObjectsWithTag("Web");

        for (int i = gos.Length - 1; i >= 0; i--)
            GameObject.DestroyImmediate(gos[i]);
    }
    #endregion

    #region Recipes 


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






    #endregion 

    #region Characters

    private static Battles.CharacterSO CreateCharacter(string[] line, CardsCollectionSO cardCollections, Collections.RelicsSO.ComboCollectionSO recipeCollection)
    {
        const int ID = 0;
        if (ushort.TryParse(line[ID], out ushort characterID))
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
    #endregion
}

