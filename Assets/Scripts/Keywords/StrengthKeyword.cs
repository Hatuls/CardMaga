using CardMaga.Battle;

namespace CardMaga.Keywords
{
    public class StunKeyword : BaseKeywordLogic
    {
        public StunKeyword(KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
        }

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            if (data.GetTarget == TargetEnum.MySelf || data.GetTarget == TargetEnum.All)
            {
                _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Add(data.GetAmountToApply);
            }

            if (data.GetTarget == TargetEnum.Opponent || data.GetTarget == TargetEnum.All)
            {
                _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType).Add(data.GetAmountToApply);
            }
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            if (data.GetTarget == TargetEnum.MySelf || data.GetTarget == TargetEnum.All)
            {
                _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Reduce(data.GetAmountToApply);
            }

            if (data.GetTarget == TargetEnum.Opponent || data.GetTarget == TargetEnum.All)
            {
                _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType).Reduce(data.GetAmountToApply);
            }
        }
    }
    public class StrengthKeyword : BaseKeywordLogic
    {
        public StrengthKeyword(KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
        }

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data )
        {
            //UnityEngine.Debug.Log("<Color=red><a>Keyword Activated:</a></color> " + data.GetTarget.ToString() + " recieved " + data.KeywordSO.GetKeywordType.ToString() + " with Amount " + data.GetAmountToApply);


            if (data.GetTarget == TargetEnum.MySelf || data.GetTarget == TargetEnum.All)
            {
                _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Add(data.GetAmountToApply);
            }

            if (data.GetTarget == TargetEnum.Opponent || data.GetTarget == TargetEnum.All)
            {
                _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType).Add(data.GetAmountToApply);
            }
            data.KeywordSO.SoundEventSO.PlaySound();
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data )
        {
            if (data.GetTarget == TargetEnum.MySelf || data.GetTarget == TargetEnum.All)
            {
                _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Reduce(data.GetAmountToApply);
            }

            if (data.GetTarget == TargetEnum.Opponent || data.GetTarget == TargetEnum.All)
            {
                _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType).Reduce(data.GetAmountToApply);
            }
        }
    }
}