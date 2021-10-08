
using UnityEngine;
using System.Collections.Generic;


public enum ParticleEffectsEnum
{
    None = 0,
    Crafting = 1,
    Shield = 2,
    Bleeding = 3,
    Heal = 4,
    Strength = 5,
    RecieveDamage = 6,
    Blocking = 7,
    Attack = 8,
    PushKick = 9


}

public class VFXManager : MonoSingleton<VFXManager>
{

    [SerializeField]VFXController _playerVFX, _enemyVFX;

    [SerializeField] List<ParticalEffectBase> _VFXLIST = new List<ParticalEffectBase>();
    

    public override void Init()
    {

    }


    public void PlayParticle(bool isOnPlayer, BodyPartEnum part, ParticleEffectsEnum effect)
    {
        if (effect == ParticleEffectsEnum.None)
            return;

        var controller = isOnPlayer ? _playerVFX : _enemyVFX;

        if (_VFXLIST == null || _VFXLIST.Count == 0)
            Debug.LogError("VFX MANAGER VFX List is not set");
        foreach (var item in _VFXLIST)
        {
            if (item.GetParticalEffect == effect)
            {
                if (item != null)
                    controller.ActivateParticle(part, item);
                else
                    Debug.Log($"Could not find Partical Effect Base from {part.ToString()} the effect should have been {effect.ToString()}");

            }
       
        }

     


    }

    public static ParticleEffectsEnum KeywordToParticle(Keywords.KeywordTypeEnum keywordTypeEnum)
    {
        switch (keywordTypeEnum)
        {
            case Keywords.KeywordTypeEnum.Attack:
                return ParticleEffectsEnum.Attack;
            case Keywords.KeywordTypeEnum.Shield:
                return ParticleEffectsEnum.Shield;
            case Keywords.KeywordTypeEnum.Heal:
                return ParticleEffectsEnum.Heal;
            case Keywords.KeywordTypeEnum.Strength:
                return ParticleEffectsEnum.Strength;
            case Keywords.KeywordTypeEnum.Bleed:
                return ParticleEffectsEnum.Bleeding;
            case Keywords.KeywordTypeEnum.MaxHealth:
                return ParticleEffectsEnum.Heal;
            default:
                break;
        }
        return ParticleEffectsEnum.None;
    }
}
