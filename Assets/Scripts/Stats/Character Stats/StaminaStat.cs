using CardMaga.Keywords;

namespace Characters.Stats
{
    public class StaminaStat : BaseStat
    {
        public StaminaStat(int amount) : base(amount)
        {
        }

        public override KeywordType Keyword => KeywordType.Stamina;
    }
}