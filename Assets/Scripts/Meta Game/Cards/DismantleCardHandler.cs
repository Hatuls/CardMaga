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
        public ushort GetDismanteCost(Card card)
        {
            throw new NotImplementedException();
        }
        public bool CanBeDismantled(Card card)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
