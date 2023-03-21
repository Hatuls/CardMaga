using CardMaga.Keywords;
using UnityEngine;

namespace Characters.Stats
{
    //Not Active
    public class DefenseStat : BaseStat
    {
        BaseStat _healStat;
        public DefenseStat(BaseStat health, int amount) : base(amount)
        {
            _healStat = health;
        }

        public override KeywordType Keyword => KeywordType.Shield;

        public override void Reduce(int amount)
        {
            int difference = Amount - amount;
            base.Reduce(amount);
            if (Amount < 0)
                Amount = 0;

            if (difference < 0)
                _healStat.Reduce(Mathf.Abs(difference));
          
        }
    }

}