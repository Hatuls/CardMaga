
using System;
using UnityEngine;

public class InputReciever : MonoBehaviour
{
    #region Fields

    public static Touch? PlayerTouch { get; set; }
    public static event Action<Vector2> OnTouchDetectd;
    public static event Action<Vector2> OnTouchStart;

    Vector2 _touchPosOnScreen;
    Vector2 _firstTouchLocation;

    #endregion
    

    
    #region Monobehaiviour CallBacks

    private void Update()
    {
        TouchDetector(); 
        MouseDetector();
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
            OnTouchStart?.Invoke(_touchPosOnScreen);
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

            _touchPosOnScreen = PlayerTouch.Value.position;
            OnTouchDetectd?.Invoke(_touchPosOnScreen);
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