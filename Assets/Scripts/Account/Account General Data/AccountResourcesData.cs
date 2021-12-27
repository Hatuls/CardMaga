using System;
using System.Threading.Tasks;
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
        private UshortStat _diamonds;
        [SerializeField]
        private UshortStat _gold;
        #endregion

        #region Properites
        public UshortStat Gold { get => _gold; private set => _gold = value; }
        public UshortStat Diamonds { get => _diamonds; private set => _diamonds = value; }
        public UshortStat Chips { get => _chips; private set => _chips = value; }

        #endregion

        public AccountResourcesData()
        {
      
        }
        public AccountResourcesData(ushort chips = 0,uint diamonds = 0, uint gold = 0 )
        {
            _chips = new UshortStat(chips);
            _diamonds = new UshortStat((ushort)diamonds);
            _gold = new UshortStat((ushort)gold);
        }

        public async Task NewLoad()
        {

            _chips = new UshortStat((ushort)DefaultVersion._gameVersion.Chips);
            _diamonds = new UshortStat((ushort)DefaultVersion._gameVersion.Diamonds);
            _gold = new UshortStat(0);
        }
    }
}