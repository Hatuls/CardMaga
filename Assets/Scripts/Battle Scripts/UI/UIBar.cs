﻿using UnityEngine.UI;
using UnityEngine;
using TMPro;
namespace Battle.UI
{
    [RequireComponent(typeof(Slider))]

    public class UIBar : MonoBehaviour
    {

        [SerializeField] BarUISettings _barUISettings;
        [SerializeField] TextMeshProUGUI _maxValueText;
        [SerializeField] TextMeshProUGUI _currentValueText;
        [SerializeField] Slider _slider;
        [SerializeField] Color _clr;
        [SerializeField] bool _toUseColor = true;
        [SerializeField] Gradient _gradient;
        [SerializeField] Image _fill;
        public void SetValueBar(int value)
        {
            //  _slider.value = value;
            LeanTween.value(this.gameObject, SliderValue, _slider.value, value, _barUISettings.DelayTime).setEase(_barUISettings.DelayTimeLeanTweenEase);
            SetTextsAndColor(value);
        }

        private void SetTextsAndColor(int value)
        {
            if (_currentValueText != null)
                _currentValueText.text = value.ToString();

            SetColor();
        }
        public void InitValueBar(int value)
        {
            LeanTween.value(this.gameObject, SliderValue, _slider.value, value, _barUISettings.InitTime).setEase(_barUISettings.InitTimeLeanTweenEase);
            SetTextsAndColor(value);
        }
        private void SliderValue(float x) => _slider.value = x;
        public void SetMaxValue(int maxValue)
        {
            _slider.maxValue = maxValue;
            if (_maxValueText != null)
                _maxValueText.text = string.Concat(" / " + maxValue);
            SetColor();
        }

        public void SetColor()
        {
            _fill.color = _toUseColor ? _clr:_gradient.Evaluate(_slider.normalizedValue);
        }
    }

}
