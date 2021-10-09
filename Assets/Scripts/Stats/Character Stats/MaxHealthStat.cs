using Keywords;
namespace Characters.Stats
{
    public class MaxHealthStat : StatAbst
    {
       public HealthStat _healthStat { get; set; }
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.MaxHealth;
        public MaxHealthStat( bool isPlayer,  int amount) : base(isPlayer,  amount)
        {

        }
        public override void Reduce(int amount)
        {
            base.Reduce(amount);
            if (Amount <= 0)
                Amount = 1;

            if (_healthStat.Amount > Amount)
            {
                _healthStat.Reset(Amount);
            }
            // if amount is less than hp reduce the hp 

        }
        public override void Add(int amount)
        {
            base.Add(amount);
            _healthStat.Add(amount);
            // add to health;
        }

    }

}