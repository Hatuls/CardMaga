
using Keywords;

namespace Characters.Stats
{
    public class StaminaStat : BaseStat
    {
        public StaminaStat(bool isPalyer,  int amount) : base(isPalyer,  amount)
        {
        }

        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Stamina;
    }
}