using Keywords;
namespace Characters.Stats
{
    public class StunStat : StatAbst
    {
        public StunStat(bool isPlayer, int amount = 0) : base(isPlayer, amount)
        {
        }

        public override KeywordTypeEnum Keyword =>  KeywordTypeEnum.Stun;

        public override void Add(int amount)
        {
            base.Add(amount);
        }

        public override bool HasValue()
        {
            return base.HasValue();
        }

        public override bool HasValue(int amount)
        {
            return base.HasValue(amount);
        }

        public override void Reduce(int amount)
        {
            base.Reduce(amount);
        }

        public override void Reset(int value = 0)
        {
            base.Reset(value);
        }
    }
}