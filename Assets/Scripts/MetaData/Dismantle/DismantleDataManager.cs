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
        public event Action<CardInstance> OnSuccessfulAddToCollection;
        public event Action<CardInstance> OnSuccessfulRemoveFromCollection;

        private DismantleCurrencyHandler _dismantleCurrencyHandler;
        
        private AccountDataCollectionHelper _accountData;
        private DismantleHandler _dismantleHandler;

        private CardsCollectionDataHandler _cardCollectionDatas;

        private TypeValidator<CardInstance> _cardDataValidator;
        
        List<BaseValidatorCondition<CardInstance>> _validatorConditions = new List<BaseValidatorCondition<CardInstance>>()
        {
            //add validation
        };

        public CardsCollectionDataHandler CardCollectionDatas => _cardCollectionDatas;
        
        public int Priority => 0;
        
        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
            _accountData = data.AccountDataCollectionHelper;
            _dismantleHandler = new DismantleHandler(data.AccountDataCollectionHelper.CollectionCardDatasHandler);
            _dismantleCurrencyHandler = new DismantleCurrencyHandler();
            _cardDataValidator = new TypeValidator<CardInstance>();
        }

        public void SetCardCollection()
        {
            _cardCollectionDatas = _accountData.GetAllUnAssingeCard();
            
            foreach (var cardData in _cardCollectionDatas.CollectionCardDatas)
            {
                cardData.OnTryAddItemToCollection += AddCardToDismantleList;
                cardData.OnTryRemoveItemFromCollection += RemoveCardFromDismantleList;
                OnSuccessfulAddToCollection += cardData.SuccessAddOrRemoveFromCollection;
                OnSuccessfulRemoveFromCollection += cardData.SuccessAddOrRemoveFromCollection;
            }
        }

        public void AddCardToDismantleList(CardInstance cardInstance)
        {
            if (_cardDataValidator.Valid(cardInstance,out string failedMassage))
            {
                _dismantleCurrencyHandler.AddCardCurrency(cardInstance);
                _dismantleHandler.AddCardToDismantleList(cardInstance);
                CardCollectionDatas.TryRemoveCardInstance(cardInstance.InstanceID);
                
                OnSuccessfulAddToCollection?.Invoke(cardInstance);
                OnCardAddToDismantel?.Invoke(_dismantleCurrencyHandler.ChipsCurrency,_dismantleCurrencyHandler.GoldCurrency);
            }
        }
        
        public void RemoveCardFromDismantleList(CardCore cardCore)
        {
            var cache = _dismantleHandler.RemoveCardFromDismantleList(cardCore);
            _dismantleCurrencyHandler.RemoveCardCurrency(cache);
            CardCollectionDatas.AddCardInstance(new MetaCardInstanceInfo(cache));    
            
            OnSuccessfulRemoveFromCollection?.Invoke(cache);
            OnCardAddToDismantel?.Invoke(_dismantleCurrencyHandler.ChipsCurrency,_dismantleCurrencyHandler.GoldCurrency);
        }

        public void ResetDismantleCard()
        {
            _dismantleHandler.ResetDismantleList();
            _dismantleCurrencyHandler.ResetDismantelCurrency();
        }

        public void ConfirmDismantleCards()
        {
            var cache = _dismantleHandler.ConfirmDismantleList();

            foreach (var cardInstance in cache)
            {
                _cardCollectionDatas.TryRemoveCardInstance(cardInstance.InstanceID);//plaster 10.1.23
            }
            
            OnCardAddToDismantel?.Invoke(_dismantleCurrencyHandler.ChipsCurrency,_dismantleCurrencyHandler.GoldCurrency);
        }

        public void Dispose()
        {
            foreach (var cardData in _cardCollectionDatas.CollectionCardDatas)
            {
                cardData.OnTryAddItemToCollection -= AddCardToDismantleList;
                cardData.OnTryRemoveItemFromCollection -= RemoveCardFromDismantleList;
                OnSuccessfulAddToCollection -= cardData.SuccessAddOrRemoveFromCollection;
                OnSuccessfulRemoveFromCollection -= cardData.SuccessAddOrRemoveFromCollection;
            }
        }
    }
}