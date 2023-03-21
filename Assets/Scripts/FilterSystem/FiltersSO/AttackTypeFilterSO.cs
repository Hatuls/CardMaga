using CardMaga.Card;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackTypeFilterSO", menuName = "ScriptableObjects/Filter/BattleCardData/AttackTypeFilterSO")]
public class AttackTypeFilterSO : CardDataFilter
{
    public override bool Filter(BattleCardData obj)
    {
        if (obj.CardSO.CardTypeData.CardType == CardTypeEnum.Attack)
        {
            return true;
        }

        return false;
    }
}
