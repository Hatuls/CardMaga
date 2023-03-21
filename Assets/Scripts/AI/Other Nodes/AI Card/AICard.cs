using CardMaga.Card;
using System;
namespace CardMaga.AI
{
    [Serializable]
    public class AICard : IWeightable
    {
        public BattleCardData BattleCard { get; private set; }
        public int Weight { get; set; }

        public void AssignCard(BattleCardData battleCard) => BattleCard = battleCard;
        public void Reset()
        {
            BattleCard = null;
            Weight = 0;
        }
    }

}