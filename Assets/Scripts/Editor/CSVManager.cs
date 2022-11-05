﻿
using Collections;
using Keywords;
using Rewards;
using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
namespace CardMaga.CSV
{

    public class CSVManager
    {
#if UNITY_EDITOR
        public static PackRewardsCollectionSO _packRewardsCollectionSO;
        public static CharacterCollectionSO _characterCollection;
        public static ComboCollectionSO _comboCollection;
        public static CardsCollectionSO _cardCollection;
        public static KeywordsCollectionSO _keywordsSO;
        public static Sprite[] cardsPictures;
        public static BattleRewardCollectionSO _battleRewards;
        public static CardUpgradeCostSO _upgradeCardCostSO;
        public static DismentalCostsSO _dismentalCardCostSO;
#endif


        #region URL

        #region Battle CSV
        const string _driveURL = "https://docs.google.com/spreadsheets/d/1R1mP6Bk_rplQTWiIapxpgYIezIZWsVI7z-m2up1Ck88/export?format=csv&gid=";

        const string _driveURLOfCardSO = "1611461659";
        const string _driveURLOfRecipeSO = "371699274";
        const string _driveURLOfCharacterSO = "945070348";
        const string _driveURLOfKeywordsSO = "116208579";
        const string _driveURLOfBattleRewardSO = "39048757";
        const string _driveURLOfPackRewards = "1123662053";
        #endregion


        #region Meta CSV
        const string _driveMetaURL = "https://docs.google.com/spreadsheets/d/11FQ280bkkd9J-UZpHKlLdKnLdoULX4MI3md1trWPArI/export?format=csv&gid=";
        const string _driveURLOfDismentalAndUpgrades = "26424949";
        const string _driveURLOfDiffculty = "1834560392";
        #endregion

        #endregion
        [MenuItem("Google Drive/Update All ScriptableObjects!")]
        public static void Start()
        {
            ClearConsole();

            BattleDataAsync();
        }

        private static void ClearConsole()
        {
            // This simply does "LogEntries.Clear()" the long way:
            var logEntries = System.Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll");
            var clearMethod = logEntries?.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            clearMethod?.Invoke(null, null);
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
       
         new CSVToUpgradeAndDismental(),
        };

            string[] metaurls = new string[]
            {
     //   _driveURLOfPackRewards,
        _driveURLOfDismentalAndUpgrades,
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
          _driveURLOfPackRewards,
        _driveURLOfBattleRewardSO
            };

            CSVAbst[] csvs = new CSVAbst[] {
            new CSVToKeywordsSO(),
            new CSVToCardSO(),
            new CSVTORecipeSO(),
            new CSVToCharacterSO(),
            new CSVToPackReward(),
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


}