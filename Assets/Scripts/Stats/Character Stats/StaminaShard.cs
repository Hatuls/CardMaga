using CardMaga.Keywords;
namespace Characters.Stats
{
    public class StaminaShard : BaseStat
    {
        byte _maxShardSize;
        private StaminaHandler _staminaHandler;
        private StaminaStat _staminaStat;
        public StaminaShard(int amount, StaminaHandler staminaHandler, StaminaStat staminaStat) : base(amount)
        {
            _maxShardSize = Factory.GameFactory.Instance.KeywordFactoryHandler.GetKeywordSO(Keyword).InfoAmount;
            _staminaHandler = staminaHandler;
            _staminaStat = staminaStat;
        }

        public override KeywordType Keyword => KeywordType.StaminaShards;

        public override void Add(int amount)
        {
            base.Add(amount);

            if (Amount >= _maxShardSize)
            {
                _staminaHandler.AddStaminaAddition(1);

                _staminaStat.Add(1);
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