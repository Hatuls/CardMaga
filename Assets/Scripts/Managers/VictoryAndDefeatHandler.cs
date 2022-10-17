using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class VictoryAndDefeatHandler : MonoBehaviour
{
    [SerializeField] private UnityEvent OnLeftPlayerWin;
    [SerializeField] private UnityEvent OnRightPlayerWin;

    [SerializeField] private PlayableDirector _playableDirector;
    
    public void OpenScreen(bool isLeftPlayerWon)
    {
        if (isLeftPlayerWon)
            OnLeftPlayerWin?.Invoke();
        else
            OnRightPlayerWin?.Invoke();

        StartCoroutine(PlayTimelineRoutine(_playableDirector,Test));
    }

    private void Test()
    {
        Debug.Log("Done");
    }

    private IEnumerator AnimationRunTimeCheck()
    {
        while (Math.Abs(_playableDirector.duration - _playableDirector.time) > 0.2)
        {
            yield return null;
        }
        
        Debug.Log("Done");
        StopAllCoroutines();
    }
    
    private IEnumerator PlayTimelineRoutine(PlayableDirector playableDirector, Action onComplete)
    {
        playableDirector.Play();
        yield return new WaitForSeconds((float) playableDirector.duration);
        onComplete();
    }
}
