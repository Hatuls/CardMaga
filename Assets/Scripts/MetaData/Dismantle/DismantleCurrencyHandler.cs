using System;
using Account.GeneralData;
using CardMaga.MetaData.AccoutData;
using UnityEngine;

namespace CardMaga.MetaData.Dismantle
{
    [Serializable]
    public class DismantleCurrencyHandler
    {
        [SerializeField] private CurrencyPerRarityCostSO _costsSo;
        
        private int _chipsCurrency;
        private int _goldCurrency;

        public int ChipsCurrency => _chipsCurrency;

        public int GoldCurrency => _goldCurrency;

        
        public void AddCardCurrency(CardInstance cardInstance)
        {
            _chipsCurrency += Mathf.RoundToInt(_costsSo.GetCardCostPerCurrencyAndCardCore(cardInstance, Rewards.CurrencyType.Chips).Amount) ;
            _goldCurrency += 0;
        }
        
        public void RemoveCardCurrency(CardInstance cardInstance)
        {
            _chipsCurrency -= Mathf.RoundToInt(_costsSo.GetCardCostPerCurrencyAndCardCore(cardInstance, Rewards.CurrencyType.Chips).Amount);
            _goldCurrency -= 0;
        }

        public void ResetDismantelCurrency()
        {
            _chipsCurrency = 0;
            _goldCurrency = 0;
        }
    }
}