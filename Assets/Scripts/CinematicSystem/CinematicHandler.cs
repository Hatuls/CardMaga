using System;
using System.Collections;
using Rei.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

[Serializable]
public class CinematicHandler
{
    public event Action<CinematicHandler> OnCinematicCompleted;
    
    [SerializeField, EventsGroup] private UnityEvent OnCinematicStart;
    [SerializeField, EventsGroup] private UnityEvent OnCinematicEnd;

    [SerializeField] private string _cinematicName;
    [SerializeField,MinMaxSlider(0,20f), Tooltip("The delay before the operation will be executed")]
    private Vector2 _delayBeforeCinematic;
    [SerializeField, MinMaxSlider(0, 20f), Tooltip("The delay after the operation was executed\nNote: Will not have effect if the operation that will take the token wont release it before this delay")]
    private Vector2 _delayAfterCinematic;

    [SerializeField] private bool isPuseCinematicSequenceOnEndSequenceOnEnd;
    [SerializeField] private PlayableDirector _playableDirector;

    private double _duration;
    private int _cinematicID;
    private bool _isCompleted;

    public PlayableDirector PlayableDirector
    {
        get => _playableDirector;
    }

    public double CinematicTime
    {
        get => _playableDirector.time;
    }

    public double Duration
    {
        get => _duration;
    }

    public bool IsCompleted
    {
        get => _isCompleted;
    }

    public bool IsPuseCinematicSequenceOnEnd
    {
        get => isPuseCinematicSequenceOnEndSequenceOnEnd;
    }

    public void Init(int cinematicId)
    {
        _cinematicID = cinematicId;
        _duration = _playableDirector.duration;

    }

    private void StartCinematic()
    {
        _playableDirector.Play();
        OnCinematicStart?.Invoke();
    }

    public IEnumerator RunCinematic()
    {
        if (_delayBeforeCinematic != Vector2.zero)
            yield return new WaitForSeconds(_delayBeforeCinematic.GetRandomValue());
        
        StartCinematic();
        
        while (_playableDirector.time < _duration && !_isCompleted)
        {
            yield return null;
        }
        
        if (_delayAfterCinematic != Vector2.zero)
            yield return new WaitForSeconds(_delayAfterCinematic.GetRandomValue());
        
        _isCompleted = true;
        OnCinematicCompleted?.Invoke(this);
    }
    
    public void SkipCinematic()
    {
        _playableDirector.time = _duration - Time.deltaTime;
    }
}
