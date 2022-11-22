using CardMaga.Rewards.Bundles;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [System.Serializable] //for tests
    public class AccountBarVisualData
    {
        #region Fields
        [SerializeField] string _accountNickname; // new user
        [SerializeField] int _accountImageID; // 0
        [SerializeField] int _currentExpAmount; // 0
        [SerializeField] int _maxExpAmount; // 0 
        [SerializeField] int _accountLevel; // 1
        [SerializeField] ResourcesCost _chipsData; // 50
        [SerializeField] ResourcesCost _goldData; // 500
        [SerializeField] ResourcesCost _diamondsData; // 100

        public AccountBarVisualData(string accountNickname, int accountImageID, int currentExpAmount, int maxExpAmount, int accountLevel, ResourcesCost chipsData, ResourcesCost goldData, ResourcesCost diamondsData)
        {
            _accountNickname = accountNickname;
            _accountImageID = accountImageID;
            _currentExpAmount = currentExpAmount;
            _maxExpAmount = maxExpAmount;
            _accountLevel = accountLevel;
            _chipsData = chipsData;
            _goldData = goldData;
            _diamondsData = diamondsData;
        }
        #endregion

        #region Properties
        public string AccountNickname => _accountNickname;
        public int AccountImageID => _accountImageID;
        public int CurrentExpAmount => _currentExpAmount;
        public int MaxExpAmount => _maxExpAmount;
        public int AccountLevel => _accountLevel;
        public ResourcesCost ChipsData => _chipsData;
        public ResourcesCost GoldData => _goldData;
        public ResourcesCost DiamondsData => _diamondsData;

        #endregion
        //Currency Types
    }
}