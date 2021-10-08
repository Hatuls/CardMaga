using Keywords;
namespace Characters.Stats
{
    public class DefenseStat : StatAbst
    {
        public DefenseStat(bool isPlayer,  int amount) : base(isPlayer,  amount)
        {
        }

        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Shield;



        public override void Reduce(int amount)
        {
            base.Reduce(amount);

            if (Amount < 0)
                Amount = 0;

            // transfer damage to Health
        }
    }

}