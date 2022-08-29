using UnityEngine;

public class Test3 : NotificationUIElement
{
    protected override void OnDirty()
    {
        Debug.Log(name + "SetDirty");
    }

    protected override void OnClean()
    {
        Debug.Log(name + "Setclean");
    }
}
