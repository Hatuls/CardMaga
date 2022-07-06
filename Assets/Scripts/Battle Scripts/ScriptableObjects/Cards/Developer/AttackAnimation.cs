using UnityEngine;

namespace Cards
{
    [System.Serializable]
    public class AnimationBundle
    {
        public BodyPartEnum BodyPartEnum;
        public CameraDetails CameraDetails;

        public string AttackAnimation;
        public string ShieldAnimation;
        public string GetHitAnimation;
        public bool IsSlowMotion;
    }
    
}
