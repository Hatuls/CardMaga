using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.LoadingScreen
{
    public class SplashArtScreen : AbstScreenTransition
    {
        [SerializeField]
        float _secondsToWait = 1;
        public override void StartTransition()
        {
            _objectHolder.SetActive(true);
            StartCoroutine(SplashScreenWait());
        }
        IEnumerator SplashScreenWait()
        {
            yield return new WaitForSeconds(_secondsToWait);
            TransitionCompleted();
        }
    }
}
