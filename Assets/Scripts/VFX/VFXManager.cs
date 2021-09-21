
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
    [Sirenix.OdinInspector.ShowInInspector]
    static  Dictionary<ParticleEffectsEnum, ParticalEffectBase> _VFXDictionary;
    

    public override void Init()
    {

    }

    public static void RegisterParticle(ParticalEffectBase particle)
    {
        if (_VFXDictionary== null)
            _VFXDictionary = new Dictionary<ParticleEffectsEnum, ParticalEffectBase>();

        if (!_VFXDictionary.ContainsKey(particle.GetParticalEffect))
            _VFXDictionary.Add(particle.GetParticalEffect, particle);
    }


    public void PlayParticle(bool isOnPlayer, BodyPartEnum part, ParticleEffectsEnum effect)
    {
        if (effect == ParticleEffectsEnum.None)
            return;

        var controller = isOnPlayer ? _playerVFX : _enemyVFX;

        if (_VFXDictionary != null && _VFXDictionary.TryGetValue(effect, out ParticalEffectBase value))
        {
            controller.ActivateParticle(part, value);
        }
        else
            Debug.Log($"Could not find Partical Effect Base from {part.ToString()} the effect should have been {effect.ToString()}");



    }

    public static ParticleEffectsEnum KeywordToParticle(Keywords.KeywordTypeEnum keywordTypeEnum)
    {
        switch (keywordTypeEnum)
        {
            case Keywords.KeywordTypeEnum.Attack:
                return ParticleEffectsEnum.Attack;
            case Keywords.KeywordTypeEnum.Defense:
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
