
using Battle;
using Battle.Turns;
using Cinemachine;
using Managers;
using ReiTools.TokenMachine;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraManager : MonoBehaviour, ISequenceOperation<BattleManager>
{
    [SerializeField]
    private CinemachineBrain _cinemachineBrain;

    [SerializeField]
    private TransitionCamera _defaultCamera;

    [SerializeField]
    private TransitionCamera _eyalTestCamera;

    public static List<VirtualCamera> _cameras = new List<VirtualCamera>();

    private VirtualCamera _activeCamera = null;


    [SerializeField, Tooltip("The Time till the camera will return to default position"), Min(0)]
    private float _delayTillReturn;
    private float _counter = 0;
    private Coroutine _coroutineCallback;
    private Coroutine _delayTimerCoroutine;
    private bool IsDefaultCamera
    {
        get
        {
            return _activeCamera?.GetCameraID == _defaultCamera?.NextCamID;
        }
    }

    public int Priority =>10;

    private void Awake()
    {
        _counter = 0;
        AnimatorController.OnAnimationStart += SwitchCamera;
        BattleManager.Register(this, OrderType.Default);
        // AnimatorController.OnAnimationEnding += ReturnToDefaultCamera;
    }



    private void Start()
    {
        if (!TryReturnVirtualCamera(_defaultCamera.NextCamID, out _activeCamera))
            throw new System.Exception("Default Camera was not assigned!");
    }



    #region Private
    private bool IsActiveCamera(VirtualCamera camera)
    {
        return camera == _activeCamera;
    }

    private bool IsActiveCamera(CameraIdentification camera)
    {
        return camera == _activeCamera.GetCameraID;
    }


    /// <summary>
    /// Translating scriptable object into virtual camera in the scene
    /// </summary>
    /// <param name="cameraID"></param>
    /// <returns></returns>
    private bool TryReturnVirtualCamera(CameraIdentification cameraID, out VirtualCamera virtualCamera)
    {
        foreach (VirtualCamera vcam in _cameras)
        {
            if (vcam.GetCameraID == cameraID)
            {
                virtualCamera = vcam;
                return true;
            }
        }
        virtualCamera = null;
        return false;
    }

    private void SwitchPriority(VirtualCamera camera,Action onComplete = null)
    {
        _activeCamera = camera;
        _activeCamera.ChangePriority(10);
   
        foreach (VirtualCamera currentCamera in _cameras)
        {
            if (currentCamera != camera)
            {
                currentCamera.ChangePriority(0);
            }
        }
        if(_coroutineCallback!=null)
        StopCoroutine(_coroutineCallback);
       _coroutineCallback =  StartCoroutine(TransitionCameraCallback(onComplete));
    }


    private bool CheckTransitionCamera(TransitionCamera transitionCamera)
    {
        if (transitionCamera == null)
        {
            Debug.LogError("The transitionCamera is missing!");
            return false;
        }
        if (transitionCamera.CustomBlend == null)
        {
            Debug.LogError("The CustomBlend is missing!");
            return false;
        }

        if (transitionCamera.NextCamID == null)
        {
            Debug.LogError("The NextCamID is missing!");
            return false;
        }

        return true;
    }

    private IEnumerator DelayTillReturn(Action onComplete = null)
    {
        Transform current = _cinemachineBrain.transform;



        Debug.Log("Camera Animation Started");
        while (_delayTillReturn > _counter)
        {
            do
            {
                yield return null;
            } while (Vector3.Distance(current.position, _activeCamera.transform.position) >= Mathf.Epsilon);

            _counter += Time.deltaTime;
        }
        Debug.Log("Camera Animation Finished");
        ReturnToDefaultCamera();
    }

    private void StopCounter()
    {
        if (_delayTimerCoroutine != null)
        {
            StopCoroutine(DelayTillReturn());
            _delayTimerCoroutine = null;
        }
    }
    #endregion


    #region public
    public void ReturnToDefaultCamera(ITokenReciever tokenMachine)
    {
        IDisposable disposable = tokenMachine.GetToken();
        StopCounter();
        SwitchCamera(_defaultCamera, disposable.Dispose);
    }

    public void ReturnToDefaultCamera()
    {
        StopCounter();
        SwitchCamera(_defaultCamera);
    }

    [Button]
    public void EyalThisIsForYouEmojiHeart()
    {
        SwitchCamera(_eyalTestCamera);
    }

    public void SwitchCamera(TransitionCamera transitionCamera,Action onCallback = null)
    {
        if (!CheckTransitionCamera(transitionCamera))
            return;

   
        if (TryReturnVirtualCamera(transitionCamera.NextCamID, out VirtualCamera virtualCamera) && !IsActiveCamera(virtualCamera))
        {

            if (IsDefaultCamera)
                StartTimer();

            _cinemachineBrain.m_CustomBlends = transitionCamera.CustomBlend;
            SwitchPriority(virtualCamera, onCallback);

        }
    }
    private IEnumerator TransitionCameraCallback( Action onComplete = null)
    {
        while (_cinemachineBrain.IsBlending)
            yield return null;
        onComplete?.Invoke();
    }
    private void StartTimer(Action onComplete = null)
    {

        _delayTimerCoroutine= StartCoroutine(DelayTillReturn(onComplete));

    }

    public static void Register(VirtualCamera camera)
    {
        _cameras.Add(camera);
    }

    public static void Unregister(VirtualCamera camera)
    {
        _cameras.Remove(camera);
    }
    #endregion


    public void ResetCameraCounter()
    {
        Debug.Log("Animation Reset");
        _counter = 0;
    }

    public void ExecuteTask(ITokenReciever tokenMachine, BattleManager data)
    {
       var _turnHandler = data.TurnHandler;
        _turnHandler.GetTurn(GameTurnType.LeftPlayerTurn).OnTurnExit += ReturnToDefaultCamera;
        _turnHandler.GetTurn(GameTurnType.RightPlayerTurn).OnTurnExit += ReturnToDefaultCamera;
        data.OnBattleManagerDestroyed += GameDestroyed;
    }
    private void GameDestroyed(BattleManager bm)
    {
        var _turnHandler = bm.TurnHandler;
        _turnHandler.GetTurn(GameTurnType.LeftPlayerTurn).OnTurnExit  -= ReturnToDefaultCamera;
        _turnHandler.GetTurn(GameTurnType.RightPlayerTurn).OnTurnExit -= ReturnToDefaultCamera;
        AnimatorController.OnAnimationStart -= SwitchCamera;
        bm.OnBattleManagerDestroyed -= GameDestroyed;
        StopCounter();
        //AnimatorController.OnAnimationEnding -= ReturnToDefaultCamera;
    }
}
