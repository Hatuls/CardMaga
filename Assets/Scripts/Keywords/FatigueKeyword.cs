using Battle;
using Keywords;

public class FatigueKeyword : BaseKeywordLogic
{

    public override void ProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
    {
        if (data.GetTarget == TargetEnum.MySelf || data.GetTarget == TargetEnum.All)
        {
            playersManager.GetCharacter(currentPlayer).StaminaHandler.AddStaminaAddition(-data.GetAmountToApply);
        }

        if (data.GetTarget == TargetEnum.Opponent || data.GetTarget == TargetEnum.All)
        {
            playersManager.GetCharacter(currentPlayer).StaminaHandler.AddStaminaAddition(-data.GetAmountToApply);
        }
    }

    public override void UnProcessOnTarget(bool currentPlayer, KeywordData data, IPlayersManager playersManager)
    {
        if (data.GetTarget == TargetEnum.MySelf || data.GetTarget == TargetEnum.All)
        {
            playersManager.GetCharacter(currentPlayer).StaminaHandler.AddStaminaAddition(data.GetAmountToApply);
        }

        if (data.GetTarget == TargetEnum.Opponent || data.GetTarget == TargetEnum.All)
        {
            playersManager.GetCharacter(currentPlayer).StaminaHandler.AddStaminaAddition(data.GetAmountToApply);
        }
    }

    public override KeywordTypeEnum Keyword
    {
        get { return KeywordTypeEnum.Fatigue; }
    }
}
