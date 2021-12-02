using UnityEngine;

public class VFXController : MonoBehaviour
{

    [SerializeField] bool _isPlayer;


    [SerializeField] AvatarHandler _avatarHandler;
    public AvatarHandler AvatarHandler { set => _avatarHandler = value; }


    #region Animation Event Callbacks
    private void SendParticle(BodyPartEnum bodyPartEnum, ParticleEffectsEnum particleEffectsEnum)
        => VFXManager.Instance.PlayParticle(_isPlayer, bodyPartEnum, particleEffectsEnum);
    public void ApplyRightArmParticle(ParticleEffectsEnum particleEffectsEnum)
        => SendParticle(BodyPartEnum.RightArm, particleEffectsEnum);

    public void ApplyLeftArmParticle(ParticleEffectsEnum particleEffectsEnum)
        => SendParticle(BodyPartEnum.LeftArm, particleEffectsEnum);
    public void ApplyHeadParticle(ParticleEffectsEnum particleEffectsEnum)
        => SendParticle(BodyPartEnum.Head, particleEffectsEnum);

    public void ApplyLeftLegParticle(ParticleEffectsEnum particleEffectsEnum)
       => SendParticle(BodyPartEnum.LeftLeg, particleEffectsEnum);
    public void ApplyRightLegParticle(ParticleEffectsEnum particleEffectsEnum)
          => SendParticle(BodyPartEnum.RightLeg, particleEffectsEnum);
    public void ApplyBottomBodyParticle(ParticleEffectsEnum particleEffectsEnum)
              => SendParticle(BodyPartEnum.BottomBody, particleEffectsEnum);
    public void ApplyChestBodyParticle(ParticleEffectsEnum particleEffectsEnum)
        => SendParticle(BodyPartEnum.Chest, particleEffectsEnum);
    #endregion

    #region Particle Effect function
    internal void ActivateParticle(BodyPartEnum part, ParticalEffectBase particalEffectBase)
    {
        if (particalEffectBase == null)
            Debug.LogError("Particle is null");

        particalEffectBase.SetParticalPosition(_avatarHandler.GetBodyPart(part));
        particalEffectBase.PlayParticle();
    }



    #endregion

}

public enum BodyPartEnum
{
    None = 0,
    RightArm = 1,
    LeftArm = 2,
    Head = 3,
    LeftLeg = 4,
    RightLeg = 5,
    BottomBody = 6,
    Chest = 7,
    Joker = 8,
};

