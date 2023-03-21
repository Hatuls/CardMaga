using CardMaga.Keywords;

namespace Characters.Stats
{
    public class MaxHealthStat : BaseStat
    {
        private HealthStat _healthStat;

        public HealthStat HealthStat { get => _healthStat; set => _healthStat = value; }
        public override KeywordType Keyword => KeywordType.MaxHealth;
        public MaxHealthStat(int amount) : base(amount)
        {

        }
        public override void Reduce(int amount)
        {
            base.Reduce(amount);
            if (Amount <= 0)
                Amount = 1;

            if (HealthStat.Amount > Amount)
            {
                HealthStat.Reset(Amount);
            }
            // if amount is less than hp reduce the hp 
        }
        public override void Add(int amount)
        {
            base.Add(amount);

            HealthStat.Add(amount);
        }


    }
}