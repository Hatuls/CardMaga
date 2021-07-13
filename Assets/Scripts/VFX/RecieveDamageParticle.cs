using UnityEngine;

public class RecieveDamageParticle : ParticalEffectBase
{
    Vector3 forward;
    [UnityEngine.SerializeField] float _rotation;
    public override ParticleEffectsEnum GetParticalEffect => ParticleEffectsEnum.RecieveDamage;
    protected override void RotatePartical(Transform ParentLocation)
    {
        //  base.RotatePartical(ParentLocation);
        forward = ParentLocation.root.forward;
        // forward =(forward.normalized * _rotation) - transform.position;
        //       Quaternion.LookRotation(forward.normalized, Vector3.up) * Quaternion.Euler(0 , _rotation ,0);
        transform.localRotation = Quaternion.LookRotation(forward.normalized, Vector3.up) * Quaternion.Euler(0, 0, _rotation);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Quaternion.LookRotation(forward.normalized, Vector3.up) * Quaternion.Euler(0, 0, _rotation)* Vector3.one);
        //Gizmos.DrawLine(this.transform.position, Quaternion.Euler( 0,0, _rotation) * Vector3.one);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, Quaternion.LookRotation(forward.normalized, Vector3.up) * Quaternion.Euler(0, _rotation, 0) * Vector3.one);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, Quaternion.LookRotation(forward.normalized, Vector3.up) * Quaternion.Euler(_rotation, 0, 0) * Vector3.one);
    }
}
