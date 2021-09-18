using UnityEditor;
using System.IO;
using UnityEngine;
using System;
using System.Collections.Generic;

public class CSVToCardSO 
{
    static Keywords.KeywordSO[] _keywordsSO;
    static Sprite[] cardsPictures;
    const string _driveURLOfCardSO = "https://docs.google.com/spreadsheets/d/1R1mP6Bk_rplQTWiIapxpgYIezIZWsVI7z-m2up1Ck88/export?format=csv&gid=1611461659";
     static string[] csv;
   [MenuItem("Google Drive/Update Cards SO")]
    public static void GenerateCards()
    {
        WebRequests.Get(_driveURLOfCardSO, (x) => Debug.Log("Error " + x), OnCompleteDownloadingCardCSV);
    }




    private static void OnCompleteDownloadingCardCSV(string txt)
    {
        DestroyWebGameObjects();

        _keywordsSO = Resources.LoadAll<Keywords.KeywordSO>("KeywordsSO");

        float timer = 0;
        while (_keywordsSO == null && timer < float.MaxValue/2)
        {
            timer += 0.5f;
        }
        if (_keywordsSO == null)
            Debug.LogError("Keywords SO is null!!");

        cardsPictures = Resources.LoadAll<Sprite>("Art/CardsPictures");
        timer = 0;
        while (cardsPictures == null && timer < float.MaxValue / 2)
        {
            timer += 0.5f;
        }
        if (cardsPictures == null)
        Debug.LogError("CardPictures is null!!");


        SeperateFiles(txt);

        csv = null;
    }
    public static void DestroyWebGameObjects()
    {
        var gos = GameObject.FindGameObjectsWithTag("Web");

        for (int i = gos.Length - 1; i >= 0; i--)
            GameObject.DestroyImmediate(gos[i]);
    }

    private static void SeperateFiles(string data)
    {
        
         csv = data.Replace("\r", "").Split('\n');

        var Collection = ScriptableObject.CreateInstance<CardsCollectionSO>();

       
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

        Collection.Init(cardList.ToArray());
        AssetDatabase.CreateAsset(Collection, $"Assets/Resources/Collection SO/CardCollection.asset");
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
        else if(cardSO[isCardUpgrade] != "0")
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
        const int UpgradeToCardID= 23;
        const int IsExhausted = 24;
        

        card.ID = int.Parse(cardSO[ID]);

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
            IsCinemtaic = cardSO[Cinematic] == "1" ,
            //IsSlowMotion = bool.Parse

        };


        // Description
        card.CardDescription = cardSO[CardDescription];

        //Stamina Cost
        //card.StaminaCost = int.TryParse(cardSO[StaminaCost], out int cost) ? cost : -1;
        //if (card.StaminaCost <0)
        //    Debug.LogError($"CardID {cardSO[ID]} : Coulmne R {StaminaCost}  is not an int OR its less than 0");

        //Purchase Cost
        card.PurchaseCost = int.TryParse(cardSO[PurchaseCost], out int pCost) ? pCost : -1;
        if (card.PurchaseCost < 0)
            Debug.LogError($"CardID {cardSO[ID]} : Coulmne U :({PurchaseCost}) Value:({cardSO[PurchaseCost]}) is not an int OR its less than 0");

        //ToExhaust
        card.ToExhaust = cardSO[IsExhausted] == "0" ? false : true;
        card.StaminaCost = int.Parse(cardSO[StaminaCost]);
    
        // id fuses from
        string[] idCrafts = cardSO[IDThatCraftMe].Split('&');
        if (idCrafts[0].Equals('0') == false)
        {
            int[] arr = new int[0];
            foreach (var id in idCrafts)
            {
                if (int.Parse(id) == 0)
                    break;
                Array.Resize(ref arr, arr.Length+1);
                arr[arr.Length - 1] = int.Parse(id);
            }

            card.CardsFusesFrom = arr;
        }




        //Upgrades
        List<Cards.PerLevelUpgrade> _PerLevelUpgrade = new List<Cards.PerLevelUpgrade>();
        _PerLevelUpgrade.Add(new Cards.PerLevelUpgrade(GetCardsUpgrade(card, cardSO, StaminaCost, BodyPart,CardType)));
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
            if (item.Length>0 && item != "-")
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
        
        if (KeywordSoIndex >= 0 && KeywordSoIndex < Enum.GetNames(typeof(Keywords.KeywordTypeEnum)).Length)
        {
            if ((Keywords.KeywordTypeEnum)KeywordSoIndex == Keywords.KeywordTypeEnum.None)
                Debug.LogError("Keyword Type Is not In Range of KeywordTypeEnum");

            if (_keywordsSO.Length == 0)
                Debug.LogError("Keyword So Was Not Loaded From Assets/Resources/KeywordsSO");

            for (int j = 0; j < _keywordsSO.Length; j++)
            {
                if (_keywordsSO[j].GetKeywordType == (Keywords.KeywordTypeEnum)KeywordSoIndex)
                    return _keywordsSO[j];
            }
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
    private static Cards.PerLevelUpgrade.Upgrade[] GetCardsUpgrade(Cards.CardSO card, string[] getRow,int StaminaCost,int BodyPart, int CardType)
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

}
