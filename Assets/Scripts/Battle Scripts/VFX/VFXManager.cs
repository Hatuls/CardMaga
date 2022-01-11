
using System;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoSingleton<VFXManager>
{

    [SerializeField] VFXController _playerVFX, _enemyVFX;

    [SerializeField] List<ParticleSystemVFX> _VFXLIST;


    public override void Init()
    {
        _VFXLIST = new List<ParticleSystemVFX>();
    }
    
    public void PlayParticle(bool isOnPlayer, BodyPartEnum bodyPartEnum, VFXSO vfx)
    {
        if (vfx == null)
            return;

        var controller = isOnPlayer ? _playerVFX : _enemyVFX;
        PlayParticle(controller.AvatarHandler.GetBodyPart(bodyPartEnum),vfx);
    }
    public void PlayParticle(Transform transform, VFXSO vfx)
    {
        if (vfx == null)
            return;

        ParticleSystemVFX vfxGO = RecieveFirstOfVFX(vfx);

        vfxGO.StartVFX(vfx, transform);
    }

    private ParticleSystemVFX RecieveFirstOfVFX(VFXSO vfx)
    {
        for (int i = 0; i < _VFXLIST.Count; i++)
        {
            if (_VFXLIST[i].VFXID == vfx && !_VFXLIST[i].IsPlaying)
                return _VFXLIST[i];
        }
        return CreateVFX(vfx); ;
    }

    private ParticleSystemVFX CreateVFX(VFXSO vfx)
    {
        ParticleSystemVFX vfxBase = Instantiate(vfx.VFXPrefab).GetComponent<ParticleSystemVFX>();
        _VFXLIST.Add(vfxBase);
        return vfxBase;
    }
}


