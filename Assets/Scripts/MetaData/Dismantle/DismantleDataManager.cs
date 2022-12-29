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
        private TypeValidator<MetaCollectionCardData> _cardDataValidator;
        private DismantleHandler _dismantleHandler;
        private AccountDataCollectionHelper _accountData;

        private List<BaseValidatorCondition<MetaCollectionCardData>> _validatorConditions;

        public List<MetaCollectionCardData> CardCollectionDatas => _accountData.ALlCollectionCardDatas;
        
        public int Priority => 0;
        
        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
            _accountData = data.AccountDataCollectionHelper;
            _dismantleHandler = new DismantleHandler();
            _dismantleCurrencyHandler = new DismantleCurrencyHandler();
            _cardDataValidator = new TypeValidator<MetaCollectionCardData>();

            _validatorConditions = new List<BaseValidatorCondition<MetaCollectionCardData>>()
            {
                new IsEnoughInstance(),
            };
            
            SetCardCollection();//temp
        }

        private void SetCardCollection()
        {
            foreach (var cardData in CardCollectionDatas)
            {
                cardData.OnTryAddItemToCollection += AddCardToDismantleList;
            }
        }

        public void AddCardToDismantleList(MetaCollectionCardData collectionCardData)
        {
            if (_cardDataValidator.Valid(collectionCardData,out string failedMassage) && collectionCardData.GetUnassingCard(out CardInstance cardInstance))
            {
                _dismantleCurrencyHandler.AddCardCurrency(cardInstance);
                _dismantleHandler.AddCardToDismantleList(cardInstance);
                
                OnCardAddToDismantel?.Invoke(_dismantleCurrencyHandler.ChipsCurrency,_dismantleCurrencyHandler.GoldCurrency);
            }
        }
        
        public void RemoveCardFromDismantleList(MetaCollectionCardData collectionCardData)
        {
            var cache = _dismantleHandler.RemoveCardFromDismantleList(collectionCardData);
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
            foreach (var cardData in CardCollectionDatas)
            {
                cardData.OnTryAddItemToCollection -= AddCardToDismantleList;
            }
        }
    }
}