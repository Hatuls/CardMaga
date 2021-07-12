using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public abstract class ParticalEffectBase : MonoBehaviour
{
    private ParticleSystem _particleSystem;   
    public abstract ParticleEffectsEnum GetParticalEffect { get; }
    public void SetParticalPosition(Transform ParentLocation)
    {
        if (ParentLocation == null)
            return;
      transform.SetParent(ParentLocation);
        transform.localRotation = ParentLocation.rotation;
      transform.localPosition = Vector3.zero;
      transform.SetParent(null);
    }

    public void PlayParticle()
    {
        if (_particleSystem==null)
        {
            Debug.LogError(" Particle Is Not Set");
        }
        if (_particleSystem.isPlaying)
            _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        _particleSystem.Play(true);
    }

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        VFXManager.RegisterParticle(this);
    }
}
