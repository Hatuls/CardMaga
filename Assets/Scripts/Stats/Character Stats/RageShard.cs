using Keywords;
namespace Characters.Stats
{
    public class RageShard : BaseStat
    {
        byte _maxShardSize;
        private RageStat _rageStat;
        public RageShard(bool isPlayer, int amount,RageStat rageStat) : base(isPlayer, amount)
        {
            _maxShardSize = Factory.GameFactory.Instance.KeywordSOHandler.GetKeywordSO(Keyword).InfoAmount;
            _rageStat = rageStat;
        }

        public override KeywordTypeEnum Keyword => KeywordTypeEnum.RageShard;

        public override void Add(int amount)
        {
            base.Add(amount);

            if (Amount >= _maxShardSize)
            {
                _rageStat.Add(1);
                Reset();
            }
        }
        public override void Reduce(int value)
        {
            if (Amount - value <= 0)
            {
                Reset();
                return;
            }
            base.Reduce(value);

        }
    }

}