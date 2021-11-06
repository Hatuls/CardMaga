using Keywords;
namespace Characters.Stats
{
    public class ProtectedStat : StatAbst
    {
        public ProtectedStat(bool isPlayer, int amount) : base(isPlayer, amount)
        {
        }

        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Protected;
    }
}