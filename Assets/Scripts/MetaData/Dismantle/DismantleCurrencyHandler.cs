using System;
using CardMaga.MetaData.AccoutData;
using UnityEngine;

namespace CardMaga.MetaData.Dismantle
{
    [Serializable]
    public class DismantleCurrencyHandler
    {
        [SerializeField] private DismentalCostsSO _costsSo;
        
        private int _chipsCurrency;
        private int _goldCurrency;

        public int ChipsCurrency => _chipsCurrency;

        public int GoldCurrency => _goldCurrency;

        
        public void AddCardCurrency(MetaCardData metaCardData)
        {
            _chipsCurrency += AddChips(metaCardData);
            _goldCurrency += AddGold(metaCardData);
        }

        private int AddChips(MetaCardData metaCardData)
        {
            return  _costsSo.GetCardDismentalCost(metaCardData);
        }

        private int AddGold(MetaCardData metaCardData)
        {
            return 0;
        }

    }
}