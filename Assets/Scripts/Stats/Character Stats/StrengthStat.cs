using CardMaga.Keywords;
using Keywords;
namespace Characters.Stats
{
    public class StrengthStat : BaseStat
    {
        public StrengthStat(int amount) : base(amount)
        {
        }

        public override KeywordType Keyword => KeywordType.Strength;

        public override void Reduce(int amount)
        {
            base.Reduce(amount);

            if (Amount < 0)
                Amount = 0;
        }

    }

}