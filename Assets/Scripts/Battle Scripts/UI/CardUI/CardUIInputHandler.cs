using System;
using Battles.UI;
using UnityEngine;

public class CardUIInputHandler : TouchableItem
{
    private void Awake()
    {
        base.OnClick += OnClickFun;//need fix
        base.OnHold += OnHoldFun;
        base.OnBeginHold += OnBeginHoldFun;
        base.OnEndHold += OnEndHoldFun;
        base.OnPointUp += OnPointUpFun;
        base.OnPointDown += OnPointDownFun;

        ForceChangeState(false);
    }

    public event Action OnClick;
    public event Action OnBeginHold;
    public event Action OnEndHold;
    public event Action OnHold;
    public event Action OnPointDown;
    public event Action OnPointUp;

    private void OnClickFun()
    {
        Debug.Log(gameObject.name + " Click");
        OnClick?.Invoke();
    }

    private void OnBeginHoldFun()
    {
        Debug.Log(gameObject.name + " BeginHold");
        OnBeginHold?.Invoke();
    }

    private void OnEndHoldFun()
    {
        Debug.Log(gameObject.name + " EndHold");
        OnEndHold?.Invoke();
    }

    private void OnHoldFun()
    {
        Debug.Log(gameObject.name + " Hold");
        OnHold?.Invoke();
    }

    private void OnPointDownFun()
    {
        Debug.Log(gameObject.name + " PointDown");
        OnPointDown?.Invoke();
    }

    private void OnPointUpFun()
    {
        Debug.Log(gameObject.name + " PointUp");
        OnPointUp?.Invoke();
    }
}