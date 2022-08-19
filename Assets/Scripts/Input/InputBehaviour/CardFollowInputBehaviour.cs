using System;
using System.Collections;
using System.Collections.Generic;
using CardMaga.Input;
using UnityEngine;

public class CardFollowInputBehaviour : InputBehaviour
{
    public event Action OnClick;
    public event Action OnBeginHold;
    public event Action OnEndHold;
    public event Action OnHold;
    public event Action OnPointDown;
    public event Action OnPointUp;

    public void Click()
    {
        throw new System.NotImplementedException();
    }

    public void Hold()
    {
        throw new System.NotImplementedException();
    }

    public void BeginHold()
    {
        throw new System.NotImplementedException();
    }

    public void EndHold()
    {
        throw new System.NotImplementedException();
    }

    public void PointDown()
    {
        throw new System.NotImplementedException();
    }

    public void PointUp()
    {
        throw new System.NotImplementedException();
    }
}
