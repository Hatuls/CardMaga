using UnityEngine;
using UnityEngine.VFX;

namespace CardMaga.VFX
{
    public class VFXEffect : BaseVisualEffect
    {
        [SerializeField]
        private VisualEffect _vfx;

        public override bool IsActive => _vfx.aliveParticleCount > 0;
        public override void Play()
        {
            base.Play();
            _vfx.Play();

        }

        public override void Stop()
        {
            _vfx.Stop(); 
            base.Stop();
        }
        public override void Init()
        {
            gameObject.SetActive(true); 
            base.Init();
        }

 
    }
}