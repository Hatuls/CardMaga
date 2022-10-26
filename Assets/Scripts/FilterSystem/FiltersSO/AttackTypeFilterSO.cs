using CardMaga.Card;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackTypeFilterSO", menuName = "ScriptableObjects/Filter/CardData/AttackTypeFilterSO")]
public class AttackTypeFilterSO : CardDataFilter
{
    public override bool Filter(CardData obj)
    {
        if (obj.CardSO.CardType.CardType == CardTypeEnum.Attack)
        {
            return true;
        }

        return false;
    }
}
