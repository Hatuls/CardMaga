using DG.Tweening;
using ReiTools.TokenMachine;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{

    [SerializeField]
    private LoadingDetails[] _loadingDetails;
    private int _current;

    [SerializeField]
    AnimationCurve _progressCurve;
    [SerializeField]
    Slider _slider;

    [SerializeField]
    TextMeshProUGUI _text, _loadingText;
    [SerializeField]
    UnityEvent OnLoadingBarCompleted;
    [SerializeField]
    OperationManager _loadingOperation;
    public void Awake()
    {
        ResetSlider();
        _slider.onValueChanged.AddListener(SetLoadingText);
    }
    private void Start()
    {
        StartLoading();
    }
    public void ResetSlider()
    {
        _current = 0;
        _text.text = "Loading...";
        _loadingText.text = "0.0%";
        _slider.value = 0;
    }

    public void StartLoading()
    {
        TokenMachine t = new TokenMachine(MoveToNextScene);
        _loadingOperation.Init(t);
        _loadingOperation.StartOperation();
    }

    public void MoveNext()
    {
        if (_current < _loadingDetails.Length)
        {
            var current = _loadingDetails[_current];
            Tween t = _slider.DOValue(current.ProgressTo, current.Duration).SetEase(_progressCurve);
            _text.text = current.Text;



            _current++;
        }
    }
    private void MoveToNextScene() => OnLoadingBarCompleted?.Invoke();
    private void SetLoadingText(float val)
        => _loadingText.text = string.Concat(val, "%");


    public void OnDestroy()
    {
        _slider.onValueChanged.RemoveListener(SetLoadingText);
    }
}

[System.Serializable]
public class LoadingDetails
{
    public float ProgressTo;
    public string Text;
    public float Duration;
}