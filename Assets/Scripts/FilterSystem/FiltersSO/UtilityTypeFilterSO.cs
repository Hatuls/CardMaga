using CardMaga.Card;
using UnityEngine;

[CreateAssetMenu(fileName = "UtilityTypeFilterSO", menuName = "ScriptableObjects/Filter/CardData/UtilityTypeFilterSO")]
public class UtilityTypeFilterSO : CardDataFilter
{
    public override bool Filter(CardData obj)
    {
        if (obj.CardSO.CardType.CardType == CardTypeEnum.Utility)
        {
            return true;
        }

        return false;
    }
}
