using Unity.Events;
using Cinemachine;
using System.Collections;
using UnityEngine;
[RequireComponent(typeof(IntListener))]
public class CameraController : MonoSingleton<CameraController>
{
    public enum CameraAngleLookAt { Enemy=0  ,Both =1, Player=2}





    //[SerializeField] Camera _camera;
    //[SerializeField] Canvas _canvas;
    // [SerializeField] RectTransform _parentCanvas;
    // [SerializeField] LeanTweenType[] _cameraShakes;
    [SerializeField] GameObject IntroCinematic; // 
    [SerializeField] GameObject MoveCameraAngle;
    [SerializeField] GameObject ShakeAtPlayer;
    [SerializeField] GameObject ShakeAtMiddle;
    [SerializeField] GameObject ShakeAtEnemy;


    [SerializeField] float _returnSpeed;
    [SerializeField] float _goToSpeed;
    private CameraAngleLookAt _cameraAngleLookAt;
    [SerializeField] CinemachineVirtualCamera _cinemachineVirtualCamera;
    private CinemachineTrackedDolly _cineMachineTrackedDolly;
    public CinemachineTrackedDolly GetAngleTrackedDolly
    {
        get
        {
            if (_cineMachineTrackedDolly== null)
                _cineMachineTrackedDolly = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();

            return _cineMachineTrackedDolly;
        }
    }

    private void Start()
    {
        _cameraAngleLookAt = CameraAngleLookAt.Both;
        return;
        MoveCameraAngle.SetActive(false);
        ShakeAtEnemy.SetActive(false);
        ShakeAtMiddle.SetActive(false);
        ShakeAtPlayer.SetActive(false);

        IntroCinematic.SetActive(true);

        //wait till animation ends
        //IntroCinematic.SetActive(false);
        //MoveCameraAngle set to 1 (middle position)
    }
    public void MoveCameraAnglePos(int index)
    {
        if (GetAngleTrackedDolly.m_PathPosition != index)
        {

            StartCoroutine(CameraTransition(index));
        }
        //0 is player
        //1 is middle
        //2 is Enemy
    }
    IEnumerator CameraTransition(int point)
    {

    
        float lerpTime = point == 1 ?  _returnSpeed : _goToSpeed;
      
        for (float i = 0; i <= lerpTime;i += Time.deltaTime)
        {
            GetAngleTrackedDolly.m_PathPosition = Mathf.Lerp(
                (float)_cameraAngleLookAt,
                (float)point,
                i / lerpTime);
            yield return null;

        }
        _cameraAngleLookAt = (CameraAngleLookAt)point;
       
    }
    public void Shake(int moveCameraAngleIndex)
    {
        //0 is shake at player
        //1 is shake at middle
        //2 is shake at Enemy
    }
    //Vector3 startPos;
    //public Camera GetCamera => _camera;
    public override void Init()
    {
        //_camera = Camera.main;
        //startPos = transform.position;
    }

    //public static void ShakeCamera()
    //{
    //    float rotation;
    //    do
    //    {
    //        rotation = UnityEngine.Random.Range(-4f, 4f);
    //    } while (Mathf.Abs(rotation) < 2.5f);


    //    LeanTween.rotateZ(Instance.gameObject,
    //        rotation,
    //        0.5f)
    //        .setEase(Instance.GetRandomCameraShakeCurve()).
    //        setOnComplete(() => LeanTween.rotateZ(Instance.gameObject, 0, 0.2f).setEase(Instance.GetRandomCameraShakeCurve()));
    //}

    //private LeanTweenType GetRandomCameraShakeCurve()
    //{
    //    if (_cameraShakes == null || _cameraShakes.Length == 0)
    //        return LeanTweenType.easeInSine;

    //    return _cameraShakes[UnityEngine.Random.Range(0, _cameraShakes.Length)];
    //}

    //void ZoomOnObject(in Transform target,in float howMuchZoom) {  }
}
