using Keywords;
namespace Characters.Stats
{
    public class DefenseStat : BaseStat
    {
        public DefenseStat( int amount) : base( amount)
        {
        }

        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Shield;

        public override void Reduce(int amount)
        {
            base.Reduce(amount);

            if (Amount < 0)
                Amount = 0;

            // transfer damage to Health
        }
    }

}