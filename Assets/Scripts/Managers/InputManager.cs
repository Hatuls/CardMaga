using Battles.UI;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
public class InputManager : MonoSingleton<InputManager> , ITouchable
{
    #region Fields
    Touch? _playerTouch;
    [SerializeField] bool toUseRayCastHit = false;
    ITouchable _object;
    public enum InputState { Touch=1,Mouse=2,None = 0}
    public static InputState inputState;
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

    public bool IsInteractable => throw new NotImplementedException();
    #endregion


    #region Functions

    #region Monobehaiviour CallBacks
    private void Update()
    {
        if (_isScreenLocked == false)
        {
            TouchDetector();
            MouseDetector();
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void MouseDetector()
    {
        if (Input.GetMouseButtonDown(0))
        {
         
            inputState = InputState.Mouse;

       
         //   RectTransformUtility.ScreenPointToLocalPointInRectangle(_main.GetComponent<RectTransform>(), Input.mousePosition, _main.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main, out _firstTouchLocation);
   
            _firstTouchLocation = Input.mousePosition;

           if(ShootRaycast2D(ref _firstTouchLocation))
            {
                OnFirstTouch(_firstTouchLocation);
            }
            _touchPosOnScreen = _firstTouchLocation;
        }
        else if (Input.GetMouseButtonUp(0))
        {

            OnReleaseTouch(_touchPosOnScreen);
            Debug.Log("Release Click");
        }
        else if (Input.GetMouseButton(0))
        {
         //   RectTransformUtility.ScreenPointToLocalPointInRectangle(_object != null ? _object.Rect: _main.GetComponent<RectTransform>(), Input.mousePosition, _main.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main, out _touchPosOnScreen);
           // _touchPosOnScreen = Physics2D.Raycast(_touchPosOnScreen, Vector3.forward, 10f); Input.mousePosition;


            //  if (ShootRaycast2D(ref _touchPosOnScreen))
            if (_object != null)
            {
              Debug.Log("Holding Click");
                _touchPosOnScreen = Input.mousePosition;
                    OnHoldTouch(_touchPosOnScreen, _firstTouchLocation);
            }
                

        }
    }


    private bool ShootRaycast2D(ref Vector2 position)
    {
        _raycastHit = Physics2D.Raycast(position, Vector3.forward, 10f);

        if (_raycastHit.collider)
        {
            AssignObjectFromTouch(_raycastHit.collider.GetComponent<IInputAbleObject>()?.GetTouchAbleInput);
           // Debug.Log("DetectInput");
            //    RectTransformUtility.ScreenPointToLocalPointInRectangle(_object.Rect.parent.GetComponent<RectTransform>(), position, _main.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main, out position);
            return true;
        }
        return false;
    }
    #endregion


    #region Private Functions
    private void TouchDetector() 
    {
        if (Input.touchCount > 0)
        {
            inputState = InputState.Touch;
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
        Gizmos.color = Color.green;
        Gizmos.DrawRay(_firstTouchLocation, 100*Vector3.forward);
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
        inputState = InputState.Touch;
        _isScreenLocked = false;
    }
    public void OnFirstTouch(in Vector2 touchPos)
    {
        _object?.OnFirstTouch(touchPos);
    }
    public void OnReleaseTouch(in Vector2 touchPos)
    {

        _object?.OnReleaseTouch(touchPos);
        RemoveObjectrFromTouch();
    }

    public void OnHoldTouch(in Vector2 touchPos, in Vector2 startPos)
    {
        _object?.OnHoldTouch(touchPos, startPos);
    }

    #region Assign and Remove
    public void AssignObjectFromTouch(ITouchable objectTouched, Vector2 screenPos)
    {
        _firstTouchLocation = screenPos;
        AssignObjectFromTouch(objectTouched);
    }
    public void AssignObjectFromTouch( ITouchable objectTouched)
    {

        if ( objectTouched == null || TouchableObject == objectTouched || !objectTouched.IsInteractable)
            return;

        Debug.Log("Object Recieved Input "+ objectTouched?.ToString());
            TouchableObject = objectTouched;
    }
    public void RemoveObjectrFromTouch()
        => ResetTouch();
    public void ResetTouch()
    {
        TouchableObject?.ResetTouch();
        TouchableObject = null;
    }
    #endregion


    #endregion
    #endregion
}


public interface IInputAbleObject
{
    ITouchable GetTouchAbleInput { get; }
}