using Keywords;
namespace Characters.Stats
{
    public class StunShard : StatAbst
    {
         int _maxShardSize;
        
        public StunShard(bool isPlayer, int amount) : base(isPlayer, amount)
        {
            _maxShardSize = Factory.GameFactory.Instance.KeywordSOHandler.GetKeywordSO(Keyword).InfoAmount;
        }

        public override KeywordTypeEnum Keyword => KeywordTypeEnum.StunShard;

        public override void Add(int amount)
        {
            base.Add(amount);

            if (Amount >= _maxShardSize)
            {
                CharacterStatsManager.GetCharacterStatsHandler(isPlayer).GetStats(KeywordTypeEnum.Stun).Add(1);
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