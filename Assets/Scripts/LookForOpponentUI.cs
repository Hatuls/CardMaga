using Battle.MatchMaking;
using DG.Tweening;
using Rei.Utilities;
using ReiTools.TokenMachine;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

public class LookForOpponentUI : MonoBehaviour
{
    [Header("Fields:")]
    [SerializeField]
    private GameObject _context;
    [SerializeField]
    private GameObject _backgroundVisuals;
    [SerializeField]
    private CanvasGroup _canvas;

    private Tween _fade;


    [Title("Screen Details:")]
    [SerializeField, MinMaxSlider(0, 10f)]
    private Vector2 _screenDuration;
    [HorizontalGroup("Fade")]

    [SerializeField, VerticalGroup("Fade/Duration")]
    private float _fadeInDuration;
    [SerializeField, VerticalGroup("Fade/Duration")]
    private float _fadeOutDuration;

    [SerializeField, VerticalGroup("Fade/Curve"), LabelWidth(120f)]
    private AnimationCurve _fadeInCurve;

    [SerializeField, VerticalGroup("Fade/Curve"), LabelWidth(120f)]
    private AnimationCurve _fadeOutCurve;

    private IDisposable _token;
    private void Start()
    {
        _canvas.alpha = 0;
        LookForOpponent.OnStartLooking += ShowScreen;
    }

    private void OnDestroy()
    {
        LookForOpponent.OnStartLooking -= ShowScreen;
        _fade.Kill();
    }
    public void Init(ITokenReceiver tokenReciever)
    {
        _token = tokenReciever.GetToken();
    }
    private void ShowScreen()
    {
        _context.SetActive(true);
        _backgroundVisuals.SetActive(true);

        _fade = _canvas.DOFade(1, _fadeInDuration).SetEase(_fadeInCurve).OnComplete(StartScreen);

     void StartScreen()
        {
            StartCoroutine(ShowScreenDuration());
        }
    }
    private IEnumerator ShowScreenDuration()
    {
        float counter = 0;
        float duration = _screenDuration.GetRandomValue();
        while (counter < duration)
        {
            yield return null;
            counter += Time.deltaTime;
        }

        _fade = _canvas.DOFade(0, _fadeOutDuration)
            .SetEase(_fadeOutCurve)
            .OnComplete(_token.Dispose);
    }
}
