using System;
using UnityEngine;
namespace Account.GeneralData
{
    [Serializable]
    public class AccountResourcesData : ILoadFirstTime 

    {
        #region Fields
        [SerializeField]
        private UshortStat _chips;
        [SerializeField]
        private UintStat _diamonds;
        [SerializeField]
        private UintStat _gold;
        #endregion

        #region Properites
        public UintStat Gold { get => _gold; private set => _gold = value; }
        public UintStat Diamonds { get => _diamonds; private set => _diamonds = value; }
        public UshortStat Chips { get => _chips; private set => _chips = value; }

        #endregion

        public AccountResourcesData()
        {
      
        }
        public AccountResourcesData(ushort chips = 0,uint diamonds = 0, uint gold = 0 )
        {
            _chips = new UshortStat(chips);
            _diamonds = new UintStat(diamonds);
            _gold = new UintStat(gold);
        }

        public void NewLoad()
        {
            _chips = new UshortStat(0);
            _diamonds = new UintStat(0);
            _gold = new UintStat(0);
        }
    }
}