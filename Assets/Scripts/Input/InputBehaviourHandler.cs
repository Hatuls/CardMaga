using CardMaga.Input;
using UnityEngine;

public abstract class InputBehaviourHandler<T> : MonoBehaviour  where T : MonoBehaviour
{
    public void LockAllTouchableItems(TouchableItem<T>[] touchableItems,bool isTouchable)
    {
        for (int i = 0; i < touchableItems.Length; i++)
        {
            if (touchableItems[i] == null)
                continue;
            
            touchableItems[i].ForceChangeState(isTouchable);
        }
    }
    
    public void LockAllTouchableItemsExcept(TouchableItem<T>[] touchableItems, TouchableItem<T> touchableItem,bool isTouchable)
    {
        TouchableItem<T>[] tempArray = new TouchableItem<T>[] { touchableItem };
        
        LockAllTouchableItemsExcept(touchableItems,tempArray,isTouchable);
    }
    
    public void LockAllTouchableItemsExcept(TouchableItem<T>[] touchableItems, TouchableItem<T>[] exceptTouchableItem,bool isTouchable)
    {
        for (int i = 0; i < touchableItems.Length; i++)
        {
            bool isExcept = false;
            
            if (touchableItems[i] == null)
                continue;

            for (int j = 0; j < exceptTouchableItem.Length; j++)
            {
                isExcept = touchableItems[i] == exceptTouchableItem[j];
                break;
            }

            if (isExcept)
                continue;            
            
            touchableItems[i].ForceChangeState(isTouchable);
        }
    }

    public void SetAllTouchableItemsToDefault(TouchableItem<T>[] touchableItems)
    {
        for (int i = 0; i < touchableItems.Length; i++)
        {
            if (touchableItems[i] == null)
                continue;
            
            touchableItems[i].ResetInputBehaviour();
        }
    }
}
