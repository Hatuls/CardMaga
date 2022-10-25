using Keywords;
namespace Characters.Stats
{
    public class StrengthStat : BaseStat
    {
        public StrengthStat(int amount) : base(amount)
        {
        }

        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Strength;

        public override void Reduce(int amount)
        {
            base.Reduce(amount);

            if (Amount < 0)
                Amount = 0;
        }

    }

}