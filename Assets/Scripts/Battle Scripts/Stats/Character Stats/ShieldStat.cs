using Keywords;
namespace Characters.Stats
{
    public class ShieldStat : StatAbst
    {

        HealthStat _health;
        DexterityStat _dexterity;
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Shield;
        public ShieldStat(HealthStat health,DexterityStat dex, bool isPlayer,  int amount) : base(isPlayer,  amount)
        {
            _health = health;
            _dexterity = dex;
        }

        public override void Add(int amount)
        {
            // add dexterity
            amount += _dexterity.Amount;
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