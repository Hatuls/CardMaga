using Keywords;
namespace Characters.Stats
{
    public class DrawCardStat : BaseStat
    {
        public DrawCardStat(int amount) : base( amount)
        {
        }

        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Draw;
    }

}