using Keywords;
namespace Characters.Stats
{
    public class DrawCardStat : StatAbst
    {
        public DrawCardStat(bool isPlayer,  int amount) : base(isPlayer,  amount)
        {
        }

        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Draw;
    }

}