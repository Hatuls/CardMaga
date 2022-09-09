using Battle;
using Characters.Stats;

namespace Keywords
{
    public class StunKeyword : KeywordAbst
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Stun;

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
            if (data.GetTarget == TargetEnum.MySelf || data.GetTarget == TargetEnum.All)
            {
                playersManager.GetCharacter(currentPlayer).StatsHandler.GetStats(Keyword).Add(data.GetAmountToApply);
            }

            if (data.GetTarget == TargetEnum.Opponent || data.GetTarget == TargetEnum.All)
            {

                playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStats(Keyword).Add(data.GetAmountToApply);
            }
        }
    }
    public class StrengthKeyword : KeywordAbst
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Strength;


        public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
            UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + data.GetTarget.ToString() + " recieved " + data.KeywordSO.GetKeywordType.ToString() + " with Amount " + data.GetAmountToApply);

       
            if (data.GetTarget == TargetEnum.MySelf || data.GetTarget == TargetEnum.All)
            {
                playersManager.GetCharacter(currentPlayer).StatsHandler
             .GetStats(Keyword)
             .Add(data.GetAmountToApply);
            }

            if (data.GetTarget == TargetEnum.Opponent || data.GetTarget == TargetEnum.All)
            {

                playersManager.GetCharacter(!currentPlayer).StatsHandler
  .GetStats(Keyword)
                .Add(data.GetAmountToApply);
            }
            data.KeywordSO.SoundEventSO.PlaySound();
        }
    }
}