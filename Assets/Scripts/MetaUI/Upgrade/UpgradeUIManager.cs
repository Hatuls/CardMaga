using Account;
using Account.GeneralData;
using CardMaga.Input;
using CardMaga.MetaUI;
using CardMaga.MetaUI.CollectionUI;
using CardMaga.SequenceOperation;
using CardMaga.UI;
using CardMaga.UI.Card;
using ReiTools.TokenMachine;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.Meta.Upgrade
{

    public class UpgradeUIManager : BaseUIScreen, ISequenceOperation<MetaUIManager>
    {
        [SerializeField]
        UpgradeCardsDisplayer _upgradeCardsDisplayer;
        [SerializeField]
        private UpgradeCostHandler _upgradeCostHandler;

        private InputBehaviour<BattleCardUI> _inputBehaviour;
        private UpgradeManager _upgradeCardHandler;
        private CardInstance _currentCard;
        public int Priority => 0;




        public void ExecuteTask(ITokenReciever tokenMachine, MetaUIManager data)
        {
            _upgradeCardHandler = data.MetaDataManager.UpgradeManager;
            data.OnMetaUIManagerDestroyed += BeforeDestroyed;
            Hide();
            _inputBehaviour = new InputBehaviour<BattleCardUI>();
            _inputBehaviour.OnClick += SetCurrentCard;
            data.MetaDeckBuildingUIManager.OnDeckBuildingInitiate += AssignInputsBehaviourToDeckBuilding;
        }

        private void BeforeDestroyed(MetaUIManager data)
        {
            data.OnMetaUIManagerDestroyed -= BeforeDestroyed;
            _inputBehaviour.OnClick -= SetCurrentCard;
            data.MetaDeckBuildingUIManager.OnDeckBuildingInitiate -= AssignInputsBehaviourToDeckBuilding;
        }

        private void AssignInputsBehaviourToDeckBuilding(MetaDeckBuildingUIManager metaDeckBuildingUIManager)
        {
            var cardsInDeck = metaDeckBuildingUIManager.InDeckCardsUI;
            var cardsInCollection = metaDeckBuildingUIManager.InCollectionCardsUI;


            foreach (CardUIInputHandler item in Inputs())
                item.TrySetInputBehaviour(_inputBehaviour);
            

            IEnumerable<CardUIInputHandler> Inputs ()
            {
                foreach (var card in cardsInDeck)
                    yield return card.CardUI.Inputs;

                foreach (var card in cardsInCollection)
                    yield return card.CardUI.Inputs;
            }
        }

       
        public void SetCurrentCard(BattleCardUI battlecard)
        {
            OpenScreen();
            var card = battlecard.BattleCardData.CardInstance;
            _currentCard = card;
            _upgradeCardsDisplayer.InitCards(card);
            _upgradeCostHandler.Init(_currentCard);
        }
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
        public CurrencyPerRarityCostSO CardUpgradeCostSO
        {
            get
            {
                if (_cardUpgradeCostSO == null)
                    _cardUpgradeCostSO = Resources.Load<CurrencyPerRarityCostSO>("MetaGameData/UpgradeCostSO");
                return _cardUpgradeCostSO;
            } 
        }
        public void Init(CardInstance currentCard)
        {
            var cardCore = currentCard.GetCardCore();
            if (!currentCard.IsMaxLevel)
            {
                var chipCosts = CardUpgradeCostSO.GetCardCostPerCurrencyAndCardCore(cardCore, Rewards.CurrencyType.Chips);
                var goldCosts = CardUpgradeCostSO.GetCardCostPerCurrencyAndCardCore(cardCore, Rewards.CurrencyType.Gold);
                EnableBottomPart();

                InitBottomPart(Convert.ToInt32(chipCosts.Amount), Convert.ToInt32(goldCosts.Amount));
            }
            else
                DisableBottomPart();
        }
        private void EnableBottomPart()
        {
            _maxedOutContainer.SetActive (false);
            _hasLevelsContainer.SetActive(true);
        }
        private void DisableBottomPart()
        {
            _maxedOutContainer.SetActive(true);
            _hasLevelsContainer.SetActive(false);
        }

        private void InitBottomPart(int chipCosts, int goldCost) // need to add gold visuals...
        {

        
            int currentAmount = 0;
            int spriteIndex = 0;
            if (!ReferenceEquals(AccountManager.Instance, null))
                currentAmount = AccountManager.Instance.Data.AccountResources.Chips;
            // Set Text
            AssignText(_chipText, currentAmount, chipCosts, spriteIndex);

            if (!ReferenceEquals(AccountManager.Instance, null))
                currentAmount = AccountManager.Instance.Data.AccountResources.Gold;

            spriteIndex++;
            // Set Text
            AssignText(_goldText, currentAmount, goldCost, spriteIndex++);
            // Set);




            //Enable Inputs
            _upgradeBtn.DisableClick = false;
        }

        private void AssignText(TextMeshProUGUI text, int currentAmount,int maxAmount,int spriteIndex)
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