using CardMaga.LoadingScene;
using DG.Tweening;
using ReiTools.TokenMachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreenPanel : MonoBehaviour
{
    public enum FillOption { Alpha, FillTransition }


    private const int MAX_VALUE = 1, MIN_VALUE = 0;

    [SerializeField, Tooltip("Duration to become black")]
    private float _fadeInDuration;
    [SerializeField, Tooltip("Duration to become transparent")]
    private float _fadeOutDuration;
    [SerializeField, Tooltip("Curve to become black")]
    private AnimationCurve _fadeInCurve;

    [SerializeField, Tooltip("Curve to become transparent")]
    private AnimationCurve _fadeOutCurve;


    [SerializeField]
    Canvas _canvas;
    Camera _cam;
    [SerializeField]
    private Image _img;

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
=> StartCoroutine(FadeInCoroutine(tokenMachine));

    public void FadeOut(IRecieveOnlyTokenMachine tokenMachine)
=> StartCoroutine(FadeOutCoroutine(tokenMachine));

    private IEnumerator FadeInCoroutine(IRecieveOnlyTokenMachine tokenMachine)
    {
        IDisposable token = tokenMachine.GetToken();
        //_canvas.enabled = false;
        //_canvas.worldCamera = Camera.main;
        //_canvas.enabled = true;
        //yield return null;
       
        yield return null;
        switch (_img.type)
        {
            case Image.Type.Simple:
                _fadeInTween = _img.DOFade(MAX_VALUE, _fadeInDuration).SetEase(_fadeInCurve);
                break;
            case Image.Type.Filled:
                _fadeInTween = _img.DOFillAmount(MAX_VALUE, _fadeInDuration).SetEase(_fadeInCurve);
                break;
            case Image.Type.Sliced:
            case Image.Type.Tiled:
            default:
                throw new Exception("Fade In Option is not implemented");
        }


        _fadeInTween.OnComplete(token.Dispose);
    }
    private IEnumerator FadeOutCoroutine(IRecieveOnlyTokenMachine tokenMachine)
    {
        IDisposable token = tokenMachine.GetToken();

        //_canvas.enabled = false;
        //_canvas.worldCamera = Camera.main;
        //_canvas.enabled = true;
        //yield return null;
        yield return null;
        switch (_img.type)
        {
            case Image.Type.Simple:
                _fadeOutTween = _img.DOFade(MIN_VALUE, _fadeOutDuration).SetEase(_fadeOutCurve);
                break;
            case Image.Type.Filled:
                _fadeOutTween = _img.DOFillAmount(MIN_VALUE, _fadeOutDuration).SetEase(_fadeOutCurve);
                break;
            case Image.Type.Sliced:
            case Image.Type.Tiled:
            default:
                throw new Exception("Fade In Onption is not implemented");
        }

        _fadeOutTween.OnComplete(token.Dispose);
    }



    public void OnDestroy()
    {
        if (_fadeOutTween != null) _fadeOutTween.Kill();
        if (_fadeInTween != null) _fadeInTween.Kill();
    }


}
