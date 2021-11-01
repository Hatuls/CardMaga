using Battles;
using UnityEngine;

namespace Rewards.Battles
{

    public class BattleRewardHandler : MonoSingleton<BattleRewardHandler>
    {

        [SerializeField] SceneLoaderCallback sceneLoader;
        public override void Init()
        {
         }

       public void FinishMatch(bool playerWon)
        {
            if (playerWon)
            {
                var rewardBundle = Factory.GameFactory.Instance.RewardFactoryHandler.GetBattleRewards( BattleData.Opponent.CharacterData.Info.CharacterType);
                if (rewardBundle == null)
                    throw new System.Exception("Reward Bundle is null!");

                BattleUIRewardHandler.Instance.BattleReward = rewardBundle;
            }
            else
            {
                BattleData.Player = null;
                sceneLoader.LoadScene((int)SceneHandler.ScenesEnum.MainMenuScene);
            }
          
        }

        public void AddCard(Cards.Card cardToAdd)
        {
            BattleData.Player.AddCardToDeck(cardToAdd);


        }

        public void AddMoney(int amount)
        {
            var playerStats = Characters.Stats.CharacterStatsManager.GetCharacterStatsHandler(true);

            playerStats.GetStats(Keywords.KeywordTypeEnum.Coins).Add(amount);

            var player = BattleData.Player;

            player.CharacterData.CharacterStats.Gold = (ushort)playerStats.GetStats(Keywords.KeywordTypeEnum.Coins).Amount;

            BattleData.Player =player;
        }
    }
}