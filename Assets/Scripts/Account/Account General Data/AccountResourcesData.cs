using System;
using UnityEngine;
namespace Account.GeneralData
{
    [Serializable]
    public class AccountResourcesData : ILoadFirstTime
    {
        #region Fields
        [SerializeField]
        private IntStat _chips;
        [SerializeField]
        private IntStat _diamonds;
        [SerializeField]
        private IntStat _gold;
        #endregion

        #region Properites
        public IntStat Gold { get => _gold; private set => _gold = value; }
        public IntStat Diamonds { get => _diamonds; private set => _diamonds = value; }
        public IntStat Chips { get => _chips; private set => _chips = value; }

        #endregion

        public AccountResourcesData()
        {

        }
        public AccountResourcesData(int chips = 0, int diamonds = 0, int gold = 0)
        {
            _chips = new TicketStat(chips);
            _diamonds = new DiamondsStat(diamonds);
            _gold = new GoldStat(gold);
        }
        //remove this
        public void NewLoad()
        {

            _chips = new TicketStat(DefaultVersion._gameVersion.Chips);
            _diamonds = new DiamondsStat(DefaultVersion._gameVersion.Diamonds);
            _gold = new GoldStat(0);
        }

        public bool IsCorrupted()
        {
            return _diamonds.Value == 0 && _chips.Value == 0 && _gold.Value == 0;

        }
    }

    public class DiamondsStat : IntStat
    {
        public DiamondsStat(int val) : base(val) { }
    }
    public class TicketStat : IntStat
    {
        public TicketStat(int val) : base(val) { }
    }
    public class GoldStat : IntStat
    {
        public GoldStat(int val) : base(val) { }
    }
}