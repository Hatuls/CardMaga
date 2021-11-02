using Characters.Stats;

namespace Keywords
{
    public class StaminaShardKeyword : KeywordAbst
    {
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.StaminaShards;

        public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
        {

            var target = data.GetTarget;
            


            if (target == TargetEnum.All || target == TargetEnum.MySelf)
            {
                StaminaHandler.Instance.PlayerStamina.AddStaminaShard(data.GetAmountToApply);
            }

            if (target == TargetEnum.All || target == TargetEnum.Opponent)
                StaminaHandler.Instance.OpponentStamina.AddStaminaShard(data.GetAmountToApply);
        }
    }
}