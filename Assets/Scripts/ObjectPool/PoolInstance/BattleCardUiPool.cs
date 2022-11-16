using CardMaga.Card;
using CardMaga.UI.Card;
using UnityEngine;

public class BattleCardUiPool : BasePoolObjectVisualToData<BattleCardUI,BattleCardData>
{
    public BattleCardUiPool(BattleCardUI objectPrefab, RectTransform parent) : base(objectPrefab, parent)
    {
    }
}
