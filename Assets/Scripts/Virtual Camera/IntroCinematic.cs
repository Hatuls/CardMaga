using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;

public class IntroCinematic : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _introCinematicCam;
    [SerializeField] CinemachineSmoothPath _dollyTrack;
    bool _isMoving = false;
    public void Start()
    {
        //_introCinematicCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = _dollyTrack.MinPos;
    }
    [Button]
    public void ResetTrack()
    {
       // _introCinematicCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = _dollyTrack.MinPos;
    }
    [Button]
    public void StartCinematic()
    {
        if (!_introCinematicCam.gameObject.activeSelf)
        {
            _introCinematicCam.gameObject.SetActive(true);
        }
        _isMoving = true;
    }
    private void Update()
    {
        //if (_isMoving)
        //{
        //    _dollyTrack.m_PathPosition += Time.deltaTime;
        //    if (_dollyTrack.m_PathPosition>=_dollyTrack.m_Path.MaxPos)
        //    {
        //        _introCinematicCam.gameObject.SetActive(false);
        //        _isMoving = false;
        //    }
        //}
    }
}
