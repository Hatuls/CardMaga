using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.Serialization;


public class TouchableItem : MonoBehaviour , IPointerDownHandler , IPointerUpHandler
{
    [SerializeField] private float _holdDelaySce = .5f;
    
    [FormerlySerializedAs("_isTouchable")] [HideInInspector] public bool IsTouchable = false;
    private bool _isHold = false;
    
    public event Action OnClick;
    public event Action OnBeginHold;
    public event Action OnEndHold;
    public event Action OnHold;
    public event Action OnPointDown;
    public event Action OnPointUp;


    
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
        if (IsTouchable)
        {
            StartCoroutine(HoldDelay(eventData));
            OnPointDown?.Invoke();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (IsTouchable)
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
}
