using System;
using UnityEngine;

    public class InputReciever : MonoBehaviour
{
    #region Fields

    public static Touch? PlayerTouch { get; set; }
    public static event Action<Vector2> OnTouchDetected;
    public static event Action<Vector2> OnTouchEnded;
    public static event Action<Vector2> OnTouchStart;
    public static event Action<SwipeData> OnSwipeDetected;

    private const float CIRCLE_BORDER_TOP_LEFT = 0.875F;
    private const float CIRCLE_BORDER_TOP_RIGHT = 0.125F;
    private const float CIRCLE_BORDER_BUTTOM_RIGHT = 0.375F;
    private const float CIRCLE_BORDER_BUTTOM_LEFT = 0.625F;

    [SerializeField] private float _swipeDistance;
    [SerializeField] private bool _onlyDetectSwipeAtEnd;
    
    private static Vector2 _touchPosOnScreen;
    private static Vector2 _startTouchLocation;
    private static Vector2 _endTouchLocation;
    
    private static bool _isTouching = false;
    
    private Camera _camera;

    public enum SwipeDirection
    {
        Up,
        Down,
        Left,
        Right
    };

    #endregion

    #region Prop

    public static bool IsTouching
    {
        get => _isTouching;
    }
    
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
            _startTouchLocation = _camera.ScreenToWorldPoint(Input.mousePosition);
            OnTouchStart?.Invoke(_startTouchLocation);
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
            _isTouching = true;
            
            PlayerTouch = Input.GetTouch(0);

            if (PlayerTouch == null)
                return;

            GetTouchPhase(PlayerTouch.Value);

            _touchPosOnScreen = PlayerTouch.Value.position;
            OnTouchDetected?.Invoke(_touchPosOnScreen);
            
            return;
        }
        
        _isTouching = false;
    }

    private void GetTouchPhase(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                _startTouchLocation = touch.position;
                _touchPosOnScreen = touch.position;
                _endTouchLocation = touch.position;
                OnTouchStart?.Invoke(_startTouchLocation);
                break;
            case TouchPhase.Moved:
                _touchPosOnScreen = touch.position;
                if (!_onlyDetectSwipeAtEnd)
                    SwipeDetector(_startTouchLocation,_touchPosOnScreen);
                OnTouchDetected?.Invoke(_touchPosOnScreen);
                break;
            case TouchPhase.Ended:
                _endTouchLocation = touch.position;
                _touchPosOnScreen = touch.position;
                if (_onlyDetectSwipeAtEnd)
                    SwipeDetector(_startTouchLocation,_endTouchLocation);
                OnTouchEnded?.Invoke(_endTouchLocation);
                break;
            case TouchPhase.Canceled:
                break;
            case TouchPhase.Stationary:
                _touchPosOnScreen = touch.position;
                OnTouchDetected?.Invoke(_touchPosOnScreen);
                break;
        }
    }

    private void SwipeDetector(Vector2 start, Vector2 end)
    {
        if (PlayerTouch == null)
            return;

        if (Vector2.Distance(start,end) > _swipeDistance)
        {
            SwipeData swipeData;

            swipeData.SwipeStartPosition = start;
            swipeData.SwipeEndPosition = end;
            
            swipeData.SwipeDirection = SwipeDirectionCalculation(start,end);
            
            OnSwipeDetected?.Invoke(swipeData);
        }
    }

    private SwipeDirection SwipeDirectionCalculation(Vector2 start, Vector2 end)
    {
        Vector2 dir = (end - start).normalized;

        float dirAngle = Vector2.Angle(Vector2.zero, dir) / 360;

        if (CIRCLE_BORDER_TOP_LEFT < dirAngle && dirAngle < CIRCLE_BORDER_TOP_RIGHT)
            return SwipeDirection.Up;
        if (CIRCLE_BORDER_TOP_RIGHT < dirAngle && dirAngle < CIRCLE_BORDER_BUTTOM_RIGHT)
            return SwipeDirection.Right;
        if (CIRCLE_BORDER_BUTTOM_RIGHT < dirAngle && dirAngle < CIRCLE_BORDER_BUTTOM_LEFT)
            return SwipeDirection.Down;
        if (CIRCLE_BORDER_BUTTOM_LEFT < dirAngle && dirAngle < CIRCLE_BORDER_TOP_LEFT)
            return SwipeDirection.Left;

        Debug.LogError("Swipe Error Cant Find Swipe Direction");
        return 0;
    }

    #endregion
}
public struct SwipeData
{
    public Vector2 SwipeStartPosition;
    public Vector2 SwipeEndPosition;
    public InputReciever.SwipeDirection SwipeDirection;
}

