using Account;
using Account.GeneralData;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaUI;
using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;
using Sirenix.OdinInspector;
using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.Meta.Upgrade
{

    public class UpgradeUIManager : MonoBehaviour, ISequenceOperation<MetaUIManager>
    {
        [SerializeField]
        private UpgradeCardsVisualHandler _upgradeCardsVisualHandler;
        [SerializeField]
        private UpgradeCostHandler _upgradeCostHandler;

        private UpgradeManager _upgradeCardHandler;
        private CardInstance _currentCard;
        public int Priority => 0;
        public void SetCurrentCard(CardInstance card)
        {
            _currentCard = card;

            _upgradeCardsVisualHandler.SetMiddleCard(new MetaCardData(card, card.CardSO, new Card.BattleCardData(card)));
            _upgradeCostHandler.Init(_currentCard);
        }

        public void ExecuteTask(ITokenReciever tokenMachine, MetaUIManager data)
        {
            _upgradeCardHandler = data.MetaDataManager.UpgradeManager;
        }



        #region Editor

#if UNITY_EDITOR
        [SerializeField, Header("Editor:")]
        private CardInstance _cardInstance;

        [Button]
        private void TrySystem()
            => SetCurrentCard(_cardInstance);
#endif
        #endregion
    }

    [Serializable]
    public class UpgradeCostHandler
    {
        private const string MAXED_OUT = "Maxed Level!";
        private const string SLASH = " / ";

        [SerializeField]
        private Slider _slider;
        [SerializeField]
        private TextMeshProUGUI _text;
        [SerializeField]
        private CurrencyPerRarityCostSO _cardUpgradeCostSO;

        [SerializeField]
        private Input.Button _upgradeBtn;

        StringBuilder _stringBuilder = new StringBuilder();


        public void Init(CardInstance currentCard)
        {
            var cardCore = currentCard.GetCardCore();
            if (!cardCore.CardsAtMaxLevel)
            {
                var chipCosts = _cardUpgradeCostSO.GetCardCostPerCurrencyAndCardCore(cardCore, Rewards.CurrencyType.Chips);
                var goldCosts = _cardUpgradeCostSO.GetCardCostPerCurrencyAndCardCore(cardCore, Rewards.CurrencyType.Gold);
                InitBottomPart(Convert.ToInt32(chipCosts.Amount), Convert.ToInt32(goldCosts.Amount));
            }
            else
                DisableBottomPart();
        }

        private void DisableBottomPart()
        {
            _text.text = MAXED_OUT;
            _slider.gameObject.SetActive(false);
            _upgradeBtn.DisableClick = true;
        }

        private void InitBottomPart(int chipCosts, int goldCost) // need to add gold visuals...
        {
            // Settings the slider
            if (!_slider.gameObject.activeSelf)
                _slider.gameObject.SetActive(true);
            //Slider
            int currentAmount = 0;
            if (!ReferenceEquals(AccountManager.Instance, null))
                currentAmount = AccountManager.Instance.Data.AccountResources.Chips;
            _slider.maxValue = chipCosts;
            _slider.value = chipCosts * Mathf.Min(currentAmount / chipCosts, 1);

            // Set Text
            _stringBuilder.Append(currentAmount);
            _stringBuilder.Append(SLASH);
            _stringBuilder.Append(chipCosts);
            _text.text = _stringBuilder.ToString();
            _stringBuilder.Clear();

            //Enable Inputs
            _upgradeBtn.DisableClick = false;
        }
    }

    [Serializable]
    public class UpgradeCardsVisualHandler
    {

        [SerializeField]
        private MetaCardUI _middleCardUI;
        [SerializeField]
        private MetaCardUI _leftCardUI;
        [SerializeField]
        private MetaCardUI _rightCardUI;


        public void SetMiddleCard(MetaCardData battleCardData)
        {
            _middleCardUI.AssignDataAndVisual(battleCardData);
            InitRightCard();
            InitLeftCard();
        }

        private void InitRightCard()
         => InitCard(_rightCardUI, IsMaxLevel, _middleCardUI.CardInstance.ID + 1);
        private void InitLeftCard()
        => InitCard(_leftCardUI, IsFirstLevel, _middleCardUI.CardInstance.ID - 1);
        private void InitCard(MetaCardUI card, Predicate<CardInstance> condition, int nextID)
        {

            if (condition.Invoke(_middleCardUI.CardInstance))
                card.gameObject.SetActive(false);
            else
            {
                MetaCardData featuringLevel = Factory.GameFactory.Instance.CardFactoryHandler.GetMetaCardData(new CardCore(nextID));
                card.AssignDataAndVisual(featuringLevel);
                card.gameObject.SetActive(true);
            }
        }
        private bool IsMaxLevel(CardInstance cardInstance) => cardInstance.GetCardCore().CardsAtMaxLevel;
        private bool IsFirstLevel(CardInstance cardInstance) => cardInstance.Level == 0;


        #region Editor
#if UNITY_EDITOR
        [Header("Editor:")]
        [SerializeField]
        private MetaCardData _battleCardData;
        [Button]
        private void TrySetCard() => SetMiddleCard(_battleCardData);


#endif
        #endregion
    }
}