using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [CreateAssetMenu(fileName = "HandZoom DoTween SO", menuName = "ScriptableObjects/UI/Visuals/ZoomDoTweenSO")]

    public class ZoomDoTweenSO : ScriptableObject
    {
        public float ZoomDuration = 1;
        public float BottomPartMoveDuration = 1;
        public AnimationCurve BottomPartMoveCurve;
        public float ZoomScale = 2;
        public AnimationCurve CardCurveZoomIn;
        public float TextAlphaDuration = 1;
        public float NameTextScaleZoomIn = 0.5f;
        public float NameScaleDurationZoomIn = 0.3f;
        public AnimationCurve NameScaleCurveZoomIn;
        public float NameTextScaleZoomOut = 0.5f;
        public float NameScaleDurationZoomOut = 0.3f;
        public AnimationCurve NameScaleCurveZoomOut;
        public float BlackScreenDuration = 0.1f;
        public float GlowFadeDuration = 0.3f;
    }
}
