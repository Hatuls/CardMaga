using Battles;
using UnityEngine;

namespace Rewards.Battles
{

    public class BattleRewardHandler : MonoSingleton<BattleRewardHandler>
    {
        [SerializeField] BattleData data;

        public override void Init()
        {
            if (data == null)
                throw new System.Exception("BattleRewardHandler : Battle Data was not assigned in inspector!");
        }

       public void FinishMatch(bool playerWon)
        {
            if (playerWon)
            {
                var rewardBundle = Factory.GameFactory.Instance.RewardFactoryHandler.GetBattleRewards(data.UseSO ? data.OpponentTwo.CharacterType: data.OpponentCharacterData.Info.CharacterType );
                if (rewardBundle == null)
                    throw new System.Exception("Reward Bundle is null!");

                BattleUIRewardHandler.Instance.BattleReward = rewardBundle;
            }

        }

        public void AddCard(Cards.Card cardToAdd)
        {
            data.PlayerCharacterData.AddCardToDeck(cardToAdd);


        }

        public void AddMoney(int amount)
        {
            var playerStats = Characters.Stats.CharacterStatsManager.GetCharacterStatsHandler(true);



            playerStats.GetStats(Keywords.KeywordTypeEnum.Coins).Add(amount);


            var x = data.PlayerCharacterData;


            x.CharacterStats.Gold = playerStats.GetStats(Keywords.KeywordTypeEnum.Coins).Amount;

            data.UpdatePlayerCharacter(x);
        }
    }
}