using System;
using UnityEngine;

public class InputReciever : MonoBehaviour
{
    #region Fields

    public static Touch? PlayerTouch { get; set; }
    public static event Action<Vector2> OnTouchDetected;
    public static event Action<Vector2> OnTouchEnded;
    public static event Action<Vector2> OnTouchStart;

    private static Vector2 _touchPosOnScreen;
    private Vector2 _firstTouchLocation;
    private Camera _camera;

    #endregion

    #region Prop

    public static Vector2 TouchPosOnScreen
    {
        get => _touchPosOnScreen;
    }

    #endregion

    #region Monobehaiviour CallBacks

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        TouchDetector();
        //MouseDetector();
    }

    #endregion

    #region Private Functions

    private void MouseDetector()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _firstTouchLocation = _camera.ScreenToWorldPoint(Input.mousePosition);
            OnTouchStart?.Invoke(_firstTouchLocation);
        }

        if (Input.GetMouseButton(0))
        {
            _touchPosOnScreen = _camera.ScreenToWorldPoint(Input.mousePosition);
            OnTouchDetected?.Invoke(_touchPosOnScreen);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _touchPosOnScreen = _camera.ScreenToWorldPoint(Input.mousePosition);
            OnTouchEnded?.Invoke(_touchPosOnScreen);
        }
    }

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
}