using Battle;

namespace Keywords
{
    public class StunKeyword : BaseKeywordLogic
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Stun;

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
            if (data.GetTarget == TargetEnum.MySelf || data.GetTarget == TargetEnum.All)
            {
                playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(Keyword).Add(data.GetAmountToApply);
            }

            if (data.GetTarget == TargetEnum.Opponent || data.GetTarget == TargetEnum.All)
            {

                playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(Keyword).Add(data.GetAmountToApply);
            }
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
            if (data.GetTarget == TargetEnum.MySelf || data.GetTarget == TargetEnum.All)
            {
                playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(Keyword).Reduce(data.GetAmountToApply);
            }

            if (data.GetTarget == TargetEnum.Opponent || data.GetTarget == TargetEnum.All)
            {

                playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(Keyword).Reduce(data.GetAmountToApply);
            }
        }
    }
    public class StrengthKeyword : BaseKeywordLogic
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Strength;


        public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
            UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + data.GetTarget.ToString() + " recieved " + data.KeywordSO.GetKeywordType.ToString() + " with Amount " + data.GetAmountToApply);


            if (data.GetTarget == TargetEnum.MySelf || data.GetTarget == TargetEnum.All)
            {
                playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(Keyword).Add(data.GetAmountToApply);
            }

            if (data.GetTarget == TargetEnum.Opponent || data.GetTarget == TargetEnum.All)
            {

                playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(Keyword).Add(data.GetAmountToApply);
            }
            data.KeywordSO.SoundEventSO.PlaySound();
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
            if (data.GetTarget == TargetEnum.MySelf || data.GetTarget == TargetEnum.All)
            {
                playersManager.GetCharacter(currentPlayer).StatsHandler
             .GetStat(Keyword)
             .Reduce(data.GetAmountToApply);
            }

            if (data.GetTarget == TargetEnum.Opponent || data.GetTarget == TargetEnum.All)
            {

                playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(Keyword)
                .Reduce(data.GetAmountToApply);
            }
        }
    }
}