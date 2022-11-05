using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : BaseNotificationUIElement
{
    protected override void Dirty()
    {
        Debug.Log(name + "SetDirty");
    }

    protected override void Clean()
    {
        Debug.Log(name + "SetClean");
    }
}
