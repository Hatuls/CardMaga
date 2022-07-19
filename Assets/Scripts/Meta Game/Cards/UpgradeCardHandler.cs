using CardMaga.Card;
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
        public ushort GetUpgradeCost(CardData card)
        {
            throw new NotImplementedException();
        }
        public bool CanBeUpgrade(CardData card)
        {
            throw new NotImplementedException();
        }
        public void LevelUpCard(CardData card)
        {

        }
        #endregion
    }
}
