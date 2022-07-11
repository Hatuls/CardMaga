using System.Collections;
using System.Collections.Generic;
using Battles.UI;
using UnityEngine;

public class MoveFromDefultStateToSelectState : BaseCondition
{
    [SerializeField] private HandUI _handUI;

    private bool _toChangeState = false;
    
    public override bool CheckCondition()
    {
        return _toChangeState;
    }

    public override void InitCondition()
    {
        _toChangeState = false;
        _handUI.OnCardSelect += ChangeState;
    }

    private void ChangeState()
    {
        _handUI.OnCardSelect -= ChangeState;
        _toChangeState = true;
    }
}
