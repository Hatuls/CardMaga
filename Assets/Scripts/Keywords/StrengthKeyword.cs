using CardMaga.Battle;

namespace CardMaga.Keywords
{
    public class StunKeyword : BaseKeywordLogic
    {
        public StunKeyword(KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
        }

 

        public override void ProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {
            if (target == TargetEnum.MySelf || target == TargetEnum.All)
            {
                _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Add(amount);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType).Add(amount);
            }
        }

   
        public override void UnProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {
            if (target == TargetEnum.MySelf || target == TargetEnum.All)
            {
                _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Reduce(amount);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType).Reduce(amount);
            }
        }
    }
    public class StrengthKeyword : BaseKeywordLogic
    {
        public StrengthKeyword(KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
        }



        public override void ProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {

            if (target == TargetEnum.MySelf || target == TargetEnum.All)
            {
                _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Add(amount);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType).Add(amount);
            }
            KeywordSO.SoundEventSO.PlaySound();
        }

 
        public override void UnProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {
            if (target == TargetEnum.MySelf || target == TargetEnum.All)
            {
                _playersManager.GetCharacter(currentPlayer).StatsHandler.GetStat(KeywordType).Reduce(amount);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                _playersManager.GetCharacter(!currentPlayer).StatsHandler.GetStat(KeywordType).Reduce(amount);
            }
        }
    }
}