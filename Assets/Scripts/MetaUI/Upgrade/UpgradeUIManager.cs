using Account;
using Account.GeneralData;
using CardMaga.Input;
using CardMaga.MetaUI;
using CardMaga.MetaUI.CollectionUI;
using CardMaga.Rewards.Bundles;
using CardMaga.SequenceOperation;
using CardMaga.UI;
using CardMaga.UI.Card;
using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace CardMaga.Meta.Upgrade
{

    public class UpgradeUIManager : BaseUIScreen, ISequenceOperation<MetaUIManager>
    {
        public event Action<int, int> OnCostAssigned;

        [SerializeField]
        private UpgradeConfirmationScreen _upgradeConfirmationScreen;
        [SerializeField]
        private UpgradeCardsDisplayer _upgradeCardsDisplayer;
        [SerializeField]
        private UpgradeCostHandler _upgradeCostHandler;

        private InputBehaviour<BattleCardUI> _inputBehaviour;
        private UpgradeManager _upgradeCardHandler;
        private CardInstance _currentCard;
        public int Priority => 0;




        public void ExecuteTask(ITokenReceiver tokenMachine, MetaUIManager data)
        {
            _upgradeCardHandler = data.MetaDataManager.UpgradeManager;
            data.OnMetaUIManagerDestroyed += BeforeDestroyed;
            Hide();
            _inputBehaviour = new InputBehaviour<BattleCardUI>();
            _inputBehaviour.OnClick += SetCurrentCard;

            _upgradeCardHandler.OnUpgradeCardCompleted += SetCurrentCard;
            data.MetaDeckEditingUIManager.OnDeckBuildingInitiate += AssignInputsBehaviourToDeckBuilding;
        }

        private void BeforeDestroyed(MetaUIManager data)
        {
            data.OnMetaUIManagerDestroyed -= BeforeDestroyed;
            _inputBehaviour.OnClick -= SetCurrentCard;
            data.MetaDeckEditingUIManager.OnDeckBuildingInitiate -= AssignInputsBehaviourToDeckBuilding;
            _upgradeCardHandler.OnUpgradeCardCompleted -= SetCurrentCard;
        }

        private void AssignInputsBehaviourToDeckBuilding(MetaDeckEditingUIManager metaDeckEditingUIManager)
        {
            var cardsInDeck = metaDeckEditingUIManager.InDeckCardsUI;
            var cardsInCollection = metaDeckEditingUIManager.InCollectionCardsUI;


            foreach (CardUIInputHandler item in Inputs())
                item.TrySetInputBehaviour(_inputBehaviour);


            IEnumerable<CardUIInputHandler> Inputs()
            {
                foreach (var card in cardsInDeck)
                    yield return card.CardUI.Inputs;

                foreach (var card in cardsInCollection)
                    yield return card.CardUI.Inputs;
            }
        }


        public void SetCurrentCard(BattleCardUI battlecard)
        => SetCurrentCard(battlecard.BattleCardData.CardInstance);


        private void SetCurrentCard(CardInstance card)
        {
            _upgradeConfirmationScreen.Close();
            OpenScreen();

            _currentCard = card;
            _upgradeCardsDisplayer.InitCards(card);

            CurrencyPerRarityCostSO costSo = _upgradeCardHandler.UpgradeCosts;
            ResourcesCost chipCosts = costSo.GetCardCostPerCurrencyAndCardCore(_currentCard, Rewards.CurrencyType.Chips);
            ResourcesCost goldCosts = costSo.GetCardCostPerCurrencyAndCardCore(_currentCard, Rewards.CurrencyType.Gold);

            int chipAmount = Convert.ToInt32(chipCosts.Amount);
            int goldAmount = Convert.ToInt32(goldCosts.Amount);
            _upgradeCostHandler.Init(_currentCard.IsMaxLevel, chipAmount, goldAmount);

            if (OnCostAssigned != null)
                OnCostAssigned.Invoke(chipAmount, goldAmount);
        }
        public void TryOpenUpgradeScreen()
        {
            if (_upgradeCardHandler.CanUpgrade(_currentCard))
                _upgradeConfirmationScreen.Open();
        }
        public void Upgrade()
            => _upgradeCardHandler.TryUpgradeCard(_currentCard);

        #region Editor

#if UNITY_EDITOR
        //[SerializeField, Header("Editor:")]
        //private CardInstance _cardInstance;

        //[Button]
        //private void TrySystem()
        //    => SetCurrentCard(_cardInstance);
        //private void Start()
        //{
        //    TrySystem();
        //}
        //[Button]
        //private void Upgrade()
        //{
        //    _upgradeCardHandler = new UpgradeManager();
        //    if (_upgradeCardHandler.TryUpgradeCard(_cardInstance))
        //        SetCurrentCard(_cardInstance);
        //}
#endif
        #endregion
    }

    [Serializable]
    public class UpgradeCostHandler
    {

        private const string SLASH = " / ";


        [SerializeField]
        GameObject _maxedOutContainer;
        [SerializeField]
        GameObject _hasLevelsContainer;

        [SerializeField]
        private TextMeshProUGUI _chipText;

        [SerializeField]
        private TextMeshProUGUI _goldText;
        [SerializeField]
        private CurrencyPerRarityCostSO _cardUpgradeCostSO;

        [SerializeField]
        private Input.Button _upgradeBtn;

        [SerializeField]
        private Color _hasAmountColor;
        [SerializeField]
        private Color _noAmountColor;

        private StringBuilder _stringBuilder = new StringBuilder();

        public void Init(bool isMaxLevel, int chipCost, int goldCost)
        {
            if (!isMaxLevel)
            {
                EnableBottomPart();
                InitBottomPart(chipCost, goldCost);
            }
            else
                DisableBottomPart();
        }
        private void EnableBottomPart()
        {
            _maxedOutContainer.SetActive(false);
            _hasLevelsContainer.SetActive(true);
        }
        private void DisableBottomPart()
        {
            _maxedOutContainer.SetActive(true);
            _hasLevelsContainer.SetActive(false);
        }

        private void InitBottomPart(int chipCosts, int goldCost) // need to add gold visuals...
        {
            const int goldSpriteIndex = 0, ChipSpriteIndex = 1;
            int currentAmount = 0;
            if (!ReferenceEquals(AccountManager.Instance, null))
                currentAmount = AccountManager.Instance.Data.AccountResources.Chips;
            // Set Text
            AssignText(_chipText, currentAmount, chipCosts, ChipSpriteIndex);

            if (!ReferenceEquals(AccountManager.Instance, null))
                currentAmount = AccountManager.Instance.Data.AccountResources.Gold;

            // Set Text
            AssignText(_goldText, currentAmount, goldCost, goldSpriteIndex);


            //Enable Inputs
            _upgradeBtn.DisableClick = false;


        }

        private void AssignText(TextMeshProUGUI text, int currentAmount, int maxAmount, int spriteIndex)
        {
            Color clr = currentAmount < maxAmount ? _noAmountColor : _hasAmountColor;
            string stringText = currentAmount.ToString().ToBold().ColorString(clr).AddImageInFrontOfText(spriteIndex);
            _stringBuilder.Append(stringText);
            _stringBuilder.Append(SLASH);
            _stringBuilder.Append(maxAmount);
            text.text = _stringBuilder.ToString();
            _stringBuilder.Clear();
        }
    }
}