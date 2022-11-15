using System;
using Cinemachine;
using ReiTools.TokenMachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using Battle;
using CardMaga.SequenceOperation;
using CardMaga.Battle.UI;
using CardMaga.Battle;

namespace CardMaga.Battle.Visual.Camera
{

}
public class DollyTrackCinematicManager : MonoBehaviour, ISequenceOperation<IBattleUIManager>
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
    private EndBattleHandler _endBattleHandler;
    public int Priority => 0;
    private void Awake()
    {
        if (_dollyTrack == null)
            _dollyTrack = _cinematicCam.GetCinemachineComponent<CinemachineTrackedDolly>();
        _cinematicCam.gameObject.SetActive(false);

       // BattleManager.Register(new OperationTask<IBattleManager>(StartCinematicTrack, order), OrderType.After);
    }

 

    public void ExecuteTask(ITokenReciever tokenMachine, IBattleUIManager battleManager)
    {
        _token = tokenMachine.GetToken();
        _endBattleHandler = battleManager.BattleDataManager.EndBattleHandler;
     //  _endBattleHandler.OnBattleFinished += StartWinCinematic;
     //  SetTrack(_winTracks,ref _outroWinPath);
     //  SetTrack(_loseTracks,ref _outroLosePath);
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
    private void StartWinCinematic(ITokenReciever tokenReciever)
    {
        _token = tokenReciever?.GetToken();
        if (_endBattleHandler.IsLeftPlayerWon)
            StartWinCinematic(_token.Dispose);
        else
            StartLoseCinematic(_token.Dispose);
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
    private void OnDestroy()
    {
        if(_endBattleHandler != null)
        _endBattleHandler.OnBattleFinished -= StartWinCinematic;
    }
}
