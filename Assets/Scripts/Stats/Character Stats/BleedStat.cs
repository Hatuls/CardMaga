﻿using Keywords;
namespace Characters.Stats
{
    public class BleedStat : BaseStat
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Bleed;
        public BleedStat(bool isPlayer, int amount) : base(isPlayer, amount)
        {
        }

        public override void Reduce(int amount)
        {
            if (Amount - amount >= 0)
                base.Reduce(amount);
        }
    }

}