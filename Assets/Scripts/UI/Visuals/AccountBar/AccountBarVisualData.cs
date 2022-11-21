using CardMaga.Rewards.Bundles;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [System.Serializable] //for tests
    public class AccountBarVisualData
    {
        #region Fields
        [SerializeField] string _accountNickname;
        [SerializeField] int _accountImageID;
        [SerializeField] int _currentExpAmount;
        [SerializeField] int _maxExpAmount;
        [SerializeField] int _accountLevel;
        [SerializeField] ResourcesCost _chipsData;
        [SerializeField] ResourcesCost _goldData;
        [SerializeField] ResourcesCost _diamondsData;
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