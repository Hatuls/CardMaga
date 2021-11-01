using Battles.UI;
using UnityEngine;
using TMPro;
namespace Rewards.Battles
{
    public class BattleUIRewardHandler : MonoSingleton<BattleUIRewardHandler>
    {
        [Sirenix.OdinInspector.ShowInInspector]
        public BattleReward BattleReward{get;set;}
        [SerializeField] Animator _animator;


        [SerializeField] GameObject _rewardScreen;

        [SerializeField] GameObject _rewardGroupsContainer;


        [SerializeField] GameObject _moneyContainer;
        [SerializeField] GameObject _moneyBtn;

        [SerializeField] GameObject _cardContainer;
        [SerializeField] GameObject _cardSelection;

        [SerializeField] CardUI[] _cards;

        [SerializeField] TextMeshProUGUI _moneyTxt;
        [SerializeField] TextMeshProUGUI _title;

        [SerializeField] SceneLoaderCallback _sceneloaderEvent;
        public override void Init()
        {
            _rewardScreen.SetActive(false);
        }
   
        public void ShowBattleRewardUI(bool isPlayer)
        {
            if (!isPlayer)
            {
            if (BattleReward == null)
                throw new System.Exception("Need To Show Battle Reward but battle reward is null");

            ResetRewardUI();
            _moneyTxt.text = string.Concat(BattleReward.MoneyReward , " Coins");
            }
            else
            {
          
                LoadingManager.Instance.LoadScene(SceneHandler.ScenesEnum.GameBattleScene);
            }
        }
        private void ResetRewardUI()
        {
            _rewardScreen.SetActive(true);
            _moneyContainer.SetActive(true);
            _rewardGroupsContainer.SetActive(true);
            _cardSelection.SetActive(false);
            _cardContainer.SetActive(true);
            _title.text = "Victory!";
        }




        public void AddMoney()
        {
            BattleRewardHandler.Instance.AddMoney(BattleReward.MoneyReward);
            _moneyContainer.SetActive(false);

        }


        public void ShowCards()
        {
            _rewardGroupsContainer.SetActive(false);

            for (int i = 0; i < _cards.Length; i++)
                CardUIManager.Instance.AssignDataToCardUI(_cards[i], BattleReward.RewardCards[i]);

            _title.text = "Choose A Card!";
            _cardSelection.SetActive(true);
        }


        public void SelectCard(int selectionIndex)
        {
            BattleRewardHandler.Instance.AddCard(BattleReward.RewardCards[selectionIndex]);
            ReturnFromCardsSelection();
        }

        public void ReturnFromCardsSelection()
        {
            _title.text = "Victory!";
            _cardSelection.SetActive(false);
            _cardContainer.SetActive(false);
            _rewardGroupsContainer.SetActive(true);
        }

        public void ExitBattle()
        {
            // exit battle
            _sceneloaderEvent?.LoadScene(SceneHandler.ScenesEnum.MapScene);

        }
    }
}