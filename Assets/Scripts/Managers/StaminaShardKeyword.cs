using CardMaga.Battle;

namespace CardMaga.Keywords
{
    public class StaminaShardKeyword : BaseKeywordLogic
    {
        public StaminaShardKeyword(KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
        }

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {

            var target = data.GetTarget;

            data.KeywordSO.SoundEventSO.PlaySound();
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Add(data.GetAmountToApply);


            if (target == TargetEnum.All || target == TargetEnum.Opponent)
                _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType).Add(data.GetAmountToApply);
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            var target = data.GetTarget;

            data.KeywordSO.SoundEventSO.PlaySound();
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Reduce(data.GetAmountToApply);


            if (target == TargetEnum.All || target == TargetEnum.Opponent)
                _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType).Reduce(data.GetAmountToApply);
        }
    }
}