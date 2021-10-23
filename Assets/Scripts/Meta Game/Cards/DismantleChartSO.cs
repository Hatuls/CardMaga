using System;
using UnityEngine;

namespace Cards.Meta
{
    [CreateAssetMenu(fileName = "Dismantle SO", menuName = "ScriptableObjects/MetaGame/Dismantle Chart SO")]
    public class DismantleChartSO : ScriptableObject
    {
        #region Fields
        ushort[][] _cardDismantleGain;
        #endregion
        #region Public Methods
        public void initData(string[] chartAsString)
        {

        }
        public ushort GetDismantleCost(Cards.Card card)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
