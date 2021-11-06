using Keywords;
namespace Characters.Stats
{
    public class RageStat : StatAbst
    {
        public RageStat(bool isPlayer, int amount) : base(isPlayer, amount)
        {
        }

        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Rage;
    }
}