using Battle;
using DesignPattern;
using System;
using UnityEngine;
using CardMaga;
using CardMaga.Map;
using Battle.Combo;
using CardMaga.Card;

namespace Rewards.Battles
{

    public class BattleRewardHandler : MonoBehaviour ,IObserver
    {
        [SerializeField]
        MapPlayerTracker _mapTracker;
        [SerializeField] BattleUIRewardHandler _battleUIRewardHandler;
        [SerializeField] ObserverSO _observerSO;
        [SerializeField] MoneyIcon _moneyIcon;
        [SerializeField] EndRunScreen _endRunScreen;


        // Need To be Re-Done
        private void Start() { 
        //{
        //    var data = Account.AccountManager.Instance.BattleData;
        //   if (!data.PlayerWon)
        //         return;
        //    data.PlayerWon = false;



        //    var opponentType = data.Opponent.CharacterData.CharacterSO.CharacterType;

        //    if (opponentType == CharacterTypeEnum.Boss_Enemy)
        //    {
        //        FinishBoss();
        //        return;
        //    }
        //    _observerSO.Notify(this);
        //    var rewardBundle = Factory.GameFactory.Instance.RewardFactoryHandler.GetBattleRewards(opponentType, _mapTracker.CurrentAct,data.Player.CharacterData.ComboRecipe);
        //    if (rewardBundle == null)
        //        throw new Exception("Reward Bundle is null!");



        //    _battleUIRewardHandler.OpenRewardScreen(rewardBundle);
        }

        // Need To be Re-Done
        public void ReturnToMainMenu()
        {

           // Account.AccountManager.Instance.BattleData.Player = null;
        //    SceneHandler.LoadScene(SceneHandler.ScenesEnum.MainMenuScene);
        }
        // Need To be Re-Done
        public void AddCard(CardData cardToAdd)
        {
         //   Account.AccountManager.Instance.BattleData.Player.AddCardToDeck(cardToAdd);
        }
        // Need To be Re-Done
        public void AddMoney(int amount)
        {


          //  Account.AccountManager.Instance.BattleData.Player.CharacterData.CharacterStats.Gold += (ushort)amount;
          //  _moneyIcon.SetMoneyText(Account.AccountManager.Instance.BattleData.Player.CharacterData.CharacterStats.Gold);
       
        }
        internal void FinishBoss()
        {
            // see when boss is fnished!
            //     ReturnToMainMenu();
            _endRunScreen.FinishGame();
        }
        // Need To be Re-Done
        internal void AddCombo(Combo[] rewardCombos)
        {
        //  bool recievedSuccessfully = Account.AccountManager.Instance.BattleData.Player.AddComboRecipe(rewardCombos[0]);
        }

        public void OnNotify(IObserver Myself)
        {
            throw new NotImplementedException();
        }
    }
}