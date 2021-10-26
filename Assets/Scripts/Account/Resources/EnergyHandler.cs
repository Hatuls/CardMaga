namespace Meta.Resources
{
    public class EnergyHandler : ResourceHandler<ushort>
    {
        #region Fields
        ushort _amountToStartPlay = 5;
        #endregion
        #region Properties
        public ushort AmountToStartPlay => _amountToStartPlay;
        #endregion
        public override void AddAmount(ushort amount)
        {
            Account.AccountManager.Instance.AccountGeneralData.AccountEnergyData.Energy.CheckAndAddValue(amount);
        }
        public void AddGiftAmount(ushort amount)
        {
            Account.AccountManager.Instance.AccountGeneralData.AccountEnergyData.Energy.AddValue(amount);
        }

        public override bool HasAmount(ushort amount)
        {
            return Account.AccountManager.Instance.AccountGeneralData.AccountEnergyData.Energy.CheckStat(amount);
        }

        public override void ReduceAmount(ushort amount)
        {
            Account.AccountManager.Instance.AccountGeneralData.AccountEnergyData.Energy.ReduceValue(amount);
        }

        public override ushort Stat(ushort amount)
        {
            return Account.AccountManager.Instance.AccountGeneralData.AccountEnergyData.Energy.Value;
        }
    }
}
