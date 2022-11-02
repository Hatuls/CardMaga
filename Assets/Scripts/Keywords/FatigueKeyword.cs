using CardMaga.Battle;

namespace CardMaga.Keywords
{

    public class FatigueKeyword : BaseKeywordLogic
    {
        public FatigueKeyword(KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
        }

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            if (data.GetTarget == TargetEnum.MySelf || data.GetTarget == TargetEnum.All)
            {
                _playersManager.GetCharacter(currentPlayer).StaminaHandler.AddStaminaAddition(-data.GetAmountToApply);
            }

            if (data.GetTarget == TargetEnum.Opponent || data.GetTarget == TargetEnum.All)
            {
                _playersManager.GetCharacter(currentPlayer).StaminaHandler.AddStaminaAddition(-data.GetAmountToApply);
            }
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            if (data.GetTarget == TargetEnum.MySelf || data.GetTarget == TargetEnum.All)
            {
                _playersManager.GetCharacter(currentPlayer).StaminaHandler.AddStaminaAddition(data.GetAmountToApply);
            }

            if (data.GetTarget == TargetEnum.Opponent || data.GetTarget == TargetEnum.All)
            {
                _playersManager.GetCharacter(currentPlayer).StaminaHandler.AddStaminaAddition(data.GetAmountToApply);
            }
        }

    }

}