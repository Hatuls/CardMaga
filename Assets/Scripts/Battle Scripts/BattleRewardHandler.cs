using Battles;
using DesignPattern;
using System;
using UnityEngine;

namespace Rewards.Battles
{

    public class BattleRewardHandler : MonoBehaviour ,IObserver
    {
        [SerializeField] BattleUIRewardHandler _battleUIRewardHandler;
        [SerializeField] ObserverSO _observerSO;
        [SerializeField] MoneyIcon _moneyIcon;
        private void Start()
        {
            if (!BattleData.PlayerWon)
                return;

            BattleData.PlayerWon = false;

            var opponentType = BattleData.Opponent.CharacterData.Info.CharacterType;

            if (opponentType == CharacterTypeEnum.Boss_Enemy)
            {
                FinishBoss();
                return;
            }
            _observerSO.Notify(this);
            var rewardBundle = Factory.GameFactory.Instance.RewardFactoryHandler.GetBattleRewards(opponentType);
            if (rewardBundle == null)
                throw new Exception("Reward Bundle is null!");

            _battleUIRewardHandler.OpenRewardScreen(rewardBundle);
        }

        public void ReturnToMainMenu()
        {

            BattleData.Player = null;
            SceneHandler.LoadScene(SceneHandler.ScenesEnum.MainMenuScene);
        }
        public void AddCard(Cards.Card cardToAdd)
        {
            BattleData.Player.AddCardToDeck(cardToAdd);
        }

        public void AddMoney(int amount)
        {
      

            BattleData.Player.CharacterData.CharacterStats.Gold += (ushort)amount;
            _moneyIcon.SetMoneyText(BattleData.Player.CharacterData.CharacterStats.Gold);
       
        }

        internal void FinishBoss()
        {
            // see when boss is fnished!
            ReturnToMainMenu();
        }

        internal void AddCombo(Combo.Combo[] rewardCombos)
        {
          bool recievedSuccessfully =  BattleData.Player.AddComboRecipe(rewardCombos[0]);
        }

        public void OnNotify(IObserver Myself)
        {
            throw new NotImplementedException();
        }
    }
}