
using CardMaga.Rewards.Factory.Handlers;
using Collections;
using Keywords;
using Rewards;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
namespace CardMaga.CSV
{

    public class CSVManager
    {
        public static CharacterCollectionSO _characterCollection;
        public static ComboCollectionSO _comboCollection;
        public static CardsCollectionSO _cardCollection;
        public static KeywordsCollectionSO _keywordsSO;
        public static Sprite[] cardsPictures;
        private static RewardFactoryManagerSO _rewardFactoryManager;
        public static CardUpgradeCostSO _upgradeCardCostSO;
        public static DismentalCostsSO _dismentalCardCostSO;



        #region URL

        #region Battle CSV
        const string _driveURL = "https://docs.google.com/spreadsheets/d/1R1mP6Bk_rplQTWiIapxpgYIezIZWsVI7z-m2up1Ck88/export?format=csv&gid=";

        const string _driveURLOfCardSO = "1611461659";
        const string _driveURLOfRecipeSO = "371699274";
        const string _driveURLOfCharacterSO = "945070348";
        const string _driveURLOfKeywordsSO = "116208579";
        const string _driveURLOfPackRewards = "1123662053";
        #endregion


        #region Meta CSV
        const string _driveMetaURL = "https://docs.google.com/spreadsheets/d/11FQ280bkkd9J-UZpHKlLdKnLdoULX4MI3md1trWPArI/export?format=csv&gid=";
        const string _driveURLOfDismentalAndUpgrades = "26424949";
        const string _driveURLOfCurrenciesRewardsSO = "1812136271";
        const string _driveURLOfCardsRewardsSO = "463836199";
        const string _driveURLOfCharacterRewards = "1437334102";
        const string _driveURLOfGiftsRewards = "665526610";
        const string _driveURLOfBundlesRewards = "2119557023";

        public static RewardFactoryManagerSO RewardFactoryManager 
        { 
            get {
                if (_rewardFactoryManager == null)
                {
                    _rewardFactoryManager = ScriptableObject.CreateInstance<RewardFactoryManagerSO>();
                    AssetDatabase.CreateAsset(_rewardFactoryManager, $"Assets/Resources/Rewards/Factories/Handlers/RewardManagerFactory.asset");
                    AssetDatabase.SaveAssets();
                }

                return _rewardFactoryManager; 
            }
        }
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
                new CSVToCardsPackReward(),
            new CSVToCurrenciesReward(),
            new CSVToCharacterReward(),
            new CSVToGiftReward(),
            new CSVToBundleReward(),
         new CSVToUpgradeAndDismental(),
        };

            string[] metaurls = new string[]
            {
                _driveURLOfCardsRewardsSO,
                _driveURLOfCurrenciesRewardsSO,
                _driveURLOfCharacterRewards,
                _driveURLOfGiftsRewards,
                _driveURLOfBundlesRewards,
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
            };

            CSVAbst[] csvs = new CSVAbst[] {
            new CSVToKeywordsSO(),
            new CSVToCardSO(),
            new CSVTORecipeSO(),
            new CSVToCharacterSO(),
            new CSVToPackRewardData(),
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
        { await Task.Yield(); }
    }


}