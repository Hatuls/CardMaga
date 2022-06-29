using System;
using Battles.UI;
using UnityEngine;

public class CardUIInputHandler : TouchableItem
{
    private CardUI _cardUIRef;
    private void Awake()
    {
        base.OnClick += OnClickFun;//need fix
        base.OnHold += OnHoldFun;
        base.OnBeginHold += OnBeginHoldFun;
        base.OnEndHold += OnEndHoldFun;
        base.OnPointUp += OnPointUpFun;
        base.OnPointDown += OnPointDownFun;

        _cardUIRef = GetComponent<CardUI>();
        
        ForceChangeState(false);
    }

    public event Action<CardUI> OnClick;
    public event Action<CardUI> OnBeginHold;
    public event Action<CardUI> OnEndHold;
    public event Action<CardUI> OnHold;
    public event Action<CardUI> OnPointDown;
    public event Action<CardUI> OnPointUp;

    private void OnClickFun()
    {
        Debug.Log(gameObject.name + " Click");
        OnClick?.Invoke(_cardUIRef);
    }

    private void OnBeginHoldFun()
    {
        Debug.Log(gameObject.name + " BeginHold");
        OnBeginHold?.Invoke(_cardUIRef);
    }

    private void OnEndHoldFun()
    {
        Debug.Log(gameObject.name + " EndHold");
        OnEndHold?.Invoke(_cardUIRef);
    }

    private void OnHoldFun()
    {
        Debug.Log(gameObject.name + " Hold");
        OnHold?.Invoke(_cardUIRef);
    }

    private void OnPointDownFun()
    {
        Debug.Log(gameObject.name + " PointDown");
        OnPointDown?.Invoke(_cardUIRef);
    }

    private void OnPointUpFun()
    {
        Debug.Log(gameObject.name + " PointUp");
        OnPointUp?.Invoke(_cardUIRef);
    }
}