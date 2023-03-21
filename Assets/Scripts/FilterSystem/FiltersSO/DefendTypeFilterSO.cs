using CardMaga.Card;
using UnityEngine;

[CreateAssetMenu(fileName = "DefendTypeFilterSO", menuName = "ScriptableObjects/Filter/BattleCardData/DefendTypeFilterSO")]
public class DefendTypeFilterSO : CardDataFilter
{
    public override bool Filter(BattleCardData obj)
    {
        if (obj.CardSO.CardTypeData.CardType == CardTypeEnum.Defend)
        {
            return true;
        }

        return false;
    }
}
