using CardMaga.UI;
using UnityEngine;

public class MoveFromDefultStateToSelectState : BaseCondition
{
    [SerializeField] private HandUI _handUI;
    public override bool CheckCondition()
    {
        return _handUI.IsCardSelect;
    }
}