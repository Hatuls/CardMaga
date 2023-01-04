using Account.GeneralData;
using CardMaga.Rewards.Bundles;
using CardMaga.SequenceOperation;
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

        public CurrencyPerRarityCostSO UpgradeCosts => _upgradeCostsSO;
        public int Priority => 0;

        // Validation before upgrading a card
        public IEnumerable<IValidateOperation<UpgradeInfo>> Validations
        {
            get
            {
                yield return _validateCardsLevel;
                yield return _validateUserCurrency;
            }

        }


        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
            _upgradeCostsSO = Resources.Load<CurrencyPerRarityCostSO>("MetaGameData/UpgradeCostSO");
            if (_upgradeCostsSO == null)
                throw new Exception($"UpgradeManager: Could not load upgrade costs from resource folder");
        }

        public bool TryUpgradeCard(CardInstance cardInstance)
        {
            if (_validateCardsLevel == null)
                InitValidations();


            UpgradeInfo upgradeInfo = GenerateUpgradeInfo(cardInstance);

            bool check = true;
            foreach (var valdation in Validations)
            {
                check &= valdation.Validate(upgradeInfo);
                if (!check)
                    break;
            }

            if (check)
                DoUpgrade(cardInstance);
            else
                OnUpgradeFailed?.Invoke();

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

        private void DoUpgrade(CardInstance cardInstance)
        {
            // Upgrade card
            cardInstance.GetCardCore().LevelUp();
            OnUpgradeCardCompleted?.Invoke(cardInstance);
            OnUpgradeComplete?.Invoke();
        }
        private void InitValidations()
        {
            _validateUserCurrency = new ValidateUserCurrency();
            _validateCardsLevel = new ValidateCardsLevel();
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