namespace Meta.Resources
{
    public class EnergyHandler : ResourceHandler<int>
    {
        #region Fields
     
        #endregion
        #region Properties
        public int AmountToStartPlay => DefaultVersion._gameVersion.EnergyToPlay;
        #endregion
        public override void AddAmount(int amount)
        {
            //    Account.AccountManager.Instance.AccountGeneralData.AccountEnergyData.Energy.CheckAndAddValue(amount);
        }
        public void AddGiftAmount(ushort amount)
        {
            //  Account.AccountManager.Instance.AccountGeneralData.AccountEnergyData.Energy.AddValue(amount);
          
        }

        public override bool HasAmount(int amount)
        {
            //   return Account.AccountManager.Instance.AccountGeneralData.AccountEnergyData.Energy.CheckStat(amount);
               return true;
        }

        public override void ReduceAmount(int amount)
        {
            //  Account.AccountManager.Instance.AccountGeneralData.AccountEnergyData.Energy.ReduceValue(amount);
    
        }

        public override int Stat(int amount)
        {
            //   return Account.AccountManager.Instance.AccountGeneralData.AccountEnergyData.Energy.Value;
            return 1;
        }
    }
}
