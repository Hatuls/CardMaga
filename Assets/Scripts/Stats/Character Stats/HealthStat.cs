using Keywords;
using System;
namespace Characters.Stats
{
    public class HealthStat : BaseStat
    {
        public static event Action<bool> OnCharacterDeath;
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Heal;
        MaxHealthStat _maxHealthStats;
        public HealthStat(MaxHealthStat maxHealth, bool isPlayer, int amount) : base(isPlayer, amount)
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

            OnStatsUpdated.Invoke(isPlayer, Amount, Keyword);
        }
        public override void Reduce(int amount)
        {
            if (Amount <= 0)
            {
                throw new System.Exception($"HealthStat: Trying To Reduce Health Amount when its already a 0! Amount: {amount}");

            }

            base.Reduce(amount);

            if (Amount <= 0)
            {
                OnCharacterDeath?.Invoke(isPlayer);
            }

        }
    }

}