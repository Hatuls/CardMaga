using CardMaga.Battle;

namespace CardMaga.Keywords
{
    public class MaxHealthKeyword : BaseKeywordLogic
    {
        public MaxHealthKeyword(KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
        }


        public override void ProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {
            if (target == TargetEnum.MySelf || target == TargetEnum.All)
            {
                var maxHealth = _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType);
                if (amount > 0)
                    maxHealth.Add(amount);
                else
                    maxHealth.Reduce(-1 * amount);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                var maxHealth = _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType);
                if (amount > 0)
                    maxHealth.Add(amount);
                else
                    maxHealth.Reduce(-1 *amount);
            }
            KeywordSO.SoundEventSO.PlaySound();
        }

      

        public override void UnProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {
            if (target == TargetEnum.MySelf || target == TargetEnum.All)
            {
                var maxHealth = _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType);
                if (amount > 0)
                    maxHealth.Reduce(amount);
                else
                    maxHealth.Add(-1 * amount);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                var maxHealth = _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType);
                if (amount > 0)
                    maxHealth.Reduce(amount);
                else
                    maxHealth.Add(-1 * amount);
            }
        }
    }
}