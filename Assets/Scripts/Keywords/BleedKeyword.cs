using Battle;

namespace Keywords
{
    public class BleedKeyword : BaseKeywordLogic
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Bleed;


        public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {

            UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + data.GetTarget.ToString() + " recieved " + data.KeywordSO.GetKeywordType.ToString() + " with Amount of " + data.GetAmountToApply);

            var target = data.GetTarget;
            if (target == TargetEnum.MySelf || target == TargetEnum.All)
                playersManager.GetCharacter(currentPlayer).StatsHandler.GetStats(Keyword).Add(data.GetAmountToApply);

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
                playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStats(Keyword).Add(data.GetAmountToApply);

            data.KeywordSO.SoundEventSO.PlaySound();
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
            var target = data.GetTarget;
            if (target == TargetEnum.MySelf || target == TargetEnum.All)
                playersManager.GetCharacter(currentPlayer).StatsHandler.GetStats(Keyword).Reduce(data.GetAmountToApply);

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
                playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStats(Keyword).Reduce(data.GetAmountToApply);

        }
    }
}