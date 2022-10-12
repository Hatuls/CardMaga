using System.Collections.Generic;
using CardMaga.Input;
using CardMaga.UI.Card;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class InputBehaviourHandler<T> : MonoBehaviour  where T : MonoBehaviour
{
    private BaseHandUIState _currentState;
    private InputBehaviourState _currentStateID;
    
    protected Dictionary<InputBehaviourState, BaseHandUIState> _handUIStates;

    public InputBehaviourState CurrentStateID
    {
        get => _currentStateID;
    }
    
    protected void SetState(InputBehaviourState state,CardUI cardUI)
    {
        if (_currentState == null)
        {
            _currentState = _handUIStates[state];
            
            _currentStateID = state;
            
            cardUI.Inputs.ChangeState(_currentStateID);
            
            _currentState.EnterState(cardUI);
        }
        else
        {
            _currentState.ExitState(cardUI);
            
            _currentState = _handUIStates[state];
            
            _currentStateID = state;
            
            cardUI.Inputs.ChangeState(_currentStateID);

            if (_currentState == null)
            {
                cardUI.Inputs.ForceResetInputBehaviour();
                return;
            }
            
            _currentState.EnterState(cardUI);
        }
    }

    protected void LockAllTouchableItems(TouchableItem<T>[] touchableItems,bool isTouchable)
    {
        for (int i = 0; i < touchableItems.Length; i++)
        {
            if (touchableItems[i] == null)
                continue;
            
            touchableItems[i].ChangeState(isTouchable);
        }
    }
    
    protected void LockAllTouchableItemsExcept(TouchableItem<T>[] touchableItems, TouchableItem<T> touchableItem,bool isTouchable)
    {
        TouchableItem<T>[] tempArray = new TouchableItem<T>[] { touchableItem };
        
        LockAllTouchableItemsExcept(touchableItems,tempArray,isTouchable);
    }
    
    protected void LockAllTouchableItemsExcept(TouchableItem<T>[] touchableItems, TouchableItem<T>[] exceptTouchableItem,bool isTouchable)
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
            
            touchableItems[i].ChangeState(isTouchable);
        }
    }

    protected void SetAllTouchableItemsToDefault(TouchableItem<T>[] touchableItems)
    {
        for (int i = 0; i < touchableItems.Length; i++)
        {
            if (touchableItems[i] == null)
                continue;
            
            touchableItems[i].ForceResetInputBehaviour();
        }
    }
}

public enum InputBehaviourState
{
    Default,
    Hand,
    HandFollow,
    HandZoom,
};

