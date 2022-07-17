﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Sirenix.OdinInspector;


public class TouchableItem : MonoBehaviour , IPointerDownHandler , IPointerUpHandler
{
    [SerializeField] private float _holdDelaySce = .5f;
    
    private bool _isTouchable = false;
    private bool _isHold = false;
    
    public enum State
    {
        Lock,
        UnLock
    }
    [ShowInInspector,Sirenix.OdinInspector.ReadOnly]
    public State CurrentState { get; private set; }
    
    protected event Action OnClick;
    protected event Action OnBeginHold;
    protected event Action OnEndHold;
    protected event Action OnHold;
    protected event Action OnPointDown;
    protected event Action OnPointUp;
    
    
    private IEnumerator HoldDelay(PointerEventData eventData)
    {
        yield return new WaitForSeconds(_holdDelaySce);
        _isHold = true; 
        StartCoroutine(ProcessHoldTouchCoroutine(eventData));
    }

    private IEnumerator ProcessHoldTouchCoroutine(PointerEventData eventData)
    {
        yield return null; 
        OnBeginHold?.Invoke();
        
        while (_isHold)
        {
            yield return null;
            OnHold?.Invoke();
        }
       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isTouchable)
        {
            StartCoroutine(HoldDelay(eventData));
            OnPointDown?.Invoke();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isTouchable)
        {
            if (_isHold)
            {
                EndHold(eventData);
                return;
            }
            ProcessTouch(eventData);
        }
    }

    private void EndHold(PointerEventData eventData)
    {
        _isHold = false;
        StopAllCoroutines();
        OnEndHold?.Invoke();
        OnPointUp?.Invoke();
    }

    private void ProcessTouch(PointerEventData eventData)
    {
        StopAllCoroutines();
        OnClick?.Invoke();
        OnPointUp?.Invoke();
    }
    
    private void ChangeState(State state)
    {
        CurrentState = state;

        switch (CurrentState)
        {
            case State.Lock:
                _isTouchable = false;
                break;
            case State.UnLock:
                _isTouchable = true;
                break;
            default:
                Debug.LogError(name + " State Not Set");
                break;
        }

        Debug.Log(name + " is touchable set to " + _isTouchable);
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
