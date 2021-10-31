using Keywords;
namespace Characters.Stats
{
    public class CoinStat : StatAbst
    {
        public CoinStat(bool isPlayer, int amount) : base(isPlayer,  amount)
        {
        }

        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Coins;
        public override void Reduce(int amount)
        {
            if (Amount - amount < 0)
                Amount = 0;
            else
                base.Reduce(amount);
        }
    }

}