using Battle.Combo;
using CardMaga.Card;

namespace Rewards
{
    public class BattleReward
    {
        public ushort CreditReward { get; private set; }
        public ushort GoldReward { get; private set; }
        public CardData[] RewardCards { get; private set; }
        public Combo[] RewardCombos { get; private set; }

        public BattleReward(ushort Credit , ushort Gold, CardData[] cards,Combo[] combos= null)
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

