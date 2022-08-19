using System;
using System.Collections;
using System.Collections.Generic;
using CardMaga.Input;
using UnityEngine;

public class CardDefaultInputBehaviour : InputBehaviour
{
    public override void Click()
    {
        base.Click();
    }

    public override void Hold()
    {
        base.Hold();
        throw new System.NotImplementedException();
    }

    public override void BeginHold()
    {
        base.BeginHold();
        throw new System.NotImplementedException();
    }

    public override void EndHold()
    {
        base.EndHold();
        throw new System.NotImplementedException();
    }

    public override void PointDown()
    {
        base.PointDown();
        throw new System.NotImplementedException();
    }

    public override void PointUp()
    {
        base.PointUp();
        throw new System.NotImplementedException();
    }
}
