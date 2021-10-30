namespace Account.GeneralData
{

    public class AccountGeneralData
    {
        public AccountGeneralData()
        {
            //_accountInfoData = new AccountInfoData(TimeManager.Instance.GetCurrentTime(),);
            _accountEnergyData = new AccountEnergyData(30,1000);
            _accountLevelData = new AccountLevelData(0, 1);
        }
        #region Fields
        private AccountInfoData _accountInfoData;
        private AccountLevelData _accountLevelData;
        private AccountResourcesData _accountResourcesData;
        private AccountEnergyData _accountEnergyData;
        #endregion


        #region Properties
        public AccountInfoData AccountInfoData { get => _accountInfoData; private set => _accountInfoData = value; }
        public AccountLevelData AccountLevelData { get => _accountLevelData; private set => _accountLevelData = value; }
        public AccountResourcesData AccountResourcesData { get => _accountResourcesData; private set => _accountResourcesData = value; }
        public AccountEnergyData AccountEnergyData { get => _accountEnergyData;private set => _accountEnergyData = value; }
        #endregion
    }
}