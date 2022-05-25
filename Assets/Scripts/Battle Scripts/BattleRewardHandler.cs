using Battles;
using DesignPattern;
using System;
using UnityEngine;
using Map;
namespace Rewards.Battles
{

    public class BattleRewardHandler : MonoBehaviour ,IObserver
    {
        [SerializeField]
        MapPlayerTracker _mapTracker;
        [SerializeField] BattleUIRewardHandler _battleUIRewardHandler;
        [SerializeField] ObserverSO _observerSO;
        [SerializeField] MoneyIcon _moneyIcon;
        private void Start()
        {
            var data = Account.AccountManager.Instance.BattleData;
            if (!data.PlayerWon)
                return;
            data.PlayerWon = false;



            var opponentType = data.Opponent.CharacterData.CharacterSO.CharacterType;

            if (opponentType == CharacterTypeEnum.Boss_Enemy)
            {
                FinishBoss();
                return;
            }
            _observerSO.Notify(this);
            var rewardBundle = Factory.GameFactory.Instance.RewardFactoryHandler.GetBattleRewards(opponentType, _mapTracker.CurrentAct,data.Player.CharacterData.ComboRecipe);
            if (rewardBundle == null)
                throw new Exception("Reward Bundle is null!");



            _battleUIRewardHandler.OpenRewardScreen(rewardBundle);
        }

    
        public void ReturnToMainMenu()
        {

            Account.AccountManager.Instance.BattleData.Player = null;
        //    SceneHandler.LoadScene(SceneHandler.ScenesEnum.MainMenuScene);
        }
        public void AddCard(Cards.Card cardToAdd)
        {
            Account.AccountManager.Instance.BattleData.Player.AddCardToDeck(cardToAdd);
        }

        public void AddMoney(int amount)
        {


            Account.AccountManager.Instance.BattleData.Player.CharacterData.CharacterStats.Gold += (ushort)amount;
            _moneyIcon.SetMoneyText(Account.AccountManager.Instance.BattleData.Player.CharacterData.CharacterStats.Gold);
       
        }
        [SerializeField] EndRunScreen _endRunScreen;
        internal void FinishBoss()
        {
            // see when boss is fnished!
            //     ReturnToMainMenu();
            _endRunScreen.FinishGame();
        }

        internal void AddCombo(Combo.Combo[] rewardCombos)
        {
          bool recievedSuccessfully = Account.AccountManager.Instance.BattleData.Player.AddComboRecipe(rewardCombos[0]);
        }

        public void OnNotify(IObserver Myself)
        {
            throw new NotImplementedException();
        }
    }
}