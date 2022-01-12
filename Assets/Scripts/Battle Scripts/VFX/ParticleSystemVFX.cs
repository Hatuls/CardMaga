using UnityEngine;
using TokenFactory;
public class ParticleSystemVFX : VFXBase
{
    [SerializeField]
    ParticleSystem _particleSystem;
    [SerializeField]
    VFXSO _vfxID;


    public VFXSO VFXID => _vfxID;
    public override bool IsPlaying => _particleSystem.isPlaying&& _particleSystem.IsAlive();
   
    public override void StartVFX(IStayOnTarget stayOnTarget, Transform GetTransform)
    {
        base.StartVFX(stayOnTarget, GetTransform);
        _particleSystem?.Play();
    }
}
