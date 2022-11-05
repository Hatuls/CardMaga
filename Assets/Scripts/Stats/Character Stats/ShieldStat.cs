﻿using CardMaga.Keywords;
using Keywords;
namespace Characters.Stats
{
    public class ShieldStat : BaseStat
    {

        HealthStat _health;
       
        public override KeywordType Keyword => KeywordType.Shield;
        public ShieldStat(HealthStat health, int amount) : base(amount)
        {
            _health = health;
         
        }

        public override void Add(int amount)
        {
            // add dexterity
         
            base.Add(amount);
        }
        public override void Reduce(int amount)
        {
            if (Amount - amount >= 0)
                base.Reduce(amount);
            else
            {
                int remaining = UnityEngine.Mathf.Abs(Amount - amount);
                Reset();
                _health.Reduce(remaining);
            }
        }
    }

}