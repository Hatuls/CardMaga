using UnityEngine;

namespace CardMaga.VFX
{
    public class ParticleEffect : BaseVisualEffect
    {
        [SerializeField]
        private ParticleSystem _particleEffect;

        public override bool IsActive => _particleEffect.isPlaying;
        public override void Play()
        {
            base.Play();
            _particleEffect.Play();
        }

        public override void Stop()
        {
            _particleEffect.Stop();
            base.Stop();
        }
        public override void Init()
        {
            gameObject.SetActive(true);
            base.Init();
        }
    }
}