using CardMaga.Animation;
using CardMaga.Card;
using Cards;
using Cinemachine;
using Collections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
namespace CardMaga.CSV
{
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


            List<CardSO> cardList = new List<CardSO>();


            //0 is the headers
            // 1 is example battleCard
            const int firstCardsIndex = 3;
            const int IsBattleReward = 25;

            for (int i = firstCardsIndex; i < csv.GetLength(0); i++)
            {
                string[] row = csv[i].Replace('"', ' ').Replace('/', ' ').Split(',');

                if (row[0] == "-")
                    break;

                CardSO cardCache = CreateCard(row);



                if (cardCache == null)
                    continue;
                else
                    cardList.Add(cardCache);

                
            }

            await Task.Yield();

            CSVManager._cardCollection.Init(cardList.ToArray());
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
           await Task.Yield();
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



            // check if there is coreID to the battleCard if we found '-' it means there is no need to check the rest of the lines
            const int ID = 0;
            if (CheckIfEmpty(cardSO[ID]))
                return null;

            // check if its the base battleCard or if its a upgrade version of a previous battleCard
            const int isCardUpgrade = 1;

            if (cardSO[isCardUpgrade] == "1")
                return null;
            else if (cardSO[isCardUpgrade] != "0")
                Debug.LogError($"CardCoreID {cardSO[ID]} : Coulmne B {isCardUpgrade} value: {cardSO[isCardUpgrade]} is not valid boolean");




            // create battleCard
            CardSO card = ScriptableObject.CreateInstance<CardSO>();

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
            //const int IsRewardType = 25;
            const int CardValueIndex = 25;

            card.ID = ushort.Parse(cardSO[ID]);


            card.CardName = cardSO[CardName];
            card.CardSprite = GetCardImageByName(card.CardName);
            //Keywords
            card.CardSOKeywords = GetKeywordsData(cardSO);



            //Rarity
            if (int.TryParse(cardSO[RarityLevel], out int RarityInt))
                card.Rarity = (RarityEnum)RarityInt;
            else
                Debug.Log($"CardCoreID {cardSO[ID]} : Coulmne S ({RarityLevel}) Rarity is not an int!");


            //Cameras
            CameraDetails cameraDetails = new CameraDetails();
            if (!CheckIfEmpty(cardSO[CameraBlendTypeIndex]))
            {
                CinemachineBlenderSettings blenderSettings = Resources.Load<CinemachineBlenderSettings>($"Camera/Transitions/Battle/{cardSO[CameraBlendTypeIndex]}");
                if (blenderSettings == null)
                    throw new Exception($"CSVToCardSO: Could not find Blender settings from URL in Camera/Transitions/Battle/{cardSO[CameraBlendTypeIndex]}");

                CinemachineBlenderSettings[] cinemachineBlenderSettings = new CinemachineBlenderSettings[] { blenderSettings };
                cameraDetails.CinemachineBlenderSettings = cinemachineBlenderSettings;
            }

            List<CameraIdentification> camerasId = new List<CameraIdentification>();
            string[] charactersCameras = cardSO[CameraNameIndex].Split('^');
            for (int i = 0; i < charactersCameras.Length; i++)
            {
                string[] camerasNames = charactersCameras[i].Split('&');
                for (int j = 0; j < camerasNames.Length; j++)
                {
                    if (CheckIfEmpty(camerasNames[j]))
                        break;

                    CameraIdentification camID = Resources.Load<CameraIdentification>($"Camera/CoreID/Battle/{camerasNames[j]}");
                    if (camID == null)
                        throw new Exception($"CSVToCardSO: Could not find Camera Identification from URL in Camera/CoreID/Battle/{camerasNames[j]}");

                    camerasId.Add(camID);
                }

                if (i == 0)
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
                BodyPartEnum = int.TryParse(cardSO[BodyPart], out int bodyPartIndex) ? (CardMaga.Card.BodyPartEnum)bodyPartIndex : CardMaga.Card.BodyPartEnum.None,
            };


            //Stamina Cost
            card.StaminaCost = byte.Parse(cardSO[StaminaCost]);

            //Purchase Cost



            // coreID fuses from
            string[] idCrafts = cardSO[IDThatCraftMe].Split('&');
            if (idCrafts[0].Equals('0') == false)
            {
                int[] arr = Array.Empty<int>();
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
                Debug.LogError($"CardCoreID {cardSO[ID]} : Coulmne U :({PurchaseCost}) Value:({cardSO[PurchaseCost]}) is not an int OR its less than 0");

            //Upgrade Value
            int cardValue = 0;
            try
            {
                if (!int.TryParse(cardSO[CardValueIndex], out cardValue))
                    Debug.LogError($"CSVTOCARDSO: BattleCard CoreID - {card.ID} : BattleCard Value is not valid!\nInput: {cardSO[CardValueIndex]}");
            }
            catch (Exception)
            {
                throw new Exception($"{cardSO[CardName]}");
            }


            //Description
            List<string[]> description = GetDescription(cardSO[CardDescription]);

            //Upgrades
            List<PerLevelUpgrade> _PerLevelUpgrade = new List<Cards.PerLevelUpgrade>();
            _PerLevelUpgrade.Add(new PerLevelUpgrade(GetCardsUpgrade(card, cardSO, StaminaCost, BodyPart, CardType, IsExhausted), description, cost, cardValue));
            string firstCardId = cardSO[UpgradeToCardID];


            do
            {

                if (int.TryParse(firstCardId, out int myUpgradeVersionID))
                {
                    string[] getRow = GetRowFromCSVByID(myUpgradeVersionID);

                    if (getRow.Length == 0)
                        Debug.LogError($"CoreID {myUpgradeVersionID} has no data in it!");

                    cost = ushort.TryParse(getRow[PurchaseCost], out ushort s) ? s : (ushort)0;
                    if (cost == 0)
                        Debug.LogError($"CardCoreID {cardSO[ID]} : Coulmne U :({PurchaseCost}) Value:({cardSO[PurchaseCost]}) is not an int OR its less than 0");

                    description = GetDescription(getRow[CardDescription]);

                    if (!int.TryParse(getRow[CardValueIndex], out cardValue))
                        Debug.LogError($"CSVTOCARDSO: BattleCard CoreID - {myUpgradeVersionID} : BattleCard Value is not valid!\nInput: {getRow[CardValueIndex]}");

                    _PerLevelUpgrade.Add(new Cards.PerLevelUpgrade(GetCardsUpgrade(card, getRow, StaminaCost, BodyPart, CardType, IsExhausted), description, cost, cardValue));
                    firstCardId = getRow[UpgradeToCardID];
                }
                else
                    break;


            } while (true);
            card.PerLevelUpgrade = _PerLevelUpgrade.ToArray();
            card.CreateCardCoreInfo();

            //string[] rewardType = cardSO[IsRewardType].Split('&');
            //if (byte.TryParse(rewardType[0], out byte isBattleReward))
            //    battleCard.IsBattleReward = isBattleReward == 1;
            //else
            //    throw new Exception($"ComboSo : CoreID {battleCard.CoreID} doesnt have a valid reward type answer (can only accept 1 or 0)\nRecieved {rewardType[0]}");

            //if (byte.TryParse(rewardType[1], out byte isPackReward))
            //    battleCard.IsPackReward = isPackReward == 1;
            //else
            //    throw new Exception($"ComboSo : CoreID {battleCard.CoreID} doesnt have a valid reward type answer (can only accept 1 or 0)\nRecieved {rewardType[1]}");



            AssetDatabase.CreateAsset(card, $"Assets/Resources/Cards SO/{card.CardName}.asset");

            return card;
        }


        private static List<string[]> GetDescription(string description)
        {
            //Sequence Example:
            //15^Stun Shard & 4^Some Keyword&


            // 15^Stun Shard & 4^Some Keyword
            description = description.Remove(description.Length - 1, 1);
            string[] keyword = description.Split('&');
            //15 ^ Stun Shard
            //4 ^ Some Keyword

            int firstCut = keyword.Length;
            List<string[]> finalDescription = new List<string[]>();

            for (int i = 0; i < firstCut; i++)
            {
                string[] numberAndText = keyword[i].Split('^');
                //15
                //Stun Keyword

                //4
                //Some Keyword


                finalDescription.Add(new string[numberAndText.Length]);

                for (int j = 0; j < numberAndText.Length; j++)
                    finalDescription[i][j] = numberAndText[j];
            }

            return finalDescription;
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
        private static Cards.PerLevelUpgrade.Upgrade[] GetCardsUpgrade(CardSO card, string[] getRow, int StaminaCost, int BodyPart, int CardType, int toExhaust)
        {
            List<Cards.PerLevelUpgrade.Upgrade> upgrades = new List<Cards.PerLevelUpgrade.Upgrade>();

            var keywords = GetKeywordsData(getRow);
            int Exhaust = getRow[toExhaust] == "0" ? 0 : 1;
            var stamina = int.TryParse(getRow[StaminaCost], out int sCost) ? sCost : -1;

            upgrades.Add(new Cards.PerLevelUpgrade.Upgrade(new CardTypeData()
            {
                BodyPart = int.TryParse(getRow[BodyPart], out int bodyPartIndex) ? (CardMaga.Card.BodyPartEnum)bodyPartIndex : CardMaga.Card.BodyPartEnum.None,
                CardType = int.TryParse(getRow[CardType], out int cardTypeIndex) ? (CardTypeEnum)cardTypeIndex : CardTypeEnum.None,
            }));

            upgrades.Add(new Cards.PerLevelUpgrade.Upgrade(LevelUpgradeEnum.Stamina, stamina));

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
                    keywordSO = CSVManager._keywordsSO.GetKeywordSO((Keywords.KeywordType)IKeywordsType);


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
                                        Debug.LogError($"CardCoreID {cardSO[0]} : Coulmne K - Amount Is not an integer!");

                                }
                                else
                                    Debug.LogError($"CardCoreID {cardSO[0]} : Coulmne L - Animation Index Is not an integer!");
                            }

                        }
                        else
                            Debug.LogError($"CardCoreID {cardSO[0]} : Coulmne M {STarget[i]}- Target Is not an integer!");
                    }
                    else
                        Debug.LogError($"CardCoreID {cardSO[0]} : Coulmne J {SSeperationAmountKeywords[i]} - Seperation Amount Keywords Is not an integer!");
                }
                else
                    Debug.LogError($"CardCoreID {cardSO[0]} : Coulmne I {SKeywordsType[i]} - KeywordType Is not an integer");

            }

            return keywordsDataList.ToArray();
        }
    }
}