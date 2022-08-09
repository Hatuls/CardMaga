using Keywords;
namespace Characters.Stats
{
    public class StrengthStat : StatAbst
    {
        public StrengthStat(bool isPlayer,  int amount) : base(isPlayer,  amount)
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