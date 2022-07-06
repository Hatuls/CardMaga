
using Battle.Turns;
using Cinemachine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineBrain _cinemachineBrain;

    [SerializeField]
    private TransitionCamera _defaultCamera;

    public static List<VirtualCamera> _cameras = new List<VirtualCamera>();

    private VirtualCamera _activeCamera = null;

    private void Awake()
    {
        EndEnemyTurn.OnEndEnemyTurn += ReturnToDefaultCamera;
        EndPlayerTurn.OnEndPlayerTurn+= ReturnToDefaultCamera;
        AnimatorController.OnAnimationStart += SwitchCamera;
       // AnimatorController.OnAnimationEnding += ReturnToDefaultCamera;
    }

    private void OnDestroy()
    {
        EndEnemyTurn.OnEndEnemyTurn -= ReturnToDefaultCamera;
        EndPlayerTurn.OnEndPlayerTurn -= ReturnToDefaultCamera;
        AnimatorController.OnAnimationStart -= SwitchCamera;
        //AnimatorController.OnAnimationEnding -= ReturnToDefaultCamera;
    }

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



    #region public

    [Button]
    public void ReturnToDefaultCamera()
    {
        SwitchCamera(_defaultCamera);
    }
    
    public void SwitchCamera(TransitionCamera transitionCamera)
    {
        if (!CheckTransitionCamera(transitionCamera))
            return;

        if (TryReturnVirtualCamera(transitionCamera.NextCamID, out VirtualCamera virtualCamera) && !IsActiveCamera(virtualCamera))
        {
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

}
