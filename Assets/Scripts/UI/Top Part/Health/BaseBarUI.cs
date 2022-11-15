using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace CardMaga.UI.Bars
{
    public abstract class BaseBarUI : MonoBehaviour
    {
        public virtual void SetBarColor(Image sliderImage, Color color)
        {
            sliderImage.color = color;
        }
        public virtual void ResetSliderFill(Slider slider)
        {
            slider.value = 0;
        }
        public virtual void SetMaxValue(Slider slider,int maxHealth)
        {
            slider.maxValue = maxHealth;
        }
        public virtual Sequence DoMoveSlider(Sequence sequence,Slider slider, int amount, float slideTime, AnimationCurve ease)
        {
            sequence.Kill();
            sequence.Join(slider.DOValue(amount, slideTime).SetEase(ease));
            return sequence;
        }
    }
}
