using Cards;
using Combo;
namespace Rewards
{
    public class BattleReward
    {
        public ushort EXPReward { get; private set; }
        public ushort CreditReward { get; private set; }
        public ushort GoldReward { get; private set; }
        public ushort DiamondsReward { get; private set; }
        public Card[] RewardCards { get; private set; }
        public Combo.Combo[] RewardCombos { get; private set; }

        public BattleReward(ushort Credit , ushort Exp, ushort Gold, ushort Diamonds, Card[] cards, Combo.Combo[] combos= null)
        {
            CreditReward = Credit;
            EXPReward = Exp;
            GoldReward = Gold;
            DiamondsReward = Diamonds;

            RewardCards = cards;
            RewardCombos = combos;
        }
    }


}

