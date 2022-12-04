using CardMaga.Battle;

namespace CardMaga.Keywords
{

    public class FatigueKeyword : BaseKeywordLogic
    {
        public FatigueKeyword(KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
        }


        public override void ProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {
            if (target == TargetEnum.MySelf || target == TargetEnum.All)
            {
                _playersManager.GetCharacter(currentPlayer).StaminaHandler.AddStaminaAddition(-amount);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                _playersManager.GetCharacter(currentPlayer).StaminaHandler.AddStaminaAddition(-amount);
            }
        }

       

        public override void UnProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {
            if (target == TargetEnum.MySelf || target == TargetEnum.All)
            {
                _playersManager.GetCharacter(currentPlayer).StaminaHandler.AddStaminaAddition(amount);
            }

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
            {
                _playersManager.GetCharacter(currentPlayer).StaminaHandler.AddStaminaAddition(amount);
            }
        }
    }

}