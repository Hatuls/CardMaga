using System;
using System.Collections;
using Battle.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class VictoryAndDefeatHandler : MonoBehaviour
{
    [SerializeField] private UnityEvent OnLeftPlayerWin;
    [SerializeField] private UnityEvent OnRightPlayerWin;
    [SerializeField] private UnityEvent OnAnimationDone;

    [SerializeField] private PlayableDirector _LeftPlayerWinDirector;
    [SerializeField] private PlayableDirector _rightPlayerWinDirector;

    public void OpenScreen(bool isLeftPlayerWon)
    {
        if (isLeftPlayerWon)
        {
            OnLeftPlayerWin?.Invoke();
            StartCoroutine(PlayTimelineRoutine(_LeftPlayerWinDirector,AnimationDone));
        }
        else
        {
            OnRightPlayerWin?.Invoke();
            if(gameObject.activeSelf)
            StartCoroutine(PlayTimelineRoutine(_rightPlayerWinDirector,AnimationDone));
        }
    }

    private void AnimationDone()
    {
        StopAllCoroutines();
       OnAnimationDone?.Invoke();
    }

    private IEnumerator PlayTimelineRoutine(PlayableDirector playableDirector, Action onComplete)
    {
        playableDirector.Play();
        yield return new WaitForSeconds((float) playableDirector.duration);
        onComplete?.Invoke();
    }
}
