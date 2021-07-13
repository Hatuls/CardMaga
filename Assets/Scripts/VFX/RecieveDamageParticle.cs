using UnityEngine;

public class RecieveDamageParticle : ParticalEffectBase
{

    public override ParticleEffectsEnum GetParticalEffect => ParticleEffectsEnum.RecieveDamage;
    protected override void RotatePartical(Transform ParentLocation)
    {
        transform.LookAt(transform.position + ParentLocation.root.forward);
    }
}
