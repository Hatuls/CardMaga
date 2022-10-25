﻿using Keywords;
namespace Characters.Stats
{
    public class DexterityStat : BaseStat
    {
        public DexterityStat(int amount) : base(amount)
        {

        }
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Dexterity;

        public override void Reduce(int amount)
        {
            if (Amount - amount <= 0)
                base.Reset();
            else
                base.Reduce(amount);

        }
    }
}