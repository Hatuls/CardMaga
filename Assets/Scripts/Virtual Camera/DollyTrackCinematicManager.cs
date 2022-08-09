using System;
using Cinemachine;
using ReiTools.TokenMachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using Battle;
using Managers;

public class DollyTrackCinematicManager : MonoBehaviour
{
    [FormerlySerializedAs("_introCinematicCam")] [SerializeField] CinemachineVirtualCamera _cinematicCam;
    CinemachineTrackedDolly _dollyTrack;
    [SerializeField] int _defaultTrackNumber;
    [SerializeField] bool _isRandomTrack;
    [FormerlySerializedAs("_tracks")] [SerializeField] CinemachineSmoothPath[] _winTracks;
    [SerializeField] CinemachineSmoothPath[] _loseTracks;
    [SerializeField] CinemachineSmoothPath[] _introTracks;
    [SerializeField] UnityEvent OnCinematicEnded;
    private CinemachineSmoothPath _introPath;
    private CinemachineSmoothPath _outroWinPath;
    private CinemachineSmoothPath _outroLosePath;
    System.IDisposable _token;

    private void Awake()
    {
        if (_dollyTrack == null)
            _dollyTrack = _cinematicCam.GetCinemachineComponent<CinemachineTrackedDolly>();
        _cinematicCam.gameObject.SetActive(false);
        const int order = 2;
        BattleStarter.Register(new SequenceOperation(StartCinematicTrack, order), BattleStarter.BattleStarterOperationType.Start);
    }

 

    public void StartCinematicTrack(ITokenReciever tokenMachine)
    {
        _token = tokenMachine.GetToken();

        //SetTrack(_winTracks,ref _outroWinPath);
        //SetTrack(_loseTracks,ref _outroLosePath);
        SetTrack(_introTracks,ref _introPath);
        StartIntroCinematic();
    }
    
    private void SetTrack(CinemachineSmoothPath[] tracks,ref CinemachineSmoothPath path)
    {
        var track = tracks[_defaultTrackNumber];
        if (_isRandomTrack)
        {
            var randomTrack = Random.Range(0, tracks.Length);
            track = tracks[randomTrack];
        }
        path = track;
    }

    private void ResetTrack()
    {
        if (!CinemachineCore.Instance.IsLive(_cinematicCam))
        {
            _dollyTrack.m_PathPosition = _dollyTrack.m_Path.MinPos;
        }
    }
    
    private void StartIntroCinematic()
    {
        StartIntroCinematic(null);
    }

    public void StartWinCinematic(Action onComplete = null)
    {
        _dollyTrack.m_Path = _outroWinPath;
        StartCinematic(onComplete);
    }
    
    public void StartLoseCinematic(Action onComplete = null)
    {
        _dollyTrack.m_Path = _outroLosePath;
        StartCinematic(onComplete);
    }
    
    private void StartIntroCinematic(Action onComplete = null)
    {
        _dollyTrack.m_Path = _introPath;
        StartCinematic(onComplete);
    }
    
    private void StartCinematic(Action onComplete = null)
    {
        _cinematicCam.Priority = 10;
        ResetTrack();
        if (!_cinematicCam.gameObject.activeSelf)
        {
            _cinematicCam.gameObject.SetActive(true);
        }
        StartCoroutine(StartMovement(onComplete));
    }
    IEnumerator StartMovement(Action onComplete = null)
    {
        while (_dollyTrack.m_PathPosition < _dollyTrack.m_Path.MaxPos)
        {
            _dollyTrack.m_PathPosition += Time.deltaTime;
            yield return null;
        }
        _cinematicCam.Priority = -1;
        _cinematicCam.gameObject.SetActive(false);
        _token?.Dispose();
        onComplete?.Invoke();
        OnCinematicEnded?.Invoke();
    }
}
