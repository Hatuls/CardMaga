using UnityEditor;
using System.IO;
using UnityEngine;
using System;

public class CSVtoSO 
{

    const string _driveLink = "https://docs.google.com/spreadsheets/d/1R1mP6Bk_rplQTWiIapxpgYIezIZWsVI7z-m2up1Ck88/gviz/tq?tqx=out:csv&sheet=1611461659.csv";
    [MenuItem("Google Drive/Update Cards SO")]
    public static void GenerateCards()
    {
        WebRequests.Get(_driveLink, (x) => Debug.Log("Error " + x), OnCompleteDownload);
    }

    private static void OnCompleteDownload(string txt)
    {
      var gos = GameObject.FindGameObjectsWithTag("Web");

        for (int i = gos.Length - 1; i >= 0; i--)
            GameObject.DestroyImmediate(gos[i]);


        SeperateFiles(txt);
    }
    private static void SeperateFiles(string data)
    {
        string[] csv = data.Split('\n');

        for (int i = 2; i < csv.GetLength(0); i++)
            if (!CreateCard(csv[i].Replace('"', ' ').Replace('/', ' ').Split(','))) break;

        AssetDatabase.SaveAssets();
    }
    private static bool CreateCard(string[] cardSO)
    {
        // check if there is id to the card if we found '-' it means there is no need to check the rest of the lines
        if (CheckIfEmpty(cardSO[0]))
            return false;


        Cards.CardSO card = ScriptableObject.CreateInstance<Cards.CardSO>();

        card.ID = int.Parse(cardSO[0]);

        card.CardName = cardSO[1];



        // Card Types
        var cardType= new Cards.CardType()
        {
            //_bodyPart = (Cards.BodyPartEnum)int.Parse(cardSO[2]),
            //_cardType = (Cards.CardTypeEnum)int.Parse(cardSO[3]),
            //_rarityLevel = (Cards.RarityEnum)int.Parse(cardSO[18])


            _bodyPart = (Cards.BodyPartEnum)Enum.Parse(typeof(Cards.BodyPartEnum),cardSO[2]),
            _cardType = (Cards.CardTypeEnum)Enum.Parse(typeof(Cards.CardTypeEnum), cardSO[3]),
            
            _rarityLevel = (CheckIfEmpty(cardSO[18])) ? (Cards.RarityEnum)Enum.Parse(typeof(Cards.RarityEnum), cardSO[18]) : Cards.RarityEnum.Common
        };
        card.CardType = cardType;


        // Animations
        card.AnimationBundle = new Cards.AnimationBundle
        {
            _attackAnimation = (CheckIfEmpty(cardSO[4])) ? Cards.AttackAnimation.None : (Cards.AttackAnimation)Enum.Parse(typeof(Cards.AttackAnimation), cardSO[4]),
            _shieldAnimation = (CheckIfEmpty(cardSO[5])) ? Cards.ShieldAnimation.None : (Cards.ShieldAnimation)Enum.Parse(typeof(Cards.ShieldAnimation), cardSO[5]),
            _getHitAnimation = (CheckIfEmpty(cardSO[6])) ? Cards.GetHitAnimation.None : (Cards.GetHitAnimation)Enum.Parse(typeof(Cards.GetHitAnimation), cardSO[6]),
            IsCinemtaic = CheckIfEmpty(cardSO[19]) ? false : true
            //IsSlowMotion = bool.Parse

        };

        card.CardDescription = cardSO[14];
        Debug.Log(cardSO[15]);
        card.StaminaCost = CheckIfEmpty(cardSO[15]) ? 0: int.Parse(cardSO[15]);
        card.GoToDeckAfterCrafting = (Battles.Deck.DeckEnum)int.Parse(cardSO[20]);
        card.PurchaseCost = int.Parse(cardSO[21]);

        // id fuses from
        string[] idCrafts = cardSO[23].Split('&');
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

        // keywords



       var keywordArr = new Keywords.KeywordData[0];
        var keywordsSO = Resources.LoadAll<Keywords.KeywordSO>("Assets/Resources/KeywordsSO");



        AssetDatabase.CreateAsset(card, $"Assets/Resources/Cards SO/{card.name}.asset");
        return true;
    }

    private static bool CheckIfEmpty(string toCheck) => toCheck.Equals('-');

}
