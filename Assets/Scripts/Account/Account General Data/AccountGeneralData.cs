using UnityEngine;
namespace Account.GeneralData
{
    [System.Serializable]
    public class AccountGeneralData : ILoadFirstTime
    {


        public AccountGeneralData()
        {

        }

        #region Fields
        [SerializeField]
        private AccountInfoData _accountInfoData;
        [SerializeField]
        private AccountLevelData _accountLevelData;
        [SerializeField]
        private AccountResourcesData _accountResourcesData;
        [SerializeField]
        private AccountEnergyData _accountEnergyData;
        #endregion


        #region Properties
        public AccountInfoData AccountInfoData { get => _accountInfoData; private set => _accountInfoData = value; }
        public AccountLevelData AccountLevelData { get => _accountLevelData; private set => _accountLevelData = value; }
        public AccountResourcesData AccountResourcesData { get => _accountResourcesData; private set => _accountResourcesData = value; }
        public AccountEnergyData AccountEnergyData { get => _accountEnergyData; private set => _accountEnergyData = value; }

        public void NewLoad()
        {
            //_accountInfoData = new AccountInfoData(TimeManager.Instance.GetCurrentTime(),);



            _accountEnergyData = new AccountEnergyData();
            _accountEnergyData.NewLoad();
            _accountLevelData = new AccountLevelData();
            _accountLevelData.NewLoad();
            _accountResourcesData = new AccountResourcesData();
            _accountResourcesData.NewLoad();
        }
        #endregion
    }
}