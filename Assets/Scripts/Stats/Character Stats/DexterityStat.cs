using Keywords;
namespace Characters.Stats
{
    public class DexterityStat : StatAbst
    {
        public DexterityStat(bool isPlayer ,  int amount) : base(isPlayer,  amount)
        {

        }

        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Dexterity;

        public override void Add(int amount)
        {
            base.Add(amount);
        }
        public override void Reduce(int amount)
        {
            if (Amount - amount <= 0)
                base.Reset();
            else
                base.Reduce(amount);

        }

    }

}