using System;
using System.Collections.Generic;
using Account;
using Account.GeneralData;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.Collection;
using CardMaga.Rewards;
using CardMaga.Rewards.Bundles;
using CardMaga.SequenceOperation;
using CardMaga.ValidatorSystem;
using MetaData;
using ReiTools.TokenMachine;

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
        private MetaAccountData _metaAccountData;
        private AccountDataAccess _accountDataAccess;

        private TypeValidator<CardInstance> _cardDataValidator;
        
        List<BaseValidatorCondition<CardInstance>> _validatorConditions = new List<BaseValidatorCondition<CardInstance>>()
        {
            //add validation
        };

        public CardsCollectionDataHandler CardCollectionDatas => _cardCollectionDatas;
        
        public int Priority => 0;

        public DismantleHandler DismantleHandler => _dismantleHandler;

        public DismantleCurrencyHandler DismantleCurrencyHandler => _dismantleCurrencyHandler;

        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
            _accountDataAccess = data.AccountDataAccess;
            _metaAccountData = data.MetaAccountData;
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
                CardCollectionDatas.TryRemoveCardInstance(cardInstance.InstanceID,false);
                
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
                _cardCollectionDatas.TryRemoveCardInstance(cardInstance.InstanceID,true);//plaster 10.1.23
                _metaAccountData.AccountCards.Remove(cardInstance);
                _accountDataAccess.RemoveCard(cardInstance.GetCoreId());
            }
            
            var account = Account.AccountManager.Instance;
            var resource = account.Data.AccountResources;

            var chipsCost = new ResourcesCost(CurrencyType.Chips, _dismantleCurrencyHandler.ChipsCurrency);
            var goldCost = new ResourcesCost(CurrencyType.Gold, _dismantleCurrencyHandler.GoldCurrency);

            var costs = new ResourcesCost[]
            {
                chipsCost, goldCost
            };

            for (int i = 0; i < costs.Length; i++)
                resource.AddResource(costs[i]);
            
            AccountManager.Instance.UpdateDataOnServer();//plaster 10.1.23
            _dismantleHandler.ResetDismantleList();
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