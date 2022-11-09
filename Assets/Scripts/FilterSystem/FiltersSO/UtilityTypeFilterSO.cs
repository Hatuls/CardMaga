using CardMaga.Card;
using UnityEngine;

[CreateAssetMenu(fileName = "UtilityTypeFilterSO", menuName = "ScriptableObjects/Filter/BattleCardData/UtilityTypeFilterSO")]
public class UtilityTypeFilterSO : CardDataFilter
{
    public override bool Filter(BattleCardData obj)
    {
        if (obj.CardSO.CardType.CardType == CardTypeEnum.Utility)
        {
            return true;
        }

        return false;
    }
}
