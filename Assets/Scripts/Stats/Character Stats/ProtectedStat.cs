﻿using CardMaga.Keywords;
namespace Characters.Stats
{
    public class ProtectedStat : BaseStat
    {
        public ProtectedStat(int amount) : base(amount)
        {
        }

        public override KeywordType Keyword => KeywordType.Protected;
    }


    public class WeakStat : BaseStat
    {
        VFXController _vfxController;
        ParticleSystemVFX _vulnerableVFX;
        public WeakStat(int amount) : base(amount)
        {
        }

        public override KeywordType Keyword => KeywordType.Weak;

    }
    public class VulnerableKeyword : BaseStat
    {
        VFXController _vfxController;
        ParticleSystemVFX _weakParticleVFX;
        public VulnerableKeyword(int amount) : base(amount)
        {
        }
        public override void Add(int amount)
        {
            if (Amount == 0)
            {
                //if (_weakParticleVFX == null)
                //{
                //    var tuffle = VFXManager.Instance.RecieveParticleSystemVFX(isPlayer, Factory.GameFactory.Instance.KeywordSOHandler.GetKeywordSO(Keyword).GetVFX());
                //    _weakParticleVFX = tuffle.Item1;
                //    _weakParticleVFX.Cancel();
                //    _vfxController = tuffle.Item2;
                //}
                //_weakParticleVFX.StartVFX(_weakParticleVFX.VFXID, _vfxController.AvatarHandler.GetBodyPart(_weakParticleVFX.VFXID.DefaultBodyPart));
            }
            base.Add(amount);
        }
        public override void Reduce(int amount)
        {
            base.Reduce(amount);
            if (Amount <= 0 && (_weakParticleVFX?.IsPlaying ?? false))
            {
                _weakParticleVFX.Cancel();
            }
        }
        public override void Reset(int value = 0)
        {
            base.Reset(value);
            _weakParticleVFX?.Cancel();
        }
        public override KeywordType Keyword => KeywordType.Vulnerable;
    }
}