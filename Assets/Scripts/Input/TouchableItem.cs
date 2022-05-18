using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;


public abstract class TouchableItem : MonoBehaviour , IPointerDownHandler , IPointerUpHandler
{
    [SerializeField] private float _holdDelaySce = .5f;
    
    [HideInInspector] public bool _isTouchable = false;
    private bool _isHold = false;
    
    public event Action<PointerEventData> OnClick;
    public event Action<PointerEventData> OnBeginHold;
    public event Action<PointerEventData> OnEndHold;
    public event Action<PointerEventData> OnHold;
    public event Action<PointerEventData> OnPointDown;
    public event Action<PointerEventData> OnPoinrUp;


    
    private IEnumerator HoldDelay(PointerEventData eventData)
    {
        yield return new WaitForSeconds(_holdDelaySce);
        _isHold = true; 
        StartCoroutine(ProcessHoldTouchCoroutine(eventData));
    }

    private IEnumerator ProcessHoldTouchCoroutine(PointerEventData eventData)
    {
        yield return null; 
        OnBeginHold?.Invoke(eventData);
        
        while (_isHold)
        {
            yield return null;
            OnHold?.Invoke(eventData);
        }
       
    }
    
    
    public void OnPointerDown(PointerEventData eventData)
    { 
        StartCoroutine(HoldDelay(eventData));
        OnPointDown?.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isHold)
        {
            EndHold(eventData);
            return;
        }
        ProcessTouch(eventData);
    }

    private void EndHold(PointerEventData eventData)
    {
        _isHold = false;
        StopAllCoroutines();
        OnEndHold?.Invoke(eventData);
        OnPoinrUp?.Invoke(eventData);
    }

    private void ProcessTouch(PointerEventData eventData)
    {
        StopAllCoroutines();
        OnClick?.Invoke(eventData);
        OnPoinrUp?.Invoke(eventData);
    }
}
