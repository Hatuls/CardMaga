using Keywords;
namespace Characters.Stats
{
    public class HealthStat : StatAbst
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Heal;
        MaxHealthStat _maxHealthStats;
        public HealthStat(MaxHealthStat maxHealth,bool isPlayer , int amount) : base(isPlayer, amount)
        {
            _maxHealthStats = maxHealth;
        }
        public override void Reset(int value = 0)
        {
            base.Reset(value);
        }
        public override void Add(int amount)
        {
            Amount += amount;
            if (Amount > _maxHealthStats.Amount)
                Amount = _maxHealthStats.Amount;

            _updateUIStats.Invoke(isPlayer, Amount, Keyword);
        }
        public override void Reduce(int amount)
        {
            base.Reduce(amount);

            if (Amount <= 0)
            {
                Battles.BattleManager.BattleEnded(isPlayer);
            }

        }
    }

}