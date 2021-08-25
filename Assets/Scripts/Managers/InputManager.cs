using UnityEngine;
using UnityEngine.EventSystems;
public class InputManager : MonoSingleton<InputManager> , ITouchable
{
    #region Fields
    Touch? _playerTouch;
    [SerializeField] bool toUseRayCastHit = false;
    ITouchable _object;

    public ITouchable TouchableObject
    {
        get => _object;
        set
        {
           
            if (_object != value)
            {
                   _object?.ResetTouch();

                _object = value;
                
            }
        }
    }
    [Tooltip("Lock Screen From Touch")]
    [SerializeField] bool _isScreenLocked;
    Vector2 _touchPosOnScreen;
    Vector2 _firstTouchLocation;
    [SerializeField] Canvas _main;
    #endregion
    RaycastHit2D _raycastHit;


    #region Properties
    public bool SetLockedScreen
    {
        set {
            if (value != _isScreenLocked)
                _isScreenLocked = value;
        }
    }

    public RectTransform Rect => throw new System.NotImplementedException();
    #endregion


    #region Functions

    #region Monobehaiviour CallBacks
    private void Update()
    {
        if (_isScreenLocked == false)
            TouchDetector();
    }
    #endregion


    #region Private Functions
    private void TouchDetector() 
    {
        if (Input.touchCount > 0)
        {
            _playerTouch = Input.GetTouch(0);
          
            if (_playerTouch == null)
                return;
            
            _touchPosOnScreen = _playerTouch.Value.position;
            //_touchPosOnScreen = CameraController.Instance.GetTouchPositionOnUIScreen(_playerTouch.Value.position);
            if (toUseRayCastHit)
            {
                _raycastHit = Physics2D.Raycast(_touchPosOnScreen, Vector3.forward, 10f);

                if (_raycastHit.collider)
                {
                    AssignObjectFromTouch(_raycastHit.collider.GetComponent<IInputAbleObject>()?.GetTouchAbleInput);

                    RectTransformUtility.ScreenPointToLocalPointInRectangle(_object.Rect.parent.GetComponent<RectTransform>(), _touchPosOnScreen, _main.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main, out _touchPosOnScreen);

                }
            }



            if (TouchableObject != null)
                 OnTouch();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_touchPosOnScreen, 100*Vector3.forward);
    }
    private void OnTouch()
    {
        switch (_playerTouch.GetValueOrDefault().phase)
        {
            case TouchPhase.Began:
                _firstTouchLocation = _touchPosOnScreen;
                OnFirstTouch(in _touchPosOnScreen);
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                OnReleaseTouch(in _touchPosOnScreen);
                break;

            case TouchPhase.Stationary:
            case TouchPhase.Moved:
                OnHoldTouch(in _touchPosOnScreen,in _firstTouchLocation);
                break;

            default:
                Debug.LogError("Error Not Possible But We broke out of an enum");
                break;
        }
    }
    #endregion


    #region Public Functions
    public override void Init()
    {
        _isScreenLocked = false;
    }
    public void OnFirstTouch(in Vector2 touchPos)
    {
        _object.OnFirstTouch(touchPos);
    }
    public void OnReleaseTouch(in Vector2 touchPos)
    {

        _object.OnReleaseTouch(touchPos);
        ResetTouch();

    }

    public void OnHoldTouch(in Vector2 touchPos, in Vector2 startPos)
    {
        _object.OnHoldTouch(touchPos, startPos);
    }
    public void AssignObjectFromTouch(ITouchable objectTouched, Vector2 screenPos)
    {
        _firstTouchLocation = screenPos;
        AssignObjectFromTouch(objectTouched);
    }
    public void AssignObjectFromTouch( ITouchable objectTouched)
    {

        if (TouchableObject == objectTouched)
            return;
 


        if (TouchableObject != null)
            TouchableObject.ResetTouch();

            TouchableObject = objectTouched;
    }
    public void ResetTouch()
        => TouchableObject = null;
    #endregion


    #endregion
}


public interface IInputAbleObject
{
    ITouchable GetTouchAbleInput { get; }
}