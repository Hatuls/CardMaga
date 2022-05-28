using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUIInputHandler : MonoBehaviour
{
    [SerializeField] private TouchableItem _touchableItem;
    [SerializeField] private CardUINew _cardUINew;

    private State currentState;

    public State CurrentState
    {
        get { return currentState; }
    }

    public enum State
    {
        Lock,
        UnLock
    };

    public static event Action<CardUINew> OnClick;
    public static event Action<CardUINew> OnBeginHold;
    public static event Action<CardUINew> OnEndHold;
    public static event Action<CardUINew> OnHold;
    public static event Action<CardUINew> OnPointDown;
    public static event Action<CardUINew> OnPoinrUp;

    private void Awake()
    {
        _touchableItem.OnClick += OnClickFun;
        _touchableItem.OnHold += OnHoldFun;
        _touchableItem.OnBeginHold += OnBeginHoldFun;
        _touchableItem.OnEndHold += OnEndHoldFun;
        _touchableItem.OnPoinrUp += OnPoinrUpFun;
        _touchableItem.OnPointDown += OnPointDownFun;
        
        currentState = State.Lock;
        ChangeState(currentState);
    }

    private void OnClickFun()
    {
        Debug.Log(gameObject.name + " Click");
        OnClick?.Invoke(_cardUINew);
    }
    
    private void OnBeginHoldFun()
    {
        Debug.Log(gameObject.name + " BeginHold");
        OnBeginHold?.Invoke(_cardUINew);
    }
    
    private void OnEndHoldFun()
    {
        Debug.Log(gameObject.name + " EndHold");
        OnEndHold?.Invoke(_cardUINew);
    }
    
    private void OnHoldFun()
    {
        Debug.Log(gameObject.name + " Hold");
        OnHold?.Invoke(_cardUINew);
    }
    
    private void OnPointDownFun()
    {
        Debug.Log(gameObject.name + " PointDown");
        OnPointDown?.Invoke(_cardUINew);
    }
    
    private void OnPoinrUpFun()
    {
        Debug.Log(gameObject.name + " PointUp");
        OnPoinrUp?.Invoke(_cardUINew);
    }

    private void ChangeState(State state)
    {
        switch (state)
        {
            case State.Lock:
                _touchableItem._isTouchable = false;
                break;
            case State.UnLock:
                _touchableItem._isTouchable = true;
                break;
            default:
                Debug.LogError(name + " State Not Set");
                break;
        }

        Debug.Log(name + " is touchable set to " + _touchableItem._isTouchable);
    }
    
    [ContextMenu("ToggleState")]
    public void ToggleState()
    {
        if (currentState == State.Lock)
        {
            currentState = State.UnLock;
        }

        else if (currentState == State.UnLock)
        {
            currentState = State.Lock;
        }
        
        ChangeState(currentState);
    }

    public void ForceChangeState(bool isTouchable)
    {
        if (isTouchable)
        {
            currentState = State.UnLock;
        }
        else
        {
            currentState = State.Lock;
        }
        
        ChangeState(currentState);
    }
}
