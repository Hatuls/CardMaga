using CardMaga.UI.PopUp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeBasePopUP : BasePopUp
{
    [SerializeField] private float _activationTimer = 0;

    public override void Awake()
    {
        base.Awake();
        _popUpTransitionHandler.OnEnterTransitionEnding += StartTimer;
    }

    public override void StartEnterTransition()
    {
        base.StartEnterTransition();
    }

    public override void StartExitTransition()
    {
        base.StartExitTransition();
    }

    private void StartTimer()
    {
        StopCoroutine(Timer());
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        float counter = 0;
        while (counter <= _activationTimer)
        {
            yield return null;
            counter += Time.deltaTime;
        }
        StartExitTransition();
    }

    private void OnDestroy()
    {
        _popUpTransitionHandler.OnEnterTransitionEnding -= StartTimer;
        StopAllCoroutines();
    }
}
