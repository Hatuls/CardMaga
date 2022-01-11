
using UnityEngine;
using System.Collections.Generic;

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

}
