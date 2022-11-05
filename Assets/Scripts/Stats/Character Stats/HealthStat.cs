using CardMaga.Keywords;
using Keywords;
using System;
namespace Characters.Stats
{
    public class HealthStat : BaseStat
    {
        public static event Action<bool> OnCharacterDeath;
        private  MaxHealthStat _maxHealthStats;
        private bool _isPlayer;
        public override KeywordType Keyword => KeywordType.Heal;
        public HealthStat(MaxHealthStat maxHealth,bool isPlayer, int amount) : base(amount)
        {
            _isPlayer = isPlayer;
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

            InvokeStatUpdate();
        }
        public override void Reduce(int amount)
        {
            if (Amount <= 0)
                return;

            base.Reduce(amount);

            if (Amount <= 0)
                OnCharacterDeath?.Invoke(_isPlayer);
            

        }

        public override bool IsEmpty => Amount <= 0; 
    }

}