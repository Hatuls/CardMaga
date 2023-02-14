using CardMaga.Battle;

namespace CardMaga.Keywords
{
    public class MaxStaminaKeyword : BaseKeywordLogic
    {
        public MaxStaminaKeyword(KeywordSO keywordSO, IPlayersManager playersManager) : base(keywordSO, playersManager)
        {
        }

        public override void ProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                _playersManager.GetCharacter(currentPlayer).StaminaHandler.AddStaminaAddition(amount);

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
                _playersManager.GetCharacter(!currentPlayer).StaminaHandler.AddStaminaAddition(amount);

            InvokeKeywordVisualEffect(currentPlayer, KeywordSO.OnApplyVFX);
            KeywordSO.SoundEventSO.PlaySound();
        }



        public override void UnProcessOnTarget(bool currentPlayer, TargetEnum target, int amount)
        {
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                _playersManager.GetCharacter(currentPlayer).StaminaHandler.AddStaminaAddition(-amount);

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
                _playersManager.GetCharacter(!currentPlayer).StaminaHandler.AddStaminaAddition(-amount);
        }
    }
}