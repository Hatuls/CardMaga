

namespace CardMaga.Animation
{
    [System.Serializable]
    public class AnimationBundle
    {
        public Card.BodyPartEnum BodyPartEnum;
        public CameraDetails CameraDetails;

        public string AttackAnimation;
        public string ShieldAnimation;
        public string GetHitAnimation;
        public bool IsSlowMotion;
    }
    
}
