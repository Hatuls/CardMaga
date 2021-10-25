
using UnityEngine;
using Unity.Events;

public class VFXController : MonoBehaviour
{

    [SerializeField] bool _isPlayer;

    [SerializeField] SoundsEvent _soundsEvent;

    [SerializeField] Transform _headPart;
    [SerializeField] Transform _leftHandPart;
    [SerializeField] Transform _rightHandPart;
    [SerializeField] Transform _leftLegPart;
    [SerializeField] Transform _rightLegPart;
    [SerializeField] Transform _bottomBody;
    [SerializeField] Transform _chestPart;

    public void PlaySound(ParticleEffectsEnum keywordType) => _soundsEvent?.Raise(KeywordToSound(keywordType));
  
    private SoundsNameEnum KeywordToSound(ParticleEffectsEnum keywordTypeEnum)
    {
        SoundsNameEnum soundsNameEnum;
        soundsNameEnum = SoundsNameEnum.None;
        switch (keywordTypeEnum)
        {
            case ParticleEffectsEnum.Attack:
                switch ((int)Random.Range(0, 2))
                {
                    default:
                    case 0:
                        soundsNameEnum = (_isPlayer) ? SoundsNameEnum.Attacking1 : SoundsNameEnum.EnemyAttack1;
                        break;
                    case 1:
                        soundsNameEnum = (_isPlayer) ? SoundsNameEnum.Attacking1 : SoundsNameEnum.EnemyAttack2;
                        break;
                }
                break;

            case ParticleEffectsEnum.RecieveDamage: // this is recieving dmg 

                switch ((int)Random.Range(0, 2))
                {
                    default:
                    case 0:
                        soundsNameEnum = (_isPlayer) ? SoundsNameEnum.CharacterGettingHit1 : SoundsNameEnum.EnemyGettingHit1;
                        break;
                    case 1:
                        soundsNameEnum = (_isPlayer) ? SoundsNameEnum.CharacterGettingHit2 : SoundsNameEnum.EnemyGettingHit2;
                        break;
                }

                break;
            case ParticleEffectsEnum.Shield:
                soundsNameEnum = SoundsNameEnum.GainArmor;
                break;
            case ParticleEffectsEnum.Heal:
                soundsNameEnum = SoundsNameEnum.Healing;
                break;
            case ParticleEffectsEnum.Strength:
                soundsNameEnum = SoundsNameEnum.GainStrength;
                break;
            case ParticleEffectsEnum.Bleeding:

                if (_isPlayer == true)
                {
                    soundsNameEnum = SoundsNameEnum.WomanBleeding;
                }
                else
                {
                    soundsNameEnum = SoundsNameEnum.Bleeding;
                }
                break;
            case ParticleEffectsEnum.PushKick:
                soundsNameEnum = SoundsNameEnum.None;
                break;
            case ParticleEffectsEnum.Blocking:
                soundsNameEnum = SoundsNameEnum.BlockSound;
                break;
            case ParticleEffectsEnum.Crafting:
                soundsNameEnum = SoundsNameEnum.None;
                         break;
        }
        return soundsNameEnum;
    }

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

        particalEffectBase.SetParticalPosition(GetLocationFromBodyPart(part));
        particalEffectBase.PlayParticle();
    }
    private Transform GetLocationFromBodyPart(BodyPartEnum bodyPart)
    {
        Transform transformOfBodyPart = null;
        switch (bodyPart)
        {
            case BodyPartEnum.RightArm:
                transformOfBodyPart = _rightHandPart;
                break;
            case BodyPartEnum.LeftArm:
                transformOfBodyPart = _leftHandPart;
                break;
            case BodyPartEnum.Head:
                transformOfBodyPart = _headPart;
                break;
            case BodyPartEnum.LeftLeg:
                transformOfBodyPart = _leftLegPart;
                break;
            case BodyPartEnum.RightLeg:
                transformOfBodyPart = _rightLegPart;
                break;
            case BodyPartEnum.BottomBody:
                transformOfBodyPart = _bottomBody;
                break;
            case BodyPartEnum.Chest:
                transformOfBodyPart = _chestPart;
                break;
        }


        return transformOfBodyPart;
    }

    #endregion

}

public enum BodyPartEnum {
    None =0,
    RightArm = 1,
    LeftArm = 2,
    Head =3 ,
    LeftLeg=4,
    RightLeg=5,
    BottomBody=6,
    Chest=7,
    Joker =8,
};

