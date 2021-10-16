using Cards;
using Combo;
namespace Rewards
{
    public class BattleReward
    {
        public int MoneyReward { get; private set; }
        public Card[] RewardCards { get; private set; }

        public Combo.Combo[] RewardCombos { get; private set; }
        public BattleReward(int money, Card[] cards, Combo.Combo[] combos= null)
        {
            MoneyReward = money;
            RewardCards = cards;
            RewardCombos = combos;
        }
    }


}

