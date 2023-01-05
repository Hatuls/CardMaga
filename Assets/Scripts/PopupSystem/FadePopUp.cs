using CardMaga.UI.PopUp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FadePopUp : BasePopUp
{
    public override void Enter()
    {
        base.Enter();
        _popUpTransitionHandler.StartTransitionFlowFromBeginning();
    }
}
