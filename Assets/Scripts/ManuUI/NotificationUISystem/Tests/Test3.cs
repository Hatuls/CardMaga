using UnityEngine;

public class Test3 : BaseNotificationUIElement
{
    protected override void Dirty()
    {
        Debug.Log(name + "SetDirty");
    }

    protected override void Clean()
    {
        Debug.Log(name + "Setclean");
    }
}
