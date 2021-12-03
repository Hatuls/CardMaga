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


    public class WeakStat : StatAbst
    {
        public WeakStat(bool isPlayer, int amount) : base(isPlayer, amount)
        {
        }

        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Weak;
    }
    public class VulnerableKeyword : StatAbst
    {
        public VulnerableKeyword(bool isPlayer, int amount) : base(isPlayer, amount)
        {
        }

        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Vulnerable;
    }
}