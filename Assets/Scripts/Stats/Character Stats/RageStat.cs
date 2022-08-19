using Keywords;
namespace Characters.Stats
{
    public class RageStat : BaseStat
    {
        public RageStat(bool isPlayer, int amount) : base(isPlayer, amount)
        {
        }

        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Rage;
    }
}