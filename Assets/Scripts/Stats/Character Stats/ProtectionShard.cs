using Keywords;
namespace Characters.Stats
{
    public class ProtectionShard : BaseStat
    {
        byte _maxShardSize;

        public ProtectionShard(bool isPlayer, int amount) : base(isPlayer, amount)
        {
            _maxShardSize = Factory.GameFactory.Instance.KeywordSOHandler.GetKeywordSO(Keyword).InfoAmount;
        }

        public override KeywordTypeEnum Keyword => KeywordTypeEnum.ProtectionShard;

        public override void Add(int amount)
        {
            base.Add(amount);

            if (Amount >= _maxShardSize)
            {
                CharacterStatsManager.GetCharacterStatsHandler(isPlayer).GetStats(KeywordTypeEnum.Protected).Add(1);
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