using Battle;
using Characters.Stats;

namespace Keywords
{
    public class MaxStaminaKeyword : BaseKeywordLogic
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.MaxStamina;

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
            var target = data.GetTarget;
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                playersManager.GetCharacter(currentPlayer).StaminaHandler.AddStaminaAddition(data.GetAmountToApply);

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
                playersManager.GetCharacter(!currentPlayer).StaminaHandler.AddStaminaAddition(data.GetAmountToApply);
            data.KeywordSO.SoundEventSO.PlaySound();
        }

        public override void UnProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
        {
            var target = data.GetTarget;
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                playersManager.GetCharacter(currentPlayer).StaminaHandler.AddStaminaAddition(-data.GetAmountToApply);

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
                playersManager.GetCharacter(!currentPlayer).StaminaHandler.AddStaminaAddition(-data.GetAmountToApply);
        }
    }
}