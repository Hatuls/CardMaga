using Keywords;
namespace Characters.Stats
{
    public class DrawCardStat : BaseStat
    {
        public DrawCardStat(bool isPlayer,  int amount) : base(isPlayer,  amount)
        {
        }

        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Draw;
    }

}