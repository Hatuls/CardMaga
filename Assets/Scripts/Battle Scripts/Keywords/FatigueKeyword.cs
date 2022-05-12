using System.Collections;
using System.Collections.Generic;
using Characters.Stats;
using Keywords;
using UnityEngine;

public class FatigueKeyword : KeywordAbst
{
    
    public override void ProcessOnTarget(bool currentPlayer, KeywordData data)
    {
        if (data.GetTarget == TargetEnum.MySelf || data.GetTarget == TargetEnum.All)
        {
            StaminaHandler.Instance.GetCharacterStamina(currentPlayer).AddStaminaAddition(data.GetAmountToApply);
        }

        if (data.GetTarget == TargetEnum.Opponent || data.GetTarget == TargetEnum.All)
        {
            StaminaHandler.Instance.GetCharacterStamina(!currentPlayer).AddStaminaAddition(data.GetAmountToApply);
        }
    }

    public override KeywordTypeEnum Keyword
    {
        get { return KeywordTypeEnum.Fatigue; }
    }
}
