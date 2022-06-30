
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("InputManager Was Not Initialized!");
            }
            return _instance;
        }
    }
    #region  Init
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            Init();
           // SceneHandler.onFinishLoadingScene += OnSceneLoad;
        }
        else if (_instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this);
    }


    private void OnDestroy()
    {
   //     SceneHandler.onFinishLoadingScene -= OnSceneLoad;
    }
    void Init()
    {
        inputState = InputState.Touch;
        _isScreenLocked = false;
        _object = null;
    }
    #endregion

    #region Fields
    public static Touch? PlayerTouch { get; set; }
    [SerializeField] bool toUseRayCastHit = false;
    [Sirenix.OdinInspector.ShowInInspector]
    ITouchable _object;
    public enum InputState { Touch = 1, Mouse = 2, None = 0 }
    public static InputState inputState;
    public ITouchable TouchableObject
    {
        get => _object;
        set
        {
            if (_object != value)
            {
                // _object?.ResetTouch();
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
    public bool LockScreen
    {
        set
        {
            if (value != _isScreenLocked)
                _isScreenLocked = value;
        }
    }

    public RectTransform Rect
        => _object == null ? null : _object.Rect;
    public bool IsInteractable
        => _object == null ? false : _object.IsInteractable;

    #endregion


    #region Functions

    #region Monobehaiviour CallBacks

    private void OnSceneLoad(ScenesEnum scene)
    {
        ResetTouch();
    }
    private void Update()
    {

        if (_isScreenLocked == false && !Battle.BattleManager.isGameEnded)
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


            //    _firstTouchLocation = Input.mousePosition;



            //   if(ShootRaycast2D(ref _firstTouchLocation))
            //    {
            //        OnStateEnter(_firstTouchLocation);
            //    }
            //    _touchPosOnScreen = _firstTouchLocation;
            //}
            //else if (Input.GetMouseButtonUp(0))
            //{

            //    OnStateExit(_touchPosOnScreen);
            // }
            //else if (Input.GetMouseButton(0))
            //{
            //   if (_object != null)
            //    {
            //        _touchPosOnScreen = Input.mousePosition;
            //        OnTick(_touchPosOnScreen, _firstTouchLocation);
            //    }


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
            PlayerTouch = Input.GetTouch(0);

            if (PlayerTouch == null)
                return;

            _touchPosOnScreen = PlayerTouch.Value.position;
            //_touchPosOnScreen = CameraController.Instance.GetTouchPositionOnUIScreen(_playerTouch.Value.position);
            if (toUseRayCastHit)
            {
                RayCastTouch();
            }



            if (TouchableObject != null)
                TouchableObject.OnTick(PlayerTouch.Value);
        }
    }
    private void RayCastTouch()
    {
        _raycastHit = Physics2D.Raycast(_touchPosOnScreen, Vector3.forward, 10f);

        if (_raycastHit.collider)
        {
            AssignObjectFromTouch(_raycastHit.collider.GetComponent<IInputAbleObject>()?.GetTouchAbleInput);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_object.Rect.parent.GetComponent<RectTransform>(), _touchPosOnScreen, _main.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main, out _touchPosOnScreen);

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

    #region Public Functions


    #region Assign and Remove
    public void AssignObjectFromTouch(ITouchable objectTouched, Vector2 screenPos)
    {
        _firstTouchLocation = screenPos;
        AssignObjectFromTouch(objectTouched);
    }
    public void AssignObjectFromTouch(ITouchable objectTouched)
    {

        if (objectTouched == null || TouchableObject == objectTouched || !objectTouched.IsInteractable)
            return;

        // Debug.Log("Object Recieved Input "+ objectTouched?.ToString());
        ResetTouch();
        TouchableObject = objectTouched;
    }
    public void RemoveObjectFromTouch()
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


public static class GameObjectExtentionMethod
{
    public static void SwitchActiveState(this GameObject go)
    => go.SetActive(!go.activeSelf);
    public static void Destroy(this GameObject go) => GameObject.Destroy(go);

}



public interface IInputAbleObject
{
    ITouchable GetTouchAbleInput { get; }
}