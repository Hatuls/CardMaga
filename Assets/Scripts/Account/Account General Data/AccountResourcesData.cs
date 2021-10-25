using System;
namespace Account.GeneralData
{
    [Serializable]
    public class AccountResourcesData
    {
        #region Fields
        private Stat<ushort> _chips;
        private Stat<uint> _diamonds;
        private Stat<uint> _gold;
        #endregion

        #region Properites
        public Stat<uint> Gold { get => _gold; private set => _gold = value; }
        public Stat<uint> Diamonds { get => _diamonds; private set => _diamonds = value; }
        public Stat<ushort> Chips { get => _chips; private set => _chips = value; }

        #endregion


        public AccountResourcesData(ushort chips = 0,uint diamonds = 0, uint gold = 0 )
        {
            _chips = new Stat<ushort>(chips);
            _diamonds = new Stat<uint>(diamonds);
            _gold = new Stat<uint>(gold);
        }
    }
}