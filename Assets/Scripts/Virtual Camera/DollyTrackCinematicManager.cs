using Cinemachine;
using ReiTools.TokenMachine;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DollyTrackCinematicManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _introCinematicCam;
    CinemachineTrackedDolly _dollyTrack;
    [SerializeField] int _defaultTrackNumber;
    [SerializeField] bool _isRandomTrack;
    [SerializeField] CinemachineSmoothPath[] _tracks;
    [SerializeField] UnityEvent OnCinematicEnded;
    System.IDisposable _token;

    private void Awake()
    {
        if (_dollyTrack == null)
            _dollyTrack = _introCinematicCam.GetCinemachineComponent<CinemachineTrackedDolly>();
        _introCinematicCam.gameObject.SetActive(false);

        SceneHandler.OnBeforeSceneShown += SceneStarted;
    }
    private void OnDestroy()
    {
        SceneHandler.OnBeforeSceneShown -= SceneStarted;
    }
    private void SceneStarted(ITokenReciever tokenMachine)
    {
        _token = tokenMachine.GetToken();

        SetTrack();
        StartCinematic();
    }
    [Button]
    private void SetTrack()
    {
        var track = _tracks[_defaultTrackNumber];
        if (_isRandomTrack)
        {
            var randomTrack = Random.Range(0, _tracks.Length);
            track = _tracks[randomTrack];
        }
        _dollyTrack.m_Path = track;
    }
    private void ResetTrack()
    {
        if (!CinemachineCore.Instance.IsLive(_introCinematicCam))
        {
            _dollyTrack.m_PathPosition = _dollyTrack.m_Path.MinPos;
        }
    }
    [Button]
    public void StartCinematic()
    {
        _introCinematicCam.Priority = 10;
        ResetTrack();
        if (!_introCinematicCam.gameObject.activeSelf)
        {
            _introCinematicCam.gameObject.SetActive(true);
        }
        StartCoroutine(StartMovement());
    }
    IEnumerator StartMovement()
    {
        while (_dollyTrack.m_PathPosition < _dollyTrack.m_Path.MaxPos)
        {
            _dollyTrack.m_PathPosition += Time.deltaTime;
            yield return null;
        }
        _introCinematicCam.Priority = -1;
        _introCinematicCam.gameObject.SetActive(false);
        _token?.Dispose();
        OnCinematicEnded?.Invoke();
    }
}
