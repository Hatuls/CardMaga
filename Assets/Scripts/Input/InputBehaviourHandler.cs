using CardMaga.Input;
using UnityEngine;

public abstract class InputBehaviourHandler<T> : MonoBehaviour  where T : MonoBehaviour
{
    public abstract InputBehaviour<T>[] InputBehaviours { get; }

    public void LockAllTouchableItems(TouchableItem<T>[] touchableItems)
    {
        for (int i = 0; i < touchableItems.Length; i++)
        {
            if (touchableItems[i] == null)
                continue;
            
            touchableItems[i].ForceChangeState(false);
        }
    }
    
    public void LockAllTouchableItemsExcept(TouchableItem<T>[] touchableItems, TouchableItem<T> touchableItem)
    {
        TouchableItem<T>[] tempArray = new TouchableItem<T>[] { touchableItem };
        
        LockAllTouchableItemsExcept(touchableItems,tempArray);
    }
    
    public void LockAllTouchableItemsExcept(TouchableItem<T>[] touchableItems, TouchableItem<T>[] exceptTouchableItem)
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
            
            touchableItems[i].ForceChangeState(false);
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

    public bool TrySetBehaviour(TouchableItem<T> touchableItem, InputBehaviour<T> inputBehaviour)
    {
        if (touchableItem == null || inputBehaviour == null)
            return false;
        
        touchableItem.TrySetInputBehaviour(inputBehaviour);
        return true;
    }
}
