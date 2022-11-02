using CardMaga.Card;
using UnityEngine;

[CreateAssetMenu(fileName = "DefendTypeFilterSO", menuName = "ScriptableObjects/Filter/CardData/DefendTypeFilterSO")]
public class DefendTypeFilterSO : CardDataFilter
{
    public override bool Filter(CardData obj)
    {
        if (obj.CardSO.CardType.CardType == CardTypeEnum.Defend)
        {
            return true;
        }

        return false;
    }
}
