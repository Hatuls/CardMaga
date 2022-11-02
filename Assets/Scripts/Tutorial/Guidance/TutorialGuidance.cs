using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TutorialGuidance : MonoBehaviour
{
    public void Init()
    {
    }

    public void StartGuidance()
    {
    }

    public void StopGuidance()
    {
    }

    public void StartGuidanceAfterSeconds(float seconds, Action onComplete = null)
    {
        StartCoroutine(GuidanceAfterSeconds(seconds, onComplete));
    }

    private IEnumerator GuidanceAfterSeconds(float seconds, Action onComplete = null)
    {
        StartGuidance();
        yield return new WaitForSeconds(seconds);
        if (onComplete != null)
            onComplete.Invoke();

        StopGuidance();
    }
}
