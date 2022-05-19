using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{

    [SerializeField]
    LoadingDetails[] _loadingDetails;
    int _current;

    [SerializeField]
    AnimationCurve _progressCurve;
    [SerializeField]
    Slider _slider;

    [SerializeField]
    TextMeshProUGUI _text, _loadingText;
    [SerializeField]
    UnityEvent OnLoadingBarCompleted;

    public void Awake()
    {
        ResetSlider();
        _slider.onValueChanged.AddListener(SetLoadingText);
    }
    public void ResetSlider()
    {
        _current = 0;
        _text.text = "0.0%";
        _slider.value = 0;

    }

    public void MoveNext()
    {
        if (_current < _loadingDetails.Length)
        {
            var current = _loadingDetails[_current];
            Tween t = _slider.DOValue(current.ProgressTo, current.Duration).SetEase(_progressCurve);
            _text.text = current.Text;

            if (_current == _loadingDetails.Length - 1)
                t.OnComplete(() => OnLoadingBarCompleted?.Invoke());
            _current++;
        }
    }

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