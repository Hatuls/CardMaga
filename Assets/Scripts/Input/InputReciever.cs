
using System;
using UnityEngine;

public class InputReciever : MonoBehaviour
{
    #region Fields

    public static Touch? PlayerTouch { get; set; }
    public static event Action<Vector2> OnTouchDetected;
    public static event Action<Vector2> OnTouchEnded;
    public static event Action<Vector2> OnTouchStart;

    Vector2 _touchPosOnScreen;
    Vector2 _firstTouchLocation;

    #endregion
    

    
    #region Monobehaiviour CallBacks

    private void Update()
    {
        TouchDetector(); 
        //MouseDetector();
    }

    private void MouseDetector()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _firstTouchLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            OnTouchStart?.Invoke(_firstTouchLocation);
            
        }
        if (Input.GetMouseButton(0))
        {
            _touchPosOnScreen = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            OnTouchDetected?.Invoke(_touchPosOnScreen);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _touchPosOnScreen = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            OnTouchEnded?.Invoke(_touchPosOnScreen);
        }
    }
    
    #endregion


    #region Private Functions

    private void TouchDetector()
    {
        if (Input.touchCount > 0)
        {
            PlayerTouch = Input.GetTouch(0);

            if (PlayerTouch == null)
                return;
            
            GetTouchPhase(PlayerTouch.Value);
            
            _touchPosOnScreen = PlayerTouch.Value.position;
            OnTouchDetected?.Invoke(_touchPosOnScreen);
        }
    }

    private void GetTouchPhase(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                _touchPosOnScreen = touch.position;
                OnTouchStart?.Invoke(_touchPosOnScreen);
                break;
            case TouchPhase.Moved:
                _touchPosOnScreen = touch.position;
                OnTouchDetected?.Invoke(_touchPosOnScreen);
                break;
            case TouchPhase.Ended:
                _touchPosOnScreen = touch.position;
                OnTouchEnded?.Invoke(_touchPosOnScreen);
                break;
            case TouchPhase.Canceled:
                break;
            case TouchPhase.Stationary:
                _touchPosOnScreen = touch.position;
                OnTouchDetected?.Invoke(_touchPosOnScreen);
                break;
        }
    }
    
    #endregion

    #region Gizmos

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_touchPosOnScreen, 100 * Vector3.forward);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(_firstTouchLocation, 100 * Vector3.forward);
    }

    #endregion
}