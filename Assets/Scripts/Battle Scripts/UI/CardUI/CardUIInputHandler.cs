using System;
using Battles.UI;
using UnityEngine;

public class CardUIInputHandler : TouchableItem
{
    [SerializeField] private CardUI _cardUIRef;
    private void Awake()
    {
        base.OnClick += Click;//need fix
        base.OnHold += Hold;
        base.OnBeginHold += BeginHold;
        base.OnEndHold += EndHold;
        base.OnPointUp += PointUp;
        base.OnPointDown += PointDown;

        ForceChangeState(false);
    }

    public event Action<CardUI> OnClick;
    public event Action<CardUI> OnBeginHold;
    public event Action<CardUI> OnEndHold;
    public event Action<CardUI> OnHold;
    public event Action<CardUI> OnPointDown;
    public event Action<CardUI> OnPointUp;

    private void Click()
    {
        //Debug.Log(gameObject.name + " Click");
        OnClick?.Invoke(_cardUIRef);
    }

    private void BeginHold()
    {
        //Debug.Log(gameObject.name + " BeginHold");
        OnBeginHold?.Invoke(_cardUIRef);
    }

    private void EndHold()
    {
        //Debug.Log(gameObject.name + " EndHold");
        OnEndHold?.Invoke(_cardUIRef);
    }

    private void Hold()
    {
        //Debug.Log(gameObject.name + " Hold");
        OnHold?.Invoke(_cardUIRef);
    }

    private void PointDown()
    {
        //Debug.Log(gameObject.name + " PointDown");
        OnPointDown?.Invoke(_cardUIRef);
    }

    private void PointUp()
    {
        //Debug.Log(gameObject.name + " PointUp");
        OnPointUp?.Invoke(_cardUIRef);
    }
}