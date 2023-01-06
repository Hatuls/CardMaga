using System;
using System.Collections.Generic;
using Account.GeneralData;
using CardMaga.MetaData.Collection;
using CardMaga.SequenceOperation;
using CardMaga.ValidatorSystem;
using CardMaga.ValidatorSystem.ValidatorConditions;
using MetaData;
using ReiTools.TokenMachine;
using UnityEngine;

namespace CardMaga.MetaData.Dismantle
{
    [Serializable]
    public class DismantleDataManager : ISequenceOperation<MetaDataManager>
    {
        public event Action<int,int> OnCardAddToDismantel;

        [SerializeField] private DismantleCurrencyHandler _dismantleCurrencyHandler;
        
        private AccountDataCollectionHelper _accountData;
        private DismantleHandler _dismantleHandler;

        private TypeValidator<CardInstance> _cardDataValidator;
        
        List<BaseValidatorCondition<CardInstance>> _validatorConditions = new List<BaseValidatorCondition<CardInstance>>()
        {
            //add validation
        };

        public CardsCollectionDataHandler CardCollectionDatas => _accountData.GetAllUnAssingeCard();
        
        public int Priority => 0;
        
        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
            _accountData = data.AccountDataCollectionHelper;
            _dismantleHandler = new DismantleHandler();
            _dismantleCurrencyHandler = new DismantleCurrencyHandler();
            _cardDataValidator = new TypeValidator<CardInstance>();

            
            
            //SetCardCollection();//temp
        }

        private void SetCardCollection()
        {
            foreach (var cardData in CardCollectionDatas.CollectionCardDatas)
            {
                cardData.OnTryAddItemToCollection += AddCardToDismantleList;
            }
        }

        public void AddCardToDismantleList(CardInstance cardInstance)
        {
            if (_cardDataValidator.Valid(cardInstance,out string failedMassage))
            {
                _dismantleCurrencyHandler.AddCardCurrency(cardInstance);
                _dismantleHandler.AddCardToDismantleList(cardInstance);
                
                OnCardAddToDismantel?.Invoke(_dismantleCurrencyHandler.ChipsCurrency,_dismantleCurrencyHandler.GoldCurrency);
            }
        }
        
        public void RemoveCardFromDismantleList(CardCore cardCore)
        {
            var cache = _dismantleHandler.RemoveCardFromDismantleList(cardCore);
            _dismantleCurrencyHandler.RemoveCardCurrency(cache);
                
            OnCardAddToDismantel?.Invoke(_dismantleCurrencyHandler.ChipsCurrency,_dismantleCurrencyHandler.GoldCurrency);
        }

        public void ResetDismantelCard()
        {
            _dismantleHandler.ResetDismantelList();
            _dismantleCurrencyHandler.ResetDismantelCurrency();
        }

        public void Dispose()
        {
            foreach (var cardData in CardCollectionDatas.CollectionCardDatas)
            {
                cardData.OnTryAddItemToCollection -= AddCardToDismantleList;
            }
        }
    }
}