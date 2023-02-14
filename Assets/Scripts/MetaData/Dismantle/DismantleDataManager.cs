using System;
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
using ValidatorSystem.ValidationConditionGroup.CardInstance;

namespace CardMaga.MetaData.Dismantle
{
    public class DismantleDataManager : ISequenceOperation<MetaDataManager>
    {
        public event Action<int,int> OnCardAddToDismantel;
        public event Action<MetaCardInstanceInfo> OnSuccessfulAddToCollection;
        public event Action<MetaCardInstanceInfo> OnSuccessfulRemoveFromCollection;

        private DismantleCurrencyHandler _dismantleCurrencyHandler;
        
        private AccountDataCollectionHelper _accountDataCollectionHelper;
        private DismantleHandler _dismantleHandler;

        private CardsCollectionDataHandler _cardCollectionDatas;
        private MetaAccountDataManager _metaAccountDataManager;

        public CardsCollectionDataHandler CardCollectionDatas => _cardCollectionDatas;
        
        public int Priority => 2;

        public DismantleHandler DismantleHandler => _dismantleHandler;

        public DismantleCurrencyHandler DismantleCurrencyHandler => _dismantleCurrencyHandler;

        public void ExecuteTask(ITokenReceiver tokenMachine, MetaDataManager data)
        {
            _metaAccountDataManager = data.AccountDataManager;
            
            _accountDataCollectionHelper = data.AccountDataCollectionHelper;
            _dismantleHandler = new DismantleHandler(data.AccountDataCollectionHelper.CollectionCardDatasHandler);
            _dismantleCurrencyHandler = new DismantleCurrencyHandler();
        }

        public void SetCardCollection()
        {
            _cardCollectionDatas = _accountDataCollectionHelper.GetCollectionCopy().GetAllUnAssingeCard();
            
            foreach (var cardData in _cardCollectionDatas.CollectionCardDatas.Values)
            {
                cardData.OnTryAddItemToCollection += AddCardToDismantleList;
                cardData.OnTryRemoveItemFromCollection += RemoveCardFromDismantleList;
                OnSuccessfulAddToCollection += cardData.SuccessAddOrRemoveFromCollection;
                OnSuccessfulRemoveFromCollection += cardData.SuccessAddOrRemoveFromCollection;
            }
        }

        private void AddCardToDismantleList(MetaCardInstanceInfo cardInstance)
        {
            //if (Validator.Valid(cardInstance,out IValidFailedInfo validInfo,ValidationTag.SystemCardInstance))
           // {
                _dismantleCurrencyHandler.AddCardCurrency(cardInstance);
                _dismantleHandler.AddCardToDismantleList(cardInstance);
                CardCollectionDatas.TryRemoveCardInstance(cardInstance.InstanceID,false);
                
                OnSuccessfulAddToCollection?.Invoke(cardInstance);
                OnCardAddToDismantel?.Invoke(_dismantleCurrencyHandler.ChipsCurrency,_dismantleCurrencyHandler.GoldCurrency);
           // }
        }

        private void RemoveCardFromDismantleList(CardCore cardCore)
        {
            _dismantleCurrencyHandler.RemoveCardCurrency(cardCore);
            var cache = _dismantleHandler.RemoveCardFromDismantleList(cardCore);
            CardCollectionDatas.AddCardInstance(cache);    
            
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
                _cardCollectionDatas.CleanCollection();
                _metaAccountDataManager.RemoveCard(cardInstance);
            }
            
            //_accountData.UpdateCollection();
            
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
            foreach (var cardData in _cardCollectionDatas.CollectionCardDatas.Values)
            {
                cardData.OnTryAddItemToCollection -= AddCardToDismantleList;
                cardData.OnTryRemoveItemFromCollection -= RemoveCardFromDismantleList;
                OnSuccessfulAddToCollection -= cardData.SuccessAddOrRemoveFromCollection;
                OnSuccessfulRemoveFromCollection -= cardData.SuccessAddOrRemoveFromCollection;
            }
        }
    }
}