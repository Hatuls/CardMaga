using Cards;
using Cinemachine;
using Collections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class CSVToCardSO : CSVAbst
{
    static bool isCompleted;
    static string[] csv;
    public async override Task StartCSV(string data)
    {
        isCompleted = false;
        WebRequests.Get(data, (x) => Debug.Log("Error " + x), OnCompleteDownloadingCardCSV);
        while (isCompleted == false)
        {
            await Task.Yield();
        }
    }

    private async static void OnCompleteDownloadingCardCSV(string txt)
    {

        await LoadSpritesAndKeywords();

        await SeperateFiles(txt);

        isCompleted = true;

    }

    private async static Task SeperateFiles(string data)
    {
        csv = data.Replace("\r", "").Split('\n');

        CSVManager._cardCollection = ScriptableObject.CreateInstance<CardsCollectionSO>();


        List<Cards.CardSO> cardList = new List<Cards.CardSO>();



        List<ushort> commonList = new List<ushort>(); ;
        List<ushort> unCommonList = new List<ushort>();
        List<ushort> rareList = new List<ushort>();
        List<ushort> epicList = new List<ushort>();
        List<ushort> legendReiList = new List<ushort>();



        //0 is the headers
        // 1 is example card
        const int firstCardsIndex = 2;
        const int IsBattleReward = 25;

        for (int i = firstCardsIndex; i < csv.GetLength(0); i++)
        {
            string[] row = csv[i].Replace('"', ' ').Replace('/', ' ').Split(',');

            if (row[0] == "-")
                break;

            var cardCache = CreateCard(row);



            if (cardCache == null)
                continue;
            else
            {
                cardList.Add(cardCache);

                // battle reward sort

                // is reward card
                if (cardCache.IsBattleReward)
                {

                    switch (cardCache.Rarity)
                    {

                        case RarityEnum.Common:
                            commonList.Add(cardCache.ID);
                            break;
                        case RarityEnum.Uncommon:
                            unCommonList.Add(cardCache.ID);
                            break;
                        case RarityEnum.Rare:
                            rareList.Add(cardCache.ID);
                            break;
                        case RarityEnum.Epic:
                            epicList.Add(cardCache.ID);
                            break;
                        case RarityEnum.LegendREI:
                            legendReiList.Add(cardCache.ID);
                            break;
                        case RarityEnum.None:
                        default:
                            throw new Exception("Rarity card is None or not acceptabl\ncard id" + +cardCache.ID);
                    }

                }
               
            }
        }



        await Task.Yield();
        var _rarity = new CardsCollectionSO.RarityCards[]
        {
        new  CardsCollectionSO.RarityCards(commonList.ToArray(), RarityEnum.Common),
        new  CardsCollectionSO.RarityCards(unCommonList.ToArray(), RarityEnum.Uncommon),
        new  CardsCollectionSO.RarityCards(rareList.ToArray(), RarityEnum.Rare),
        new  CardsCollectionSO.RarityCards(epicList.ToArray(), RarityEnum.Epic),
        new  CardsCollectionSO.RarityCards(legendReiList.ToArray(), RarityEnum.LegendREI),
        };




        CSVManager._cardCollection.Init(cardList.ToArray(), _rarity);
        AssetDatabase.CreateAsset(CSVManager._cardCollection, $"Assets/Resources/Collection SO/CardCollection.asset");
        AssetDatabase.SaveAssets();
        Debug.Log("Cards Updated Completed!");
        isCompleted = true;
    }

    private async static Task LoadSpritesAndKeywords()
    {

        float timeout = 1000000;
        float timer = 0;

        CSVManager.cardsPictures = Resources.LoadAll<Sprite>("Art/CardsPictures");

        while (CSVManager.cardsPictures == null && timer < timeout)
        {
            CSVManager.cardsPictures = Resources.LoadAll<Sprite>("Art/CardsPictures");
            timer += 0.5f;
        }
        if (CSVManager.cardsPictures == null)
            Debug.LogError("CardPictures is null!!");
    }
    private static bool CheckIfEmpty(string toCheck) => toCheck == "-";
    private static CardSO CreateCard(string[] cardSO)
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
        const int CameraBlendTypeIndex = 19;
        const int CameraNameIndex = 20;


        // const int GoToDeckAfterCrafting = 20;
        const int PurchaseCost = 21;
        const int IDThatCraftMe = 22;
        const int UpgradeToCardID = 23;
        const int IsExhausted = 24;
        const int IsRewardType = 25;

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


        //Cameras
        CameraDetails cameraDetails = new CameraDetails();
        if(!CheckIfEmpty(cardSO[CameraBlendTypeIndex]))
        {
            CinemachineBlenderSettings blenderSettings = Resources.Load<CinemachineBlenderSettings>($"Camera/Transitions/Battle/{cardSO[CameraBlendTypeIndex]}");
            if (blenderSettings == null)
                throw new Exception($"CSVToCardSO: Could not find Blender settings from URL in Camera/Transitions/Battle/{cardSO[CameraBlendTypeIndex]}");

            CinemachineBlenderSettings[] cinemachineBlenderSettings = new CinemachineBlenderSettings[] {blenderSettings};
            cameraDetails.CinemachineBlenderSettings = cinemachineBlenderSettings;
        }

        List<CameraIdentification> camerasId = new List<CameraIdentification>();
        string[] charactersCameras = cardSO[CameraNameIndex].Split('^');
        for (int i = 0; i < charactersCameras.Length; i++)
        {
            string[] camerasNames = cardSO[CameraNameIndex].Split('&');
            for (int j = 0; j < camerasNames.Length; j++)
            {
                if (CheckIfEmpty(camerasNames[j]))
                    break;

                CameraIdentification camID= Resources.Load<CameraIdentification>($"Camera/ID/Battle/{camerasNames[j]}");
                if (camID == null)
                    throw new Exception($"CSVToCardSO: Could not find Camera Identification from URL in Camera/ID/Battle/{camerasNames[j]}");

                camerasId.Add(camID);
            }
            if(i==0)
                cameraDetails.LeftCamera = camerasId.ToArray();
            else
                cameraDetails.RightCamera = camerasId.ToArray();
            camerasId.Clear();
        }
        card.CameraDetails = cameraDetails;

        // Animations
        card.AnimationBundle = new AnimationBundle
        {

            AttackAnimation = (CheckIfEmpty(cardSO[AttackAnimation])) ? "" : cardSO[AttackAnimation].Replace(' ', '_'),
            ShieldAnimation = (CheckIfEmpty(cardSO[ShieldAnimation])) ? "" : cardSO[ShieldAnimation].Replace(' ', '_'),
            GetHitAnimation = (CheckIfEmpty(cardSO[GotHitAnimation])) ? "" : cardSO[GotHitAnimation].Replace(' ', '_'),

            //IsSlowMotion = bool.Parse
            BodyPartEnum = int.TryParse(cardSO[BodyPart], out int bodyPartIndex) ? (Cards.BodyPartEnum)bodyPartIndex : Cards.BodyPartEnum.None,
        };


        //Stamina Cost
        card.StaminaCost = byte.Parse(cardSO[StaminaCost]);

        //Purchase Cost



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



        ushort cost = ushort.TryParse(cardSO[PurchaseCost], out ushort pCost) ? pCost : (ushort)0;
        if (cost == 0)
            Debug.LogError($"CardID {cardSO[ID]} : Coulmne U :({PurchaseCost}) Value:({cardSO[PurchaseCost]}) is not an int OR its less than 0");


        //Upgrades
        List<PerLevelUpgrade> _PerLevelUpgrade = new List<Cards.PerLevelUpgrade>();
        _PerLevelUpgrade.Add(new PerLevelUpgrade(GetCardsUpgrade(card, cardSO, StaminaCost, BodyPart, CardType, IsExhausted), cardSO[CardDescription], cost));
        string firstCardId = cardSO[UpgradeToCardID];
        do
        {
            if (int.TryParse(firstCardId, out int myUpgradeVersionID))
            {
                string[] getRow = GetRowFromCSVByID(myUpgradeVersionID);

                if (getRow.Length == 0)
                    Debug.LogError($"ID {myUpgradeVersionID} has no data in it!");

                cost = ushort.TryParse(getRow[PurchaseCost], out ushort s) ? s : (ushort)0;
                if (cost == 0)
                    Debug.LogError($"CardID {cardSO[ID]} : Coulmne U :({PurchaseCost}) Value:({cardSO[PurchaseCost]}) is not an int OR its less than 0");

                _PerLevelUpgrade.Add(new Cards.PerLevelUpgrade(GetCardsUpgrade(card, getRow, StaminaCost, BodyPart, CardType, IsExhausted), getRow[CardDescription], cost));
                firstCardId = getRow[UpgradeToCardID];
            }
            else
                break;


        } while (true);
        card.PerLevelUpgrade = _PerLevelUpgrade.ToArray();


        string[] rewardType = cardSO[IsRewardType].Split('&');
        if (byte.TryParse(rewardType[0], out byte isBattleReward))
            card.IsBattleReward = isBattleReward == 1;
        else
            throw new Exception($"CardSO : ID {card.ID} doesnt have a valid reward type answer (can only accept 1 or 0)\nRecieved {rewardType[0]}");

        if (byte.TryParse(rewardType[1], out byte isPackReward))
            card.IsPackReward = isPackReward == 1;
        else
            throw new Exception($"CardSO : ID {card.ID} doesnt have a valid reward type answer (can only accept 1 or 0)\nRecieved {rewardType[1]}");


        AssetDatabase.CreateAsset(card, $"Assets/Resources/Cards SO/{card.CardName}.asset");

        return card;
    }


    private static Sprite GetCardImageByName(string name)
    {
        foreach (var picture in CSVManager.cardsPictures)
        {
            if (picture.name == name)
                return picture;
        }
        return null;
    }
    private static Cards.PerLevelUpgrade.Upgrade[] GetCardsUpgrade(Cards.CardSO card, string[] getRow, int StaminaCost, int BodyPart, int CardType, int toExhaust)
    {
        List<Cards.PerLevelUpgrade.Upgrade> upgrades = new List<Cards.PerLevelUpgrade.Upgrade>();

        var keywords = GetKeywordsData(getRow);
        int Exhaust = getRow[toExhaust] == "0" ? 0 : 1;
        var stamina = int.TryParse(getRow[StaminaCost], out int sCost) ? sCost : -1;

        upgrades.Add(new Cards.PerLevelUpgrade.Upgrade(new Cards.CardTypeData()
        {
            BodyPart = int.TryParse(getRow[BodyPart], out int bodyPartIndex) ? (Cards.BodyPartEnum)bodyPartIndex : Cards.BodyPartEnum.None,
            CardType = int.TryParse(getRow[CardType], out int cardTypeIndex) ? (Cards.CardTypeEnum)cardTypeIndex : Cards.CardTypeEnum.None,
        }));

        upgrades.Add(new Cards.PerLevelUpgrade.Upgrade(Cards.LevelUpgradeEnum.Stamina, stamina));

        for (int j = 0; j < keywords.Length; j++)
            upgrades.Add(new Cards.PerLevelUpgrade.Upgrade(keywords[j]));

        upgrades.Add(new PerLevelUpgrade.Upgrade(LevelUpgradeEnum.ToRemoveExhaust, Exhaust));


        return upgrades.ToArray();
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
    private static Keywords.KeywordData[] GetKeywordsData(string[] cardSO)
    {
        /*
         * on each keywords type we check how much we need to create keyworddata from this type
         * and on this instance we insert based on the index his amount and animation index
         * 
         */



        const int KeywordType = 8;
        const int AmountOfTheSameKeywords = 9;
        const int Amount = 10;
        const int Target = 12;
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
                keywordSO = CSVManager._keywordsSO.GetKeywordSO((Keywords.KeywordTypeEnum)IKeywordsType);


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
                                if (int.TryParse(SAmountIndexPerKeyword[1], out int amount))
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
}
