using System;
using System.Collections.Generic;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.Collection;
using CardMaga.SequenceOperation;
using CardMaga.ValidatorSystem;
using MetaData;
using ReiTools.TokenMachine;
using UnityEngine;

namespace CardMaga.MetaData.Dismantle
{
    [Serializable]
    public class DismantleDataManager : ISequenceOperation<MetaDataManager>
    {
        public event Action<int,int> OnCardAddToDismantel;

        [SerializeField] private MetaCardDataValidator _cardDataValidator;
        [SerializeField] private DismantleCurrencyHandler _dismantleCurrencyHandler;
        private DismantleHandler _dismantleHandler;
        private AccountDataCollectionHelper _accountData;

        public List<MetaCollectionCardData> CardCollectionDatas => _accountData.ALlCollectionCardDatas;
        
        public int Priority => 0;
        
        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
            _accountData = data.AccountDataCollectionHelper;
            _dismantleHandler = new DismantleHandler();
            _dismantleCurrencyHandler = new DismantleCurrencyHandler();
            
            SetCardCollection();//temp
        }

        private void SetCardCollection()
        {
            foreach (var cardData in CardCollectionDatas)
            {
                cardData.OnTryAddItemToCollection += AddCardToDismantleList;
            }
        }

        public void AddCardToDismantleList(MetaCardData metaCardData)
        {
            if (_cardDataValidator.Valid(metaCardData))
            {
                _dismantleCurrencyHandler.AddCardCurrency(metaCardData);
                _dismantleHandler.AddCardToDismantleList(metaCardData);
                
                
                OnCardAddToDismantel?.Invoke(_dismantleCurrencyHandler.ChipsCurrency,_dismantleCurrencyHandler.GoldCurrency);
            }
            
            
        }

        public void ResetDismantelCard()
        {
            _dismantleHandler.ResetDismantelList();
            _dismantleCurrencyHandler.ResetDismantelCurrency();
        }

        public void Dispose()
        {
            foreach (var cardData in CardCollectionDatas)
            {
                cardData.OnTryAddItemToCollection -= AddCardToDismantleList;
            }
        }
    }
}