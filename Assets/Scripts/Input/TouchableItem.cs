using UnityEngine;
using UnityEngine.EventSystems;


public abstract class TouchableItem : MonoBehaviour , IPointerClickHandler
{
    [HideInInspector] public bool _isTouchable = false;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isTouchable)
        {
            ProcessTouch(eventData);
        }
    }

    protected abstract void ProcessTouch(PointerEventData eventData);
}
