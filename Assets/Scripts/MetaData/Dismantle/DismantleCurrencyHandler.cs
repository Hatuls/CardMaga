using System;
using Account.GeneralData;
using CardMaga.MetaData.AccoutData;
using CardMaga.Rewards;
using UnityEngine;

namespace CardMaga.MetaData.Dismantle
{
    public class DismantleCurrencyHandler
    {
        private CurrencyPerRarityCostSO _costsSo;
        
        private int _chipsCurrency;
        private int _goldCurrency;

        public int ChipsCurrency => _chipsCurrency;

        public int GoldCurrency => _goldCurrency;

        public DismantleCurrencyHandler()
        {
            _chipsCurrency = 0;
            _goldCurrency = 0;
            
            _costsSo = Resources.Load<CurrencyPerRarityCostSO>("MetaGameData/DismentalCostSO");
            if (_costsSo == null)
                throw new Exception($"DismantleCurrencyHandler: Could not load upgrade costs from resource folder");
        }

        ~DismantleCurrencyHandler()
        {
            Resources.UnloadAsset(_costsSo);
        }
        public void AddCardCurrency(CardInstance cardInstance)
        {
            var cardCore = cardInstance.GetCardCore();

            _chipsCurrency += Convert.ToInt32(_costsSo.GetCardCostPerCurrencyAndCardCore(cardCore, CurrencyType.Chips).Amount);

            _goldCurrency += Convert.ToInt32(_costsSo.GetCardCostPerCurrencyAndCardCore(cardCore, CurrencyType.Gold).Amount);
        }
        
        public void RemoveCardCurrency(CardInstance cardInstance)
        {
            var cardCore = cardInstance.GetCardCore();

            _chipsCurrency -= Convert.ToInt32(_costsSo.GetCardCostPerCurrencyAndCardCore(cardCore, CurrencyType.Chips).Amount);

            _goldCurrency -= Convert.ToInt32(_costsSo.GetCardCostPerCurrencyAndCardCore(cardCore, CurrencyType.Gold).Amount);
        }

        public void ResetDismantelCurrency()
        {
            _chipsCurrency = 0;
            _goldCurrency = 0;
        }
    }
}