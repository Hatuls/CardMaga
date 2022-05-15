using UnityEngine;
using UnityEngine.EventSystems;

public class ItemTest : TouchableItem
{
    protected override void ProcessTouch(PointerEventData eventData)
    {
        Debug.Log("Hey");
    }
}
