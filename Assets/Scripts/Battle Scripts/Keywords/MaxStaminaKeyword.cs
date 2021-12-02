using Characters.Stats;

namespace Keywords
{
    public class MaxStaminaKeyword : KeywordAbst
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.MaxStamina;

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {
            var target = data.GetTarget;
            if (target == TargetEnum.All || target == TargetEnum.MySelf)
                StaminaHandler.Instance.AddStartStamina(currentPlayer, data.GetAmountToApply);

            if (target == TargetEnum.Opponent || target == TargetEnum.All)
                StaminaHandler.Instance.AddStartStamina(!currentPlayer, data.GetAmountToApply);
            data.KeywordSO.SoundEventSO.PlaySound();
        }
    }
}