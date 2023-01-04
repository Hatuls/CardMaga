using CardMaga.UI.PopUp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadePopUp : BasePopUp
{
    public override void Enter()
    {
        base.Enter();
        _popUpTransitionHandler.StartTransitionFlowFromBeginning();
    }
}
