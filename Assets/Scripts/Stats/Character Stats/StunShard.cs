using CardMaga.Keywords;
namespace Characters.Stats
{
    public class StunShard : BaseStat
    {
        int _maxShardSize;
        StunStat _stunStat;
        public StunShard(int amount, StunStat stun) : base(amount)
        {
            _stunStat = stun;
            _maxShardSize = Factory.GameFactory.Instance.KeywordFactoryHandler.GetKeywordSO(Keyword).InfoAmount;
        }

        public override KeywordType Keyword => KeywordType.StunShard;

        public override void Add(int amount)
        {
            base.Add(amount);

            if (Amount >= _maxShardSize)
            {

                _stunStat.Add(1);
                Reduce(_maxShardSize);
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