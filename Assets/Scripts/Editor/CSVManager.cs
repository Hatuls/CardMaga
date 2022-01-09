﻿
using Collections;
using Map;
using Rewards;
using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class CSVManager
{
#if UNITY_EDITOR
    public static PackRewardsCollectionSO _packRewardsCollectionSO;
    public static CharacterCollectionSO _characterCollection;
    public static ComboCollectionSO _comboCollection;
    public static CardsCollectionSO _cardCollection;
    public static Keywords.KeywordsCollectionSO _keywordsSO;
    public static Sprite[] cardsPictures;
    public static BattleRewardCollectionSO _battleRewards;
    public static CardUpgradeCostSO _upgradeCardCostSO;
    public static DismentalCostsSO _dismentalCardCostSO;
    public static ActDifficultySO[] actDifficultySOs;
#endif


    #region URL

    #region Battle CSV
    const string _driveURL = "https://docs.google.com/spreadsheets/d/1R1mP6Bk_rplQTWiIapxpgYIezIZWsVI7z-m2up1Ck88/export?format=csv&gid=";

    const string _driveURLOfCardSO = "1611461659";
    const string _driveURLOfRecipeSO = "371699274";
    const string _driveURLOfCharacterSO = "945070348";
    const string _driveURLOfKeywordsSO = "116208579";
    const string _driveURLOfBattleRewardSO = "39048757";
    #endregion


    #region Meta CSV
    const string _driveMetaURL = "https://docs.google.com/spreadsheets/d/11FQ280bkkd9J-UZpHKlLdKnLdoULX4MI3md1trWPArI/export?format=csv&gid=";
    const string _driveURLOfPackRewards = "463836199";
    const string _driveURLOfDismentalAndUpgrades = "26424949";
    const string _driveURLOfDiffculty = "1834560392";
    #endregion

    #endregion
    [MenuItem("Google Drive/Update All ScriptableObjects!")]
    public static void Start()
    {
        Debug.ClearDeveloperConsole();
        System.Console.Clear();
        BattleDataAsync();
    }
    public async static void BattleDataAsync()
    {
        await LoadBattleData();

        Debug.Log("<a>Completed Updateing Battle SO's From CSV!</a>");

        await LoadMetaBattleData();

        Debug.Log("<a>Completed Updateing Meta SO's From CSV!</a>");

        Debug.Log("<a>Completed Updateing SO's From CSV!</a>");
    }



    private async static Task LoadMetaBattleData()
    {
        CSVAbst[] metacsv = new CSVAbst[] {
         new CSVToPackReward(),
         new CSVToUpgradeAndDismental(),
         new CSVToActDiffuclty(),
        };

        string[] metaurls = new string[]
        {
        _driveURLOfPackRewards,
        _driveURLOfDismentalAndUpgrades,
        _driveURLOfDiffculty,
        };

        await StartLoading(_driveMetaURL, metaurls, metacsv);
    }
    private async static Task LoadBattleData()
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

        await StartLoading(_driveURL, urls, csvs);
    }
    private static async Task StartLoading(string MainUrl, string[] urls, CSVAbst[] cSVAbsts)
    {
        for (int i = 0; i < cSVAbsts.Length; i++)
        {
            await cSVAbsts[i].StartCSV(string.Concat(MainUrl, urls[i]));

            DestroyWebGameObjects();
        }
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


public class CSVToActDiffuclty : CSVAbst
{
    public static bool taskFinished = false;
    public async override Task StartCSV(string data)
    {
        taskFinished = false;

        WebRequests.Get(data, (x) => Debug.LogError($"Error On Loading ActDiffculty {x} "), OnCompleteDownloadingActDiffucltyCSV);

        do
        {
            await Task.Yield();
        } while (!taskFinished);
        Debug.Log("Finished Loading Diffulty Acts SO");
    }

    private void OnCompleteDownloadingActDiffucltyCSV(string csv)
    {

        string[] rows = csv.Replace("\r", "").Split('\n');
        int amountOfActs = rows.Length - 1;
        CSVManager.actDifficultySOs = new ActDifficultySO[amountOfActs];

        
        for (int i = 1; i <= amountOfActs; i++)
        {
            CSVManager.actDifficultySOs[i-1] = ScriptableObject.CreateInstance<ActDifficultySO>();
            string[] row = rows[i].Replace('"', ' ').Replace('/', ' ').Split(',');
            CSVManager.actDifficultySOs[i - 1].Init(row);
            AssetDatabase.CreateAsset(CSVManager.actDifficultySOs[i - 1], $"Assets/Resources/Maps/Acts Diffuclty/{CSVManager.actDifficultySOs[i - 1].Act}Difficulty.asset");
        }

        AssetDatabase.SaveAssets();
        taskFinished = true;
    }
}