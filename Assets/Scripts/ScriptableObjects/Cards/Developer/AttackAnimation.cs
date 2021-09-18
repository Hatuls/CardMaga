﻿namespace Cards
{
    [System.Serializable]
    public class AnimationBundle
    {
        public AttackAnimation _attackAnimation;
        public ShieldAnimation _shieldAnimation;
        public GetHitAnimation _getHitAnimation;
        public bool IsSlowMotion;
        public bool IsCinemtaic;
 
    }



    public enum AttackAnimation 
    { 
    None=0,
    Jab_L =1,
    Uppercut_L =2,
    OneTwo =3,
    AxeKick = 4,
    HighKick_R = 5,
    RoundhouseKickRight = 6,
    FootShotgun=7,
    EyeLasers = 8,
    Hook_R=9
    };

    public enum ShieldAnimation
    {
        None = 0,
        MidBlock_R_Single = 1,
        MidBlock_L_Single = 2,
        Block_Single = 3
    }
    public enum GetHitAnimation
    {
        None = 0,
        MidFront_Med =1,
        HighFront_Stagger =2,
        HighRight_Med = 3,
        HighLeft_Med = 4,
        HighUpper_Weak =5,
        HighBack_Weak = 6,
    };
}
