using System;
using System.Collections;
using System.Collections.Generic;
using Battles.UI;
using UnityEngine;

public class CardUIInputHandler : MonoBehaviour
{
    [SerializeField] private TouchableItem _touchableItem;
    [SerializeField] private CardUI _cardUI;

    private State _currentState;

    public State CurrentState
    {
        get { return _currentState; }
    }

    public enum State
    {
        Lock,
        UnLock
    };

    public static event Action<CardUI> OnClick;
    public static event Action<CardUI> OnBeginHold;
    public static event Action<CardUI> OnEndHold;
    public static event Action<CardUI> OnHold;
    public static event Action<CardUI> OnPointDown;
    public static event Action<CardUI> OnPointUp;

    private void Awake()
    {
        _touchableItem.OnClick += OnClickFun;
        _touchableItem.OnHold += OnHoldFun;
        _touchableItem.OnBeginHold += OnBeginHoldFun;
        _touchableItem.OnEndHold += OnEndHoldFun;
        _touchableItem.OnPointUp += OnPointUpFun;
        _touchableItem.OnPointDown += OnPointDownFun;
        
        ChangeState(State.Lock);
    }

    private void OnClickFun()
    {
        Debug.Log(gameObject.name + " Click");
        OnClick?.Invoke(_cardUI);
    }
    
    private void OnBeginHoldFun()
    {
        Debug.Log(gameObject.name + " BeginHold");
        OnBeginHold?.Invoke(_cardUI);
    }
    
    private void OnEndHoldFun()
    {
        Debug.Log(gameObject.name + " EndHold");
        OnEndHold?.Invoke(_cardUI);
    }
    
    private void OnHoldFun()
    {
        Debug.Log(gameObject.name + " Hold");
        OnHold?.Invoke(_cardUI);
    }
    
    private void OnPointDownFun()
    {
        Debug.Log(gameObject.name + " PointDown");
        OnPointDown?.Invoke(_cardUI);
    }
    
    private void OnPointUpFun()
    {
        Debug.Log(gameObject.name + " PointUp");
        OnPointUp?.Invoke(_cardUI);
    }

    private void ChangeState(State state)
    {
        _currentState = state;
        
        switch (_currentState)
        {
            case State.Lock:
                _touchableItem.IsTouchable = false;
                break;
            case State.UnLock:
                _touchableItem.IsTouchable = true;
                break;
            default:
                Debug.LogError(name + " State Not Set");
                break;
        }

        Debug.Log(name + " is touchable set to " + _touchableItem.IsTouchable);
    }
    
    [ContextMenu("ToggleState")]
    public void ToggleState()
    {
        if (_currentState == State.Lock)
        {
            ChangeState(State.UnLock);
        }

        else if (_currentState == State.UnLock)
        {
            ChangeState(State.Lock);
        }
    }

    public void ForceChangeState(bool isTouchable)
    {
        if (isTouchable)
        {
            ChangeState(State.UnLock);
        }
        else
        {
            ChangeState(State.Lock);
        }
    }
}
