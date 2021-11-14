using Battles;
using Battles.UI;
using DesignPattern;
using TMPro;
using UnityEngine;

namespace Rewards.Battles
{
    public class BattleUIRewardHandler : MonoSingleton<BattleUIRewardHandler>, IObserver
    {
        [SerializeField]
        ushort _moneyForNotTakingAnything;
        [Sirenix.OdinInspector.ShowInInspector]
        public BattleReward BattleReward { get; set; }
        [SerializeField] Animator _animator;


        [SerializeField]
        SelectCardRewardScreen _selectCardRewardScreen;
        [SerializeField]
        BattleRewardHandler _battleRewardHandler;


        [SerializeField] GameObject _backgroundPanel;

        [SerializeField] GameObject _comboContainer;

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
                Invoke("UnNotify", 0.3f);
                _rewardPanel.SetActive(false);
                _backgroundPanel.SetActive(false);
            }
        }

        private void UnNotify() => _observerSO.Notify(null);
        internal void OpenRewardScreen(BattleReward rewardBundle)
        {
            _observerSO.Notify(this);
            BattleReward = rewardBundle;
            Init();
            _rewardPanel.SetActive(true);
        }

        public override void Init()
        {
            if (BattleReward == null)
                throw new System.Exception("Need To Show Battle Reward but battle reward is null");
            else if (BattleData.Opponent.CharacterData.Info.CharacterType == CharacterTypeEnum.Boss_Enemy)
                _battleRewardHandler.FinishBoss();


            ResetRewardUI();
            _moneyTxt.text = string.Concat(BattleReward.MoneyReward);
        }

        private void ResetRewardUI()
        {

            if (!_rewardPanel.activeSelf)
                _rewardPanel.SetActive(true);

            if (!_moneyContainer.activeSelf)
                _moneyContainer.SetActive(true);


            _backgroundPanel.SetActive(true);

            if (_cardSelectionScreen.activeSelf)
                _cardSelectionScreen.SetActive(false);

            if (_cardRewardContainer.activeSelf)
                _cardRewardContainer.SetActive(true);
            bool thereIsComboReward = BattleReward.RewardCombos != null && BattleReward.RewardCombos.Length > 0;
            if (_comboContainer.activeSelf != thereIsComboReward)
                _comboContainer.SetActive(thereIsComboReward);

            if (!thereIsComboReward)
                comboTaken = true;
            else
                _comboUI.InitRecipe(BattleReward.RewardCombos[0]);
        }

        public void AddCombo()
        {
            _battleRewardHandler.AddCombo(BattleReward.RewardCombos);
            comboTaken = true;
            CheckIfAllIsTaken();
        }


        public void AddMoney()
        {
            _battleRewardHandler.AddMoney(BattleReward.MoneyReward);
            _moneyContainer.SetActive(false);
            goldTaken = true;
            CheckIfAllIsTaken();
        }


        public void ShowCards()
        {
            _backgroundPanel.SetActive(false);
            _rewardPanel.SetActive(false);
            _selectCardRewardScreen.OnOpenCardUI(BattleReward.RewardCards, _moneyForNotTakingAnything);
        }

        public void ReturnFromCardsSelection()
        {
            _rewardPanel.SetActive(true);
            _backgroundPanel.SetActive(true);
            cardTaken = true;
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