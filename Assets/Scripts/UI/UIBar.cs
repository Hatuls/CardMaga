using UnityEngine.UI;
using UnityEngine;
using TMPro;
namespace Battles.UI
{
    [RequireComponent(typeof(Slider))]

    public class UIBar : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _maxValueText;
        [SerializeField] TextMeshProUGUI _currentValueText;
        [SerializeField] Slider _slider;
        [SerializeField] Gradient _gradient;
        [SerializeField] Image _fill;
        public void SetValueBar(int value)
        {

            _slider.value = value;
            if (_currentValueText != null)
                        _currentValueText.text = value.ToString();
            SetGradientColor();
        }

        public void SetMaxValue(int maxValue)
        {
            _slider.maxValue = maxValue;
            if (_maxValueText != null)
            _maxValueText.text = string.Concat(" / " + maxValue);
            SetGradientColor();
        }

        public void SetGradientColor()  => _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }

}
