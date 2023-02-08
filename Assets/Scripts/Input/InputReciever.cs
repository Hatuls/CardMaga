using System;
using UnityEngine;

[DefaultExecutionOrder(-999)]
public class InputReciever : MonoSingleton<InputReciever>
{
    #region Events

    public Touch? PlayerTouch { get; set; }
    public event Action<Vector2> OnTouchDetectedLocation;
    public event Action<Vector2> OnTouchEnded;
    public event Action<Vector2> OnTouchStart;
    public event Action<SwipeData> OnSwipeDetected;
    public event Action OnTouchDetected;

    #endregion
    
    #region Fields
    
    private const float CIRCLE_BORDER_TOP_LEFT = 0.25F;
    private const float CIRCLE_BORDER_TOP_RIGHT = -0.25F;
    private const float CIRCLE_BORDER_BUTTOM_RIGHT = -0.75F;
    private const float CIRCLE_BORDER_BUTTOM_LEFT = 0.75F;

    [SerializeField] private float _swipeDistance;
    [SerializeField] private bool _onlyDetectSwipeAtEnd;
    
    private Vector2 _startTouchWordPosition;
    private Vector2 _touchWordPosition;
    private Vector2 _endTouchWordPosition;
    
    private Vector2 _startTouchScreenPosition;
    private Vector2 _touchScreenPosition;
    private Vector2 _endTouchScreenPosition;
    
    private bool _isTouching = false;

    private bool _swipeDetected = false;
    
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

    public bool IsTouching
    {
        get => _isTouching;
    }
    
    public Vector2 StartTouchWordPosition => _startTouchWordPosition;
    public Vector2 TouchWordPosition => _touchWordPosition;
    public Vector2 EndTouchWordPosition => _endTouchWordPosition;

    public Vector2 StartTouchScreenPosition => _startTouchScreenPosition;
    public Vector2 EndTouchScreenPosition => _endTouchScreenPosition;
    public Vector2 TouchScreenPosition => _touchScreenPosition;
    
    #endregion

    #region Monobehaiviour CallBacks

    public override void Awake()
    {
        base.Awake();
        _camera = Camera.main;
    }

    private void Update()
    {
        TouchDetector();
    }

    #endregion

    #region Private Functions

    private void TouchDetector()
    {
        if (Input.touchCount > 0)
        {
            _isTouching = true;
            
            PlayerTouch = Input.GetTouch(0);

            if (PlayerTouch == null)
                return;

            GetTouchPhase(PlayerTouch.Value);

            return;
        }
        
        _isTouching = false;
    }

    private void GetTouchPhase(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                _startTouchScreenPosition = touch.position;
                _touchScreenPosition = touch.position;
                _endTouchScreenPosition = touch.position;
                OnTouchStart?.Invoke(_startTouchScreenPosition);
                OnTouchDetected?.Invoke();
                break;
            case TouchPhase.Moved:
                _touchScreenPosition = touch.position;
                if (!_onlyDetectSwipeAtEnd)
                    SwipeDetector(_startTouchScreenPosition,_touchScreenPosition);
                OnTouchDetectedLocation?.Invoke(_touchScreenPosition);
                break;
            case TouchPhase.Ended:
                _endTouchScreenPosition = touch.position;
                _touchScreenPosition = touch.position;
                if (_onlyDetectSwipeAtEnd)
                    SwipeDetector(_startTouchScreenPosition,_endTouchScreenPosition);
                _swipeDetected = false;
                OnTouchEnded?.Invoke(_endTouchScreenPosition);
                break;
            case TouchPhase.Canceled:
                break;
            case TouchPhase.Stationary:
                _touchScreenPosition = touch.position;
                OnTouchDetectedLocation?.Invoke(_touchScreenPosition);
                break;
        }
    }

    private void SwipeDetector(Vector2 start, Vector2 end)
    {
        if (PlayerTouch == null)
            return;

        if (Vector2.Distance(start,end) > _swipeDistance && !_swipeDetected)
        {
           // _swipeDetected = true; ?????
            
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

        float dirAngle = Vector2.SignedAngle(Vector2.up, dir) / 180;

        if (CIRCLE_BORDER_TOP_LEFT > dirAngle && dirAngle > CIRCLE_BORDER_TOP_RIGHT)
            return SwipeDirection.Up;
        if (CIRCLE_BORDER_TOP_RIGHT > dirAngle && dirAngle > CIRCLE_BORDER_BUTTOM_RIGHT)
            return SwipeDirection.Right;
        if (CIRCLE_BORDER_BUTTOM_RIGHT > dirAngle || dirAngle > CIRCLE_BORDER_BUTTOM_LEFT)
            return SwipeDirection.Down;
        if (CIRCLE_BORDER_BUTTOM_LEFT > dirAngle && dirAngle > CIRCLE_BORDER_TOP_LEFT)
            return SwipeDirection.Left;

        Debug.LogWarning("Swipe Error Cant Find Swipe Direction");
        return 0;
    }

    #endregion

    #region Editor:
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_startTouchScreenPosition * Vector3.one, 20f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_touchScreenPosition * Vector3.one, 20f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(_startTouchScreenPosition, _touchScreenPosition);
    }
    #endregion
}
public struct SwipeData
{
    public Vector2 SwipeStartPosition;
    public Vector2 SwipeEndPosition;
    public InputReciever.SwipeDirection SwipeDirection;

    public float SwipeDistance => Vector3.Distance(SwipeStartPosition, SwipeEndPosition);
}

