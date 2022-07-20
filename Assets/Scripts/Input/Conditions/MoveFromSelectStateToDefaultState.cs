using CardMaga.UI;
using UnityEngine;

namespace CardMaga.Input
{
    public class MoveFromSelectStateToDefaultState : BaseCondition
    {
        [SerializeField] private HandUI _handUI;
        public override bool CheckCondition()
        {
            return !_handUI.IsCardSelect;
        }
    }
}