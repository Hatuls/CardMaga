using Cinemachine;
using UnityEngine;
namespace CardMaga.Battle.Visual.Camera
{

    public class VirtualCamera : MonoBehaviour
    {
        [SerializeField] private CameraIdentification _cameraIdentification;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;

        public CameraIdentification GetCameraID
        {
            get { return _cameraIdentification; }
        }

        public CinemachineVirtualCamera GetVirtualCamera
        {
            get { return _virtualCamera; }
        }

        public void ChangePriority(int priority)
        {
            _virtualCamera.m_Priority = priority;
        }

        public void OnEnable()
        {
            CameraManager.Register(this);
        }

        public void OnDisable()
        {
            CameraManager.Unregister(this);
        }
    }
}