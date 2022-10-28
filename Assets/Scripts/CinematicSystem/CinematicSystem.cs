using UnityEngine;
using UnityEngine.Events;

public class CinematicSystem : MonoBehaviour
{
    [SerializeField, EventsGroup] private UnityEvent OnCinematicSequenceStart;
    [SerializeField, EventsGroup] private UnityEvent OnCinematicSequencePuse;
    [SerializeField, EventsGroup] private UnityEvent OnCinematicSequenceResume;
    [SerializeField, EventsGroup] private UnityEvent OnCinematicSequenceEnd;

    [SerializeField] private CinematicHandler[] _cinematic;

    private Coroutine _currentRunningCinematic;
    private CinematicHandler _currentCinematic;
    private int _currentCinematicIndex;

    private void Start()
    {
        Init();
        StartCinematicSequence();
    }

    public void Init()
    {
        _currentCinematicIndex = -1;
        
        for (int i = 0; i < _cinematic.Length; i++)
        {
            _cinematic[i].OnCinematicCompleted += CinematicComplete;
            _cinematic[i].Init(i);
        }
    }

    public void StartCinematicSequence()
    {
        OnCinematicSequenceStart?.Invoke();
        StartNextCinematic();
    }
    
    [ContextMenu("Resume Sequence")]
    public void ResumeCinematicSequence()
    {
        OnCinematicSequenceResume?.Invoke();
        StartNextCinematic();
    }

    private void StartNextCinematic()
    {
        _currentCinematicIndex++;
        _currentCinematic = _cinematic[_currentCinematicIndex];
        _currentRunningCinematic = StartCoroutine(_currentCinematic.RunCinematic());
    }
    
    [ContextMenu("Skip Current Cinematic")]
    public void SkipCurrentCinematic()
    {
        _cinematic[_currentCinematicIndex].SkipCinematic();
    }

    private void CinematicComplete(CinematicHandler cinematicHandler)
    {
        if (_currentCinematicIndex == _cinematic.Length - 1)
        {
            OnCinematicSequenceEnd?.Invoke();
            Debug.Log("End");
            return;
        }

        if (cinematicHandler.IsPuseCinematicSequenceOnEnd)
            OnCinematicSequencePuse?.Invoke();
        else
            StartNextCinematic();
    }
}
