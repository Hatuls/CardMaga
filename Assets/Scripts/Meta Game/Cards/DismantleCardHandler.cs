using CardMaga.Card;
using System;
namespace Cards.Meta
{
    public class DismantleCardHandler
    {
        #region Fields
        DismantleChartSO _dismantleChartSO;
        #endregion
        #region Public Methods
        public DismantleCardHandler(DismantleChartSO dismantleChartSO)
        {

        }
        public ushort GetDismanteCost(CardData card)
        {
            throw new NotImplementedException();
        }
        public bool CanBeDismantled(CardData card)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
