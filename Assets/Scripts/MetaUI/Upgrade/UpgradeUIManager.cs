using Account;
using Account.GeneralData;
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
        UpgradeCardsDisplayer _upgradeCardsDisplayer;
        [SerializeField]
        private UpgradeCostHandler _upgradeCostHandler;

        private UpgradeManager _upgradeCardHandler;
        private CardInstance _currentCard;
        public int Priority => 0;


        public void SetCurrentCard(CardInstance card)
        {
            _currentCard = card;
            _upgradeCardsDisplayer.InitCards(card);
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
        private void Start()
        {
            TrySystem();
        }
        [Button]
        private void Upgrade()
        {
            _upgradeCardHandler = new UpgradeManager();
            if (_upgradeCardHandler.TryUpgradeCard(_cardInstance))
                SetCurrentCard(_cardInstance);
        }
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
            if (!currentCard.IsMaxLevel)
            {
                if(_cardUpgradeCostSO==null)
                    _cardUpgradeCostSO = Resources.Load<CurrencyPerRarityCostSO>("MetaGameData/UpgradeCostSO");
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


}