namespace Cards
{
    [System.Serializable]
    public class AnimationBundle
    {
        public BodyPartEnum BodyPartEnum;

        public string _attackAnimation;
        public string _shieldAnimation;
        public string _getHitAnimation;
        public bool IsSlowMotion;


        public CameraViews CinemtaicView;
 
    }
    public enum CameraViews { None = 0, OverTheShoulder = 1 }
}
