using UnityEngine;
using UnityEngine.EventSystems;
public class InputManager : MonoSingleton<InputManager> , ITouchable
{
    #region Fields
    Touch? _playerTouch;

    ITouchable _object;
    [Tooltip("Lock Screen From Touch")]
    [SerializeField] bool _isScreenLocked;
    Vector2 _touchPosOnScreen;
    Vector2 _firstTouchLocation;
    #endregion


    #region Properties
    public bool SetLockedScreen
    {
        set {
            if (value != _isScreenLocked)
                _isScreenLocked = value;
        }
    }
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

                //CameraController.Instance.GetTouchPositionOnScreen(_playerTouch.Value.position);

            OnTouch();
        }
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
                if (Vector3.Distance(_firstTouchLocation, _touchPosOnScreen) > 0.5f)
                OnHoldTouch(in _touchPosOnScreen);
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
        if (_object != null)
            _object.OnFirstTouch(touchPos);

    }

    public void OnReleaseTouch(in Vector2 touchPos)
    {
        if (_object != null)
        {
            _object.OnReleaseTouch(touchPos);
            _object = null;
        }
    }

    public void OnHoldTouch(in Vector2 touchPos)
    {
        if (_object != null)
            _object.OnHoldTouch(touchPos);
    }

    public void AssignObjectFromTouch( ITouchable objectTouched)
    {
        if (objectTouched == null)
            return;
        Debug.Log("Touched!");
         _object = objectTouched;
    }
    public void RemoveObjectFromTouch()
        => _object = null;
    #endregion


    #endregion
}
