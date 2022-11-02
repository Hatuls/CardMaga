using System.Collections.Generic;
using UnityEngine;

// NEED REMAKE
public class VFXManager : MonoBehaviour
{

    [SerializeField] VFXController _playerVFX, _enemyVFX;

    [SerializeField] List<ParticleSystemVFX> _VFXList;


    public void Start()
    {
        _VFXList = new List<ParticleSystemVFX>();
    }

    public (ParticleSystemVFX, VFXController) RecieveParticleSystemVFX(bool isOnPlayer, VFXSO vfx)
    {
        if (vfx == null)
            return (null, null);

        var controller = isOnPlayer ? _playerVFX : _enemyVFX;
        return (RecieveParticleSystemVFX(vfx), controller);
    }
    public ParticleSystemVFX RecieveParticleSystemVFX(VFXSO vfx)
    {
        if (vfx == null)
            return null;

        ParticleSystemVFX vfxGO = RecieveFirstOfVFX(vfx);


        return vfxGO;
    }

    private ParticleSystemVFX RecieveFirstOfVFX(VFXSO vfx)
    {
        for (int i = 0; i < _VFXList.Count; i++)
        {
            if (_VFXList[i].VFXID == vfx && !_VFXList[i].IsPlaying)
                return _VFXList[i];
        }
        return CreateVFX(vfx); ;
    }

    private ParticleSystemVFX CreateVFX(VFXSO vfx)
    {
        ParticleSystemVFX vfxBase = Instantiate(vfx.VFXPrefab).GetComponent<ParticleSystemVFX>();
        _VFXList.Add(vfxBase);
        return vfxBase;
    }

}


