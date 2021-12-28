using Cards;
using Combo;
namespace Rewards
{
    public class BattleReward
    {
        public ushort CreditReward { get; private set; }
        public ushort GoldReward { get; private set; }
        public Card[] RewardCards { get; private set; }
        public Combo.Combo[] RewardCombos { get; private set; }

        public BattleReward(ushort Credit , ushort Gold, Card[] cards, Combo.Combo[] combos= null)
        {
            CreditReward = Credit;
         
            GoldReward = Gold;
  

            RewardCards = cards;
            RewardCombos = combos;
        }
    }

    public class RunReward
    {
        public ushort EXPReward { get; private set; }
        public ushort DiamondsReward { get; private set; }
        public RunReward(ushort eXPReward, ushort diamondsReward)
        {
            EXPReward = eXPReward;
            DiamondsReward = diamondsReward;
        }
    }

}

