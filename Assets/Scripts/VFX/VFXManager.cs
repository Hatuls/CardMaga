

using Battle;
using Managers;
using ReiTools.TokenMachine;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoSingleton<VFXManager>
{

    [SerializeField] VFXController _playerVFX, _enemyVFX;
    
    [SerializeField] List<ParticleSystemVFX> _VFXLIST;
    

    public override void Init(ITokenReciever token)
    {
        using(token.GetToken())
        _VFXLIST = new List<ParticleSystemVFX>();
    }
    
    public (ParticleSystemVFX,VFXController) RecieveParticleSystemVFX(bool isOnPlayer, VFXSO vfx)
    {
        if (vfx == null)
            return (null,null);

        var controller = isOnPlayer ? _playerVFX : _enemyVFX;
        return  ( RecieveParticleSystemVFX(vfx) , controller);
    }
    public ParticleSystemVFX RecieveParticleSystemVFX( VFXSO vfx)
    {
        if (vfx == null)
            return null;

        ParticleSystemVFX vfxGO = RecieveFirstOfVFX(vfx);

 
        return vfxGO;
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


    public override void Awake()
    {
        base.Awake();
        const int order = 5;
        BattleStarter.Register(new SequenceOperation(Init, order));
    }

}


