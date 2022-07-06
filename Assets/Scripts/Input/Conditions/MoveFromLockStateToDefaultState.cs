using System.Collections;
using System.Collections.Generic;
using Battles.UI;
using UnityEngine;

public class MoveFromLockStateToDefaultState : BaseCondition
{
    [SerializeField] private HandUI _handUI;

    private bool _toMove = false;
    
    public override bool CheckCondition()
    {
        return _toMove;
    }

    public override void InitCondition()
    {
        _handUI.OnCardDrawnAndAlign += SetBool;
    }

    private void SetBool()
    {
        _toMove = true;
    }
}
