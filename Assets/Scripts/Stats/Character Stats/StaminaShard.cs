using Keywords;
namespace Characters.Stats
{
    public class StaminaShard : BaseStat
    {
        byte _maxShardSize;

        public StaminaShard(bool isPlayer, int amount) : base(isPlayer, amount)
        {
            _maxShardSize = Factory.GameFactory.Instance.KeywordSOHandler.GetKeywordSO(Keyword).InfoAmount;
        }

        public override KeywordTypeEnum Keyword => KeywordTypeEnum.StaminaShards;

        public override void Add(int amount)
        {
            base.Add(amount);

            if (Amount >= _maxShardSize)
            {
                StaminaHandler.Instance.AddStartStamina(isPlayer, 1);
                CharacterStatsManager.GetCharacterStatsHandler(isPlayer).GetStats(KeywordTypeEnum.Stamina).Add(1);
                int remain = Amount - _maxShardSize;
                Reset();
                Add(remain);
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