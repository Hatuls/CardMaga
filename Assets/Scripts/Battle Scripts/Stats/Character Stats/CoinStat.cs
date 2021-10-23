using Keywords;
namespace Characters.Stats
{
    public class CoinStat : StatAbst
    {
        public CoinStat(bool isPlayer,  int amount) : base(isPlayer,  amount)
        {
        }

        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Coins;
        public override void Reduce(int amount)
        {
            if (Amount - amount < 0)
                throw new System.Exception("Not Enough Coins to Reduce!");
            base.Reduce(amount);
        }
    }

}