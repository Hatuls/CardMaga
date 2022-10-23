using Battle;

namespace Keywords
{
    public class MaxHealthKeyword : BaseKeywordLogic
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.MaxHealth;

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {

            var target = data.GetTarget;
            if (target == TargetEnum.MySelf || target == TargetEnum.All)
            {
                var maxHealth = playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(Keyword);
                if (data.GetAmountToApply > 0)
                    maxHealth.Add(data.GetAmountToApply);
                else
                    maxHealth.Reduce(-1 * data.GetAmountToApply);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                var maxHealth = playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(Keyword);
                if (data.GetAmountToApply > 0)
                    maxHealth.Add(data.GetAmountToApply);
                else
                    maxHealth.Reduce(-1 * data.GetAmountToApply);
            }
            data.KeywordSO.SoundEventSO.PlaySound();
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
            var target = data.GetTarget;
            if (target == TargetEnum.MySelf || target == TargetEnum.All)
            {
                var maxHealth = playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(Keyword);
                if (data.GetAmountToApply > 0)
                    maxHealth.Reduce(data.GetAmountToApply);
                else
                    maxHealth.Add(-1 * data.GetAmountToApply);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                var maxHealth = playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(Keyword);
                if (data.GetAmountToApply > 0)
                    maxHealth.Reduce(data.GetAmountToApply);
                else
                    maxHealth.Add(-1 * data.GetAmountToApply);
            }
        }
    }
}