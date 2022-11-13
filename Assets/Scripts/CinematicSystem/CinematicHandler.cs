using System;
using System.Collections;
using Rei.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace CardMaga.CinematicSystem
{
    [Serializable]
public class CinematicHandler
{
    public event Action<CinematicHandler> OnCinematicCompleted;
    
    [SerializeField, EventsGroup] private UnityEvent OnCinematicStart;
    [SerializeField, EventsGroup] private UnityEvent OnCinematicEnd;

    [SerializeField,Tooltip("Name for editor use")] private string _cinematicName;
    
    [SerializeField,MinMaxSlider(0,20f), Tooltip("The delay before the cinematic will be executed")]
    private Vector2 _delayBeforeCinematic;
    [SerializeField, MinMaxSlider(0, 20f), Tooltip("The delay after the cinematic was executed")]
    private Vector2 _delayAfterCinematic;

    [SerializeField,Tooltip("Immediately start the next cinematic at the end of this one")] private bool isPauseCinematicSequenceOnEndSequenceOnEnd;
    [SerializeField, Tooltip("Disable the ability to skip the cinematic")] private bool _isDisableSkip;
    [SerializeField] private PlayableDirector _playableDirector;

    private int _cinematicID;
    private double _duration;
    private bool _isCompleted;
    private MonoBehaviour _monoBehaviour;
    
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
        get => isPauseCinematicSequenceOnEndSequenceOnEnd;
    }

    public void Init(int cinematicId,MonoBehaviour monoBehaviour)
    {
        _cinematicID = cinematicId;
        _duration = _playableDirector.duration;
        _monoBehaviour = monoBehaviour;
    }

    public Coroutine StartCinematic()
    {
        return _monoBehaviour.StartCoroutine(RunCinematic());
    }

    private IEnumerator RunCinematic()
    {
        if (_delayBeforeCinematic != Vector2.zero)
            yield return new WaitForSeconds(_delayBeforeCinematic.GetRandomValue());

        if (!_playableDirector.gameObject.activeSelf)
        {
            _playableDirector.gameObject.SetActive(true);
        }

        OnCinematicStart?.Invoke();
        _playableDirector.Play();

        while (_playableDirector.time < _duration && !_isCompleted)
        {
            yield return null;
        }
        
        if (_delayAfterCinematic != Vector2.zero)
            yield return new WaitForSeconds(_delayAfterCinematic.GetRandomValue());

        if (!_isCompleted)
            CompleteCinematic();
    }

    private void CompleteCinematic()
    {
        _isCompleted = true;
        OnCinematicEnd?.Invoke();
        OnCinematicCompleted?.Invoke(this);
    }
    
    public void SkipCinematic()
    {
        if (_isDisableSkip)
            return;
        
        _monoBehaviour.StopAllCoroutines();
        
        _playableDirector.time = _duration - Time.deltaTime;

        CompleteCinematic();
    }
}

}
