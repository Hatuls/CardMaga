using Unity.Events;
using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using ReiTools.TokenMachine;

[RequireComponent(typeof(IntListener))]
public class CameraController : MonoSingleton<CameraController>
{
    public enum CameraAngleLookAt { Player = 0, Both = 1, Enemy = 2 }





    //[SerializeField] Camera _camera;
    //[SerializeField] Canvas _canvas;
    // [SerializeField] RectTransform _parentCanvas;
    // [SerializeField] LeanTweenType[] _cameraShakes;
    [SerializeField] GameObject IntroCinematic; // 
    [SerializeField] PlayableDirector IntroDirector;
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
            if (_cineMachineTrackedDolly == null)
                _cineMachineTrackedDolly = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();

            return _cineMachineTrackedDolly;
        }
    }

    private void Start()
    {
        _cameraAngleLookAt = CameraAngleLookAt.Both;

        GetAngleTrackedDolly.m_PathPosition = (int)_cameraAngleLookAt;

    }
    public void MoveCameraAnglePos(int index)
    {
        if (Battle.BattleManager.isGameEnded)
            return;

        if ((int)_cameraAngleLookAt != index)
        {
            StopCoroutine(CameraTransition(index));
            StartCoroutine(CameraTransition(index));
        }
        //0 is Enemy
        //1 is middle
        //2 is player
    }
    IEnumerator CameraTransition(int point)
    {


        float lerpTime = point == 1 ? _returnSpeed : _goToSpeed;

        for (float i = 0; i <= lerpTime; i += Time.deltaTime)
        {
            GetAngleTrackedDolly.m_PathPosition = Mathf.Lerp(
                (float)_cameraAngleLookAt,
                (float)point,
                i / lerpTime);
            yield return null;

        }
        _cameraAngleLookAt = (CameraAngleLookAt)point;

    }
    public void StartIntro()
    {
      
        IntroDirector.Play();
    }
    public void Shake(int moveCameraAngleIndex)
    {
        //0 is shake at player
        //1 is shake at middle
        //2 is shake at Enemy
    }
    //Vector3 startPos;
    //public Camera GetCamera => _camera;
    public override void Init(ITokenReciever token)
    {
        //_camera = Camera.main;
        //startPos = transform.position;
    }



    #region Monobehaviour Callbacks 
    public override void Awake()
    {
        base.Awake();
        SceneHandler.OnBeforeSceneShown += Init;
    }
    public void OnDestroy()
    {
        SceneHandler.OnBeforeSceneShown -= Init;

    }
    #endregion
}
