using System;
using Battles.UI;
using UnityEngine;

public class CardUIInputHandler : MonoBehaviour
{
    public enum State
    {
        Lock,
        UnLock
    }

    [SerializeField] private TouchableItem _touchableItem;
    [SerializeField] private CardUI _cardUI;

    public State CurrentState { get; private set; }

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

    public static event Action<CardUI> OnClick;
    public static event Action<CardUI> OnBeginHold;
    public static event Action<CardUI> OnEndHold;
    public static event Action<CardUI> OnHold;
    public static event Action<CardUI> OnPointDown;
    public static event Action<CardUI> OnPointUp;

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
        CurrentState = state;

        switch (CurrentState)
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
        if (CurrentState == State.Lock)
            ChangeState(State.UnLock);

        else if (CurrentState == State.UnLock) ChangeState(State.Lock);
    }

    public void ForceChangeState(bool isTouchable)
    {
        if (isTouchable)
            ChangeState(State.UnLock);
        else
            ChangeState(State.Lock);
    }
}