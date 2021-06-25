using UnityEngine.UI;
using UnityEngine;

namespace Battles.UI
{
    [RequireComponent(typeof(Slider))]

    public class UIBar : MonoBehaviour
    {
        [SerializeField] Slider _slider;
        [SerializeField] Gradient _gradient;
        [SerializeField] Image _fill;
        public void SetValueBar(int value)
        {
            _slider.value = value;
            SetGradientColor();
        }

        public void SetMaxValue(int maxValue)
        {
            _slider.maxValue = maxValue;
            SetGradientColor();
        }

        public void SetGradientColor()  => _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }
}
