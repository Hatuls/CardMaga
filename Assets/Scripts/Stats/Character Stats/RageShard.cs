using CardMaga.Keywords;
using Keywords;
namespace Characters.Stats
{
    public class RageShard : BaseStat
    {
        byte _maxShardSize;
        private RageStat _rageStat;
        public RageShard(int amount,RageStat rageStat) : base( amount)
        {
            _maxShardSize = Factory.GameFactory.Instance.KeywordFactoryHandler.GetKeywordSO(Keyword).InfoAmount;
            _rageStat = rageStat;
        }

        public override KeywordType Keyword => KeywordType.RageShard;

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