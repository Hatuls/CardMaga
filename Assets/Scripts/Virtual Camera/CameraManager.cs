
using Battle.Turns;
using Cinemachine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineBrain _cinemachineBrain;

    [SerializeField]
    private TransitionCamera _defaultCamera;

    [SerializeField]
    private TransitionCamera _eyalTestCamera;

    public static List<VirtualCamera> _cameras = new List<VirtualCamera>();

    private VirtualCamera _activeCamera = null;


    [SerializeField,Tooltip("The Time till the camera will return to default position"),Min(0)]
    private float _delayTillReturn;
    private float _counter = 0 ;


    private bool IsDefaultCamera
    {
        get
        {
            return _activeCamera.GetCameraID == _defaultCamera.NextCamID;
        }
    }

    private void Awake()
    {
        _counter = 0;
        EndEnemyTurn.OnEndEnemyTurn += ReturnToDefaultCamera;
        EndPlayerTurn.OnEndPlayerTurn += ReturnToDefaultCamera;
        AnimatorController.OnAnimationStart += SwitchCamera;


        // AnimatorController.OnAnimationEnding += ReturnToDefaultCamera;
    }
    private void Start()
    {
        if (!TryReturnVirtualCamera(_defaultCamera.NextCamID, out _activeCamera))
            throw new System.Exception("Default Camera was not assigned!");
    }
    private void OnDestroy()
    {
        EndEnemyTurn.OnEndEnemyTurn -= ReturnToDefaultCamera;
        EndPlayerTurn.OnEndPlayerTurn -= ReturnToDefaultCamera;
        AnimatorController.OnAnimationStart -= SwitchCamera;
        StopCounter();
        //AnimatorController.OnAnimationEnding -= ReturnToDefaultCamera;
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

    private void SwitchPriority(VirtualCamera camera)
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


    private IEnumerator DelayTillReturn()
    {
        Debug.Log("Camera Animation Started");
        while (_delayTillReturn >= _counter)
        {
            yield return null;
            _counter += Time.deltaTime;
        }
        Debug.Log("Camera Animation Finished");
        ReturnToDefaultCamera();
    }

    private void StopCounter() => StopCoroutine(DelayTillReturn());
    #endregion


    #region public


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

    public void SwitchCamera(TransitionCamera transitionCamera)
    {
        if (!CheckTransitionCamera(transitionCamera))
            return;

        if (TryReturnVirtualCamera(transitionCamera.NextCamID, out VirtualCamera virtualCamera) && !IsActiveCamera(virtualCamera))
        {
            ResetCameraCounter();
            if (IsDefaultCamera)
            {
                StartCoroutine(DelayTillReturn());
            }

            _cinemachineBrain.m_CustomBlends = transitionCamera.CustomBlend;
            SwitchPriority(virtualCamera);

        }
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



   
}
