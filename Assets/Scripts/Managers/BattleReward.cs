using Cards;
namespace Rewards
{
    public class BattleReward
    {
        public int MoneyReward { get; private set; }
        public Card[] RewardCards { get; private set; }
        public BattleReward(int money, params Card[] cards)
        {
            MoneyReward = money;
            RewardCards = cards;
        }
    }


}

