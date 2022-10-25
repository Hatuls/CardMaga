﻿using Keywords;
namespace Characters.Stats
{
    public class RageStat : BaseStat
    {
        public RageStat( int amount) : base( amount)
        {
        }

        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Rage;
    }
}