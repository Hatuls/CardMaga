﻿using Account.GeneralData;
using CardMaga.MetaData.Collection;
using CardMaga.Rewards.Bundles;
using CardMaga.SequenceOperation;
using CardMaga.Server.Request;
using MetaData;
using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.Meta.Upgrade
{
    public class UpgradeManager : ISequenceOperation<MetaDataManager>
    {
        public event Action<CardInstance> OnUpgradeCardCompleted;
        public event Action OnUpgradeComplete;
        public event Action OnUpgradeFailed;

        private ValidateUserCurrency _validateUserCurrency;
        private ValidateCardsLevel _validateCardsLevel;
        private CurrencyPerRarityCostSO _upgradeCostsSO;
        private CardsCollectionDataHandler _cardsCollectionDataHandler;
        public CurrencyPerRarityCostSO UpgradeCosts => _upgradeCostsSO;
        public int Priority => 0;

        // Validation before upgrading a card
        public IEnumerable<IValidateOperation<UpgradeInfo>> Validations
        {
            get
            {
                yield return _validateCardsLevel = new ValidateCardsLevel();
                yield return _validateUserCurrency = new ValidateUserCurrency();
            }

        }


        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
            _upgradeCostsSO = Resources.Load<CurrencyPerRarityCostSO>("MetaGameData/UpgradeCostSO");
            if (_upgradeCostsSO == null)
                throw new Exception($"UpgradeManager: Could not load upgrade costs from resource folder\nTried to find it at this folder: MetaGameData/UpgradeCostSO");

            _cardsCollectionDataHandler = data.AccountDataCollectionHelper.CollectionCardDatasHandler;
        }

        public bool TryUpgradeCard(CardInstance cardInstance)
        {



            UpgradeInfo upgradeInfo = GenerateUpgradeInfo(cardInstance);

            bool check = CanUpgrade(cardInstance);

            if (check)
                DoUpgrade(upgradeInfo);
            else
                OnUpgradeFailed?.Invoke();

            return check;
        }

        public bool CanUpgrade(CardInstance card)
        {
            UpgradeInfo upgradeInfo = GenerateUpgradeInfo(card);

            bool check = true;
            foreach (var validation in Validations)
            {
                check &= validation.Validate(upgradeInfo);

                if (!check) break;
            }
            return check;
        }

        private UpgradeInfo GenerateUpgradeInfo(CardInstance cardInstance)
        {
#if UNITY_EDITOR
            _upgradeCostsSO = Resources.Load<CurrencyPerRarityCostSO>("MetaGameData/UpgradeCostSO");
            if (_upgradeCostsSO == null)
                throw new Exception($"UpgradeManager: Could not load upgrade costs from resource folder");
#endif

            return new UpgradeInfo(
                cardInstance,
                new List<ResourcesCost>()
                {
                     UpgradeCosts.GetCardCostPerCurrencyAndCardCore(cardInstance, Rewards.CurrencyType.Gold),
                     UpgradeCosts.GetCardCostPerCurrencyAndCardCore(cardInstance, Rewards.CurrencyType.Chips),
                });
        }

        private void DoUpgrade(UpgradeInfo upgradeInfo)
        {
            // Upgrade card
            var request = new ReduceResourceRequest(upgradeInfo, _cardsCollectionDataHandler);
            var tokenMachine = new TokenMachine(OnRelease);

            request.SendRequest(tokenMachine);


            void OnRelease()
            {
                OnUpgradeCardCompleted?.Invoke(upgradeInfo.CardInstance);
                OnUpgradeComplete?.Invoke();
            }
        }


        ~UpgradeManager()
        {
            Resources.UnloadAsset(_upgradeCostsSO);
        }


    }

    public class ReduceResourceRequest : BaseServerRequest
    {
        private readonly UpgradeInfo upgradeInfo;
        private readonly CardsCollectionDataHandler cardsCollectionDataHandler;

        public ReduceResourceRequest(UpgradeInfo upgradeInfo, CardsCollectionDataHandler cardsCollectionDataHandler)
        {
            this.upgradeInfo = upgradeInfo;
            this.cardsCollectionDataHandler = cardsCollectionDataHandler;
        }
        protected override void ServerLogic()
        {
            UpgradeCard();

            Account.AccountManager account = ReduceResources();

            account.UpdateDataOnServer();
            ReceiveResult();
        }

        private void UpgradeCard()
        {
            var cardInstance = upgradeInfo.CardInstance;

            if (cardsCollectionDataHandler.TryGetCardInstanceInfo(x => x.InstanceID == cardInstance.InstanceID, out MetaCardInstanceInfo[] metaCardInstancesInfo))
            {
                MetaCardInstanceInfo firstInstance = metaCardInstancesInfo[0];

                if (!cardsCollectionDataHandler.TryRemoveCardInstance(cardInstance.InstanceID,false))
                    throw new Exception("UpgradeManager: Could not remove card instance from collection data handler\nInstance ID = " + cardInstance.InstanceID);

                firstInstance.CardInstance.GetCardCore().LevelUp();

                cardsCollectionDataHandler.AddCardInstance(firstInstance);
            }
        }

        private Account.AccountManager ReduceResources()
        {
            var account = Account.AccountManager.Instance;
            var resource = account.Data.AccountResources;
            var costs = upgradeInfo.Costs;

            for (int i = 0; i < costs.Count; i++)
                resource.TryReduceAmount(costs[i]);
            return account;
        }
    }



    public class UpgradeInfo
    {
        private readonly CardInstance _cardInstance;
        private readonly List<ResourcesCost> _costs;

        public UpgradeInfo(CardInstance cardInstance, List<ResourcesCost> costs)
        {
            _cardInstance = cardInstance;
            _costs = costs;
        }

        public CardInstance CardInstance => _cardInstance;
        public List<ResourcesCost> Costs => _costs;
    }

}