using CardMaga.Battle;

namespace CardMaga.Keywords
{
    public class MaxHealthKeyword : BaseKeywordLogic
    {
        public MaxHealthKeyword(KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
        }

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {

            var target = data.GetTarget;
            if (target == TargetEnum.MySelf || target == TargetEnum.All)
            {
                var maxHealth = _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType);
                if (data.GetAmountToApply > 0)
                    maxHealth.Add(data.GetAmountToApply);
                else
                    maxHealth.Reduce(-1 * data.GetAmountToApply);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                var maxHealth = _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType);
                if (data.GetAmountToApply > 0)
                    maxHealth.Add(data.GetAmountToApply);
                else
                    maxHealth.Reduce(-1 * data.GetAmountToApply);
            }
            data.KeywordSO.SoundEventSO.PlaySound();
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            var target = data.GetTarget;
            if (target == TargetEnum.MySelf || target == TargetEnum.All)
            {
                var maxHealth = _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType);
                if (data.GetAmountToApply > 0)
                    maxHealth.Reduce(data.GetAmountToApply);
                else
                    maxHealth.Add(-1 * data.GetAmountToApply);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                var maxHealth = _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType);
                if (data.GetAmountToApply > 0)
                    maxHealth.Reduce(data.GetAmountToApply);
                else
                    maxHealth.Add(-1 * data.GetAmountToApply);
            }
        }
    }
}