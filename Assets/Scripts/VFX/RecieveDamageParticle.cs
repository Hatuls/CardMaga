using UnityEngine;

public class RecieveDamageParticle : ParticalEffectBase
{
    
    [UnityEngine.SerializeField] float _rotation;
    public override ParticleEffectsEnum GetParticalEffect => ParticleEffectsEnum.RecieveDamage;
    protected override void RotatePartical(Transform ParentLocation)
    {
      //  base.RotatePartical(ParentLocation);
        transform.rotation = transform.rotation* Quaternion.Euler(0,0, _rotation );
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, Quaternion.Euler(0, 0, _rotation) * Vector3.one);
    }
}
