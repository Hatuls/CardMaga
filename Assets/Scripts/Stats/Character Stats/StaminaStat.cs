
using Keywords;

namespace Characters.Stats
{
    public class StaminaStat : StatAbst
    {
        public StaminaStat(bool isPalyer,  int amount) : base(isPalyer,  amount)
        {
        }

        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Stamina;
    }
}