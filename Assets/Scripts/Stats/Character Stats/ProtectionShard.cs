using CardMaga.Keywords;
namespace Characters.Stats
{
    public class ProtectionShard : BaseStat
    {
        byte _maxShardSize;
        private ProtectedStat _protectionStat;
        public ProtectionShard(int amount, ProtectedStat protectedStat) : base(amount)
        {
            _protectionStat = protectedStat;
            _maxShardSize = Factory.GameFactory.Instance.KeywordFactoryHandler.GetKeywordSO(Keyword).InfoAmount;
        }

        public override KeywordType Keyword => KeywordType.ProtectionShard;

        public override void Add(int amount)
        {
            base.Add(amount);

            if (Amount >= _maxShardSize)
            {
                _protectionStat.Add(1);
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