﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : NotificationUIElement
{
    protected override void OnDirty()
    {
        Debug.Log(name + "SetDirty");
    }

    protected override void OnClean()
    {
        Debug.Log(name + "SetClean");
    }
}
