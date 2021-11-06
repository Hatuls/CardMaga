
using UnityEditor;
using UnityEngine;
using Rewards;
using Collections;
using System.Threading.Tasks;



public class CSVManager
{ 

    public static CharacterCollectionSO _characterCollection;
    public static ComboCollectionSO _comboCollection;
    public static CardsCollectionSO _cardCollection;
    public static Keywords.KeywordsCollectionSO _keywordsSO;
    public static Sprite[] cardsPictures;
    public static BattleRewardCollectionSO _battleRewards;


    #region URL
    const string _driveURL = "https://docs.google.com/spreadsheets/d/1R1mP6Bk_rplQTWiIapxpgYIezIZWsVI7z-m2up1Ck88/export?format=csv&gid=";
    const string _driveURLOfCardSO = "1611461659";
    const string _driveURLOfRecipeSO = "371699274";
    const string _driveURLOfCharacterSO = "945070348";
    const string _driveURLOfKeywordsSO = "116208579";
    const string _driveURLOfBattleRewardSO = "39048757";
    #endregion

    [MenuItem("Google Drive/Update All ScriptableObjects!")]
    public static void Start()
    {
        StartAsync();
    }
    public async static void StartAsync()
    {
        string[] urls = new string[]
        {
        _driveURLOfKeywordsSO,
        _driveURLOfCardSO,
        _driveURLOfRecipeSO,
        _driveURLOfCharacterSO,
        _driveURLOfBattleRewardSO
        };

        CSVAbst[] csvs = new CSVAbst[] {
            new CSVToKeywordsSO(),
            new CSVToCardSO(),
            new CSVTORecipeSO(),
            new CSVToCharacterSO (),
            new CSVToBattleReward()
        };


        for (int i = 0; i < csvs.Length; i++)
        {
            await csvs[i].StartCSV(string.Concat(_driveURL, urls[i]));
            
            DestroyWebGameObjects();
        }
        Debug.Log("Completed Updateing SO From CSV!");
    }

    private static void DestroyWebGameObjects()
    {
        var gos = GameObject.FindGameObjectsWithTag("Web");

        for (int i = gos.Length - 1; i >= 0; i--)
            GameObject.DestroyImmediate(gos[i]);
    }
}

public abstract class CSVAbst
{
    public virtual async Task StartCSV(string data)
    { }
}
