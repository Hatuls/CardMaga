using System;

namespace Cards.Meta
{
    public class UpgradeCardHandler
    {
        #region Fields
        UpgradeChartSO _upgradeChartSO;
        #endregion
        #region Public Methods
        UpgradeCardHandler(UpgradeChartSO upgradeChartSO)
        {

        }
        public ushort GetUpgradeCost(Card card)
        {
            throw new NotImplementedException();
        }
        public bool CanBeUpgrade(Card card)
        {
            throw new NotImplementedException();
        }
        public void LevelUpCard(Card card)
        {

        }
        #endregion
    }
}
