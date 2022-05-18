using DG.Tweening;
using ReiTools.TokenMachine;
using System;
using CardMaga.LoadingScene;
using UnityEngine;

public class BlackScreenPanel : MonoBehaviour
{
    private const int MAX_VALUE = 1, MIN_VALUE = 0;

    [SerializeField]
    private float _fadeInDuration, _fadeOutDuration;
    [SerializeField]
    private AnimationCurve _fadeInCurve, _fadeOutCurve;
    [SerializeField]
    private CanvasGroup _canvasGroup;

    private Tween _fadeInTween, _fadeOutTween;
    public void OnEnable()
    {
        LoadingSceneManager.OnBeforeSceneUnLoading += FadeIn;
        LoadingSceneManager.OnSceneLoaded += FadeOut;
    }
    public void OnDisable()
    {
        LoadingSceneManager.OnBeforeSceneUnLoading -= FadeIn;
        LoadingSceneManager.OnSceneLoaded -= FadeOut;
    }
    public void FadeIn(IRecieveOnlyTokenMachine tokenMachine)
    {
        IDisposable token = tokenMachine.GetToken();
        _fadeInTween = _canvasGroup.DOFade(MAX_VALUE, _fadeInDuration).SetEase(_fadeInCurve);
        _fadeInTween.OnComplete(token.Dispose);
    }
    public void FadeOut(IRecieveOnlyTokenMachine tokenMachine)
    {
        IDisposable token = tokenMachine.GetToken();
        _fadeOutTween = _canvasGroup.DOFade(MIN_VALUE, _fadeOutDuration).SetEase(_fadeOutCurve);
        _fadeOutTween.OnComplete(token.Dispose);
    }


    public void OnDestroy()
    {
        if (_fadeOutTween != null) _fadeOutTween.Kill();
        if (_fadeInTween != null) _fadeInTween.Kill();
    }
}
