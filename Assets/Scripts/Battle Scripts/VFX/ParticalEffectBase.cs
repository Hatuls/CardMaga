using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public abstract class ParticalEffectBase : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;
    public abstract ParticleEffectsEnum GetParticalEffect { get; }
    public virtual void SetParticalPosition(Transform ParentLocation)
    {
        if (ParentLocation == null)
            return;

        transform.SetParent(ParentLocation);
        RotatePartical(ParentLocation);
        transform.localPosition = Vector3.zero;
        transform.SetParent(null);
    }
    protected virtual void RotatePartical(Transform ParentLocation) => transform.localRotation = ParentLocation.localRotation;

    public void PlayParticle()
    {
        if (_particleSystem == null)
              Debug.LogError(" Particle Is Not Set");
        
        if (_particleSystem.isPlaying)
            _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        _particleSystem.Play(true);
    }

}
