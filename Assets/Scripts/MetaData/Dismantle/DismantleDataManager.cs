using System;
using System.Collections.Generic;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.Collection;
using CardMaga.SequenceOperation;
using MetaData;
using ReiTools.TokenMachine;
using UnityEngine;

namespace CardMaga.MetaData.Dismantle
{
    [Serializable]
    public class DismantleDataManager : ISequenceOperation<MetaDataManager>
    {
        [SerializeField] private DismantleCurrencyHandler _dismantleCurrencyHandler;
        private DismantleHandler _dismantleHandler;
        private AccountDataCollectionHelper _accountData;

        public List<MetaCollectionCardData> CardCollectionDatas => _accountData.CollectionCardDatas;
        
        public int Priority => 0;
        
        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
            _accountData = data.AccountDataCollectionHelper;
            _dismantleHandler = new DismantleHandler();
            _dismantleCurrencyHandler = new DismantleCurrencyHandler();
        }

        public void AddCardToDismantleList(MetaCardData metaCardData)
        {
            _dismantleCurrencyHandler.AddCardCurrency(metaCardData);
            _dismantleHandler.AddCardToDismantleList(metaCardData);
        }

        public void ResetDismantelCard()
        {
            _dismantleHandler.ResetDismantelList();
            _dismantleCurrencyHandler.ResetDismantelCurrency();
        }
    }
}