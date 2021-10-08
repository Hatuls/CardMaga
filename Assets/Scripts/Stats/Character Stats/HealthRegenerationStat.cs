using Keywords;
namespace Characters.Stats
{
    public class HealthRegenerationStat : StatAbst
    {
        public HealthRegenerationStat(bool isPlayer,  int amount) : base(isPlayer,  amount)
        {
        }

        public override void Reduce(int amount)
        {
            if(Amount - amount >=0)
            base.Reduce(amount);
        }
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Regeneration;
    }

}