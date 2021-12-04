using DesignPattern;
using Map;
using UnityEngine;

public class CameraMovement : MonoBehaviour, IObserver
{
    //when entering scene the floor of the current avilable locations should be around the bottom part of the screen
    // when pressing the left mouse button:
    //1. find the current location of the mouse
    //2. if the movement of the mouse is upwards{move down}
    //3. else if the movement of the mouse is downwards{move up}
    //can not move down or up more than the minimum or maximum of the script
    [SerializeField] ObserverSO _raycastObserver;
    [SerializeField]
    float _minCamPosition;
    [SerializeField]
    float _maxCamPosition;
    [SerializeField]
    float _camSpeed;
    [SerializeField]
    Camera _camera;
    [SerializeField] [Range(0,1)]
    float _divideScreenOffset = 0.75f;
    [SerializeField]
    float _movementTime = 1;


    
    private static float _lastVisitedNodeY;

    public float LastVisitedNodeY
    { set
        {
            _lastVisitedNodeY = value;
            
            SetCameraPosition(CameraOffset(_lastVisitedNodeY), _movementTime);
        }
    }

    Vector3 _dragOrigin;
   public static bool _toLock;

    private void Start()
    {
        if(_lastVisitedNodeY != 0)
        {

            SetCameraPosition(CameraOffset(_lastVisitedNodeY), _movementTime);

        }
    }
    private void OnEnable()
    {
        _raycastObserver.Subscribe(this);
    }
    private void OnDisable()
    {
        _raycastObserver.UnSubscribe(this);
    }
    private void Update()
    {
        if (!_toLock)
            MoveCam();
    }
    void MoveCam()
    {
        if (Input.GetMouseButtonUp(0))
        {
            _raycastObserver.Notify(null);
            return;
        }
        if (Input.GetMouseButtonDown(0))
            _dragOrigin = _camera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButton(0))
        {
            _raycastObserver.Notify(this);
            Vector3 delta = _dragOrigin - _camera.ScreenToWorldPoint(Input.mousePosition);
             _camera.transform.position += new Vector3(0, delta.y, 0);

            SetCameraPosition(_camera.transform.position.y,0);
        }
    }

    public void OnNotify(IObserver Myself)
    {
        if (Myself == null || (Object)Myself == this)
            _toLock = false;
        else if ((Object)Myself != this)
            _toLock = true;
    }
    public void SetCameraPosition(float yPos, float timer)
    {
        var toPos =  new Vector3(_camera.transform.position.x, Mathf.Clamp(yPos, _minCamPosition, _maxCamPosition), _camera.transform.position.z);
        LeanTween.move(gameObject, toPos, timer);
    }
    private float CameraOffset(float yPos)
    {
        var screenOffset = (ScreenCoordinates._screenMaxY - ScreenCoordinates._screenMinY) * _divideScreenOffset;
        return yPos + screenOffset;
    }
}
