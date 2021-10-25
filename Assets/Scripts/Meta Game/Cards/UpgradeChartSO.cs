using System;
using UnityEngine;

namespace Cards.Meta
{
    [CreateAssetMenu(fileName = "Upgrade SO", menuName = "ScriptableObjects/MetaGame/Upgrade Chart SO")]
    public class UpgradeChartSO : ScriptableObject
    {
        #region Fields
        ushort[] _cardUpgradesCost;
        #endregion
        #region PublicMethods
        public void InitData(String[] upgradeChartString)
        {
        }
        public ushort GetUpgradeCost(ushort cardLevel)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
