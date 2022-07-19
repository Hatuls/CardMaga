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



    public event Action OnFinishFadeIn, OnFinishFadeOut;

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
    private Image _img;

    private static TokenMachine _blackPannelTokenMachine;
    private Tween _fadeInTween, _fadeOutTween;


    public static IDisposable GetToken()=> _blackPannelTokenMachine.GetToken();

  
  
    
    public void FadeIn()
=> StartCoroutine(FadeInCoroutine());

    public void FadeOut( )
=> StartCoroutine(FadeOutCoroutine());

    private IEnumerator FadeInCoroutine()
    {

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


        _fadeInTween.OnComplete(()=>OnFinishFadeIn?.Invoke());
    }
    private IEnumerator FadeOutCoroutine()
    {
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

        _fadeOutTween.OnComplete(()=>OnFinishFadeOut?.Invoke());
    }


    public void Awake()
    {
        _blackPannelTokenMachine = new TokenMachine(FadeOut,FadeIn);
    }
    public void OnDestroy()
    {
        if (_fadeOutTween != null) _fadeOutTween.Kill();
        if (_fadeInTween != null) _fadeInTween.Kill();
    }


}
