using UnityEngine;

public class ParticleSystemVFX : VFXBase
{
    [SerializeField]
    ParticleSystem _particleSystem;
    [SerializeField]
    VFXSO _vfxID;


    public VFXSO VFXID => _vfxID;
    [Sirenix.OdinInspector.ShowInInspector]
    public override bool IsPlaying => _particleSystem.isPlaying && _particleSystem.IsAlive();

    public override void StartVFX(IStayOnTarget stayOnTarget, Transform GetTransform, System.Action action = null)
    {
        base.StartVFX(stayOnTarget, GetTransform, action);
        _particleSystem?.Play();
    }

    public void Detach() => transform.SetParent(null);

}
