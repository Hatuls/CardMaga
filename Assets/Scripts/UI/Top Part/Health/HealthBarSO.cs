using UnityEngine;
namespace CardMaga.UI.Bars
{
    [CreateAssetMenu(fileName = "Health Bar SO", menuName = "ScriptableObjects/UI/Bars/HealthBarSO")]
    public class HealthBarSO : ScriptableObject
    {
        public Color HealthColor;
        public Color FillDownColor;
        public Color FillUpColor;
        public AnimationCurve HealthStartCurve;
        public float HealthStartLength = 0.3f;
        public AnimationCurve ChangeHealthCurve;
        public float HealthTransitionLength = 0.3f;
        public AnimationCurve ChangeInnerHealthCurve;
        public float DelayTillReturn;
    }
}
