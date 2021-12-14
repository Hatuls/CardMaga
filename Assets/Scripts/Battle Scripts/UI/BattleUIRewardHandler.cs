using Battles;
using DesignPattern;
using TMPro;
using UI;
using UnityEngine;

namespace Rewards.Battles
{
    public class BattleUIRewardHandler : MonoBehaviour, IObserver
    {
        [SerializeField]
        ushort _moneyForNotTakingAnything;
        [Sirenix.OdinInspector.ShowInInspector]
        public BattleReward BattleReward { get; set; }
        [SerializeField] Animator _animator;
        public static BattleUIRewardHandler Instance;

        [SerializeField]
        SelectCardRewardScreen _selectCardRewardScreen;
        [SerializeField]
        BattleRewardHandler _battleRewardHandler;


        [SerializeField] GameObject _backgroundPanel;

        [SerializeField] GameObject _comboContainer;
        [SerializeField] GameObject _comboPresentContainer;

        [SerializeField] GameObject _moneyContainer;


        [SerializeField] GameObject _cardRewardContainer;

        [SerializeField] GameObject _cardSelectionScreen;


        [SerializeField] TextMeshProUGUI _moneyTxt;
        [SerializeField] GameObject _rewardPanel;

        [SerializeField] ComboRecipeUI _comboUI;
        [SerializeField] ObserverSO _observerSO;

   
        bool cardTaken;
        bool goldTaken;
        bool comboTaken;
        private void CheckIfAllIsTaken()
        {
            bool toClose = true;
            toClose &= cardTaken;
            toClose &= goldTaken;
            toClose &= comboTaken;
            if (toClose)
            {
                Invoke("UnNotify", 1f);
                _rewardPanel.SetActive(false);
                _backgroundPanel.SetActive(false);
            }
        }

        private void UnNotify() => _observerSO.Notify(null);
        public void OpenChestScreen(BattleReward rewardBundle)
        {
            _observerSO.Notify(this);
            BattleReward = rewardBundle;
            _rewardPanel.SetActive(true);
            ResetRewardUI();
            _moneyTxt.text = string.Concat(BattleReward.CreditReward);
        }
        public void OpenRewardScreen(BattleReward rewardBundle)
        {
            _observerSO.Notify(this);
            BattleReward = rewardBundle;
            Init();
            _rewardPanel.SetActive(true);
        }
        private void Awake()
        {
            Instance = this;
        }
        public void Init()
        {
            if (BattleReward == null)
                throw new System.Exception("Need To Show Battle Reward but battle reward is null");
            else if (Account.AccountManager.Instance.BattleData.Opponent.CharacterData.Info.CharacterType == CharacterTypeEnum.Boss_Enemy)
                _battleRewardHandler.FinishBoss();


            ResetRewardUI();
            _moneyTxt.text = string.Concat(BattleReward.CreditReward);
        }

        private void ResetRewardUI()
        {

            if (!_rewardPanel.activeSelf)
                _rewardPanel.SetActive(true);

            if (!_moneyContainer.activeSelf)
                _moneyContainer.SetActive(true);



            _backgroundPanel.SetActive(true);

            //if (_cardSelectionScreen.activeSelf)
            //    _cardSelectionScreen.SetActive(false);

            if (!_cardRewardContainer.activeSelf)
                _cardRewardContainer.SetActive(true);

            bool thereIsComboReward = BattleReward.RewardCombos != null && BattleReward.RewardCombos.Length > 0 && BattleReward.RewardCombos[0] != null;



            cardTaken = false;
            goldTaken = false;
            comboTaken = false;
            _comboPresentContainer.SetActive(false);

            if (!thereIsComboReward)
                comboTaken = true;
            else
            {
                _comboUI.InitRecipe(BattleReward.RewardCombos[0]);
            }
            _comboContainer.SetActive(thereIsComboReward);
            _selectCardRewardScreen.AssignRewardCardScreen(BattleReward.RewardCards, _moneyForNotTakingAnything);

        }

        public void AddCombo()
        {
            _battleRewardHandler.AddCombo(BattleReward.RewardCombos);
            _comboPresentContainer.SetActive(false);
            _comboContainer.SetActive(false);
            comboTaken = true;
            CheckIfAllIsTaken();
        }


        public void AddMoney()
        {
            _battleRewardHandler.AddMoney(BattleReward.CreditReward);
            _moneyContainer.SetActive(false);
            goldTaken = true;
            CheckIfAllIsTaken();
        }


        public void ReturnFromCardsSelection()
        {
            _rewardPanel.SetActive(true);
            _backgroundPanel.SetActive(true);
            cardTaken = true;
            _cardSelectionScreen.SetActive(false);
            CheckIfAllIsTaken();
        }

        public void ExitBattle()
        {
            // exit battle


        }

        public void OnNotify(IObserver Myself)
        {

        }

    }
}