﻿using Keywords;
namespace Characters.Stats
{
    public class ProtectedStat : StatAbst
    {
        public ProtectedStat(bool isPlayer, int amount) : base(isPlayer, amount)
        {
        }

        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Protected;
    }


    public class WeakStat : StatAbst
    {
        VFXController _vfxController;
        ParticleSystemVFX _vulnerableVFX;
        public WeakStat(bool isPlayer, int amount) : base(isPlayer, amount)
        {
        }

        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Weak;
        public override void Add(int amount)
        {
            if (Amount == 0)
            {
                if (_vulnerableVFX == null)
                {
                    var tuffle = VFXManager.Instance.RecieveParticleSystemVFX(isPlayer, Factory.GameFactory.Instance.KeywordSOHandler.GetKeywordSO(Keyword).GetVFX());
                    _vulnerableVFX = tuffle.Item1;
                    _vulnerableVFX.Cancel();
                    _vfxController = tuffle.Item2;
                }
                _vulnerableVFX.StartVFX(_vulnerableVFX.VFXID, _vfxController.AvatarHandler.GetBodyPart(_vulnerableVFX.VFXID.DefaultBodyPart));
            }
            base.Add(amount);
        }
        public override void Reduce(int amount)
        {
            base.Reduce(amount);
            if (Amount <= 0 && _vulnerableVFX.IsPlaying)
            {
                _vulnerableVFX.Cancel();
            }
        }
        public override void Reset(int value = 0)
        {
            base.Reset(value);
            _vulnerableVFX?.Cancel();
        }
    }
    public class VulnerableKeyword : StatAbst
    {
        VFXController _vfxController;
        ParticleSystemVFX _weakParticleVFX;
        public VulnerableKeyword(bool isPlayer, int amount) : base(isPlayer, amount)
        {
        }
        public override void Add(int amount)
        {
            if (Amount == 0)
            {
                if (_weakParticleVFX == null)
                {
                    var tuffle = VFXManager.Instance.RecieveParticleSystemVFX(isPlayer, Factory.GameFactory.Instance.KeywordSOHandler.GetKeywordSO(Keyword).GetVFX());
                    _weakParticleVFX = tuffle.Item1;
                    _weakParticleVFX.Cancel();
                    _vfxController = tuffle.Item2;
                }
                _weakParticleVFX.StartVFX(_weakParticleVFX.VFXID, _vfxController.AvatarHandler.GetBodyPart(_weakParticleVFX.VFXID.DefaultBodyPart));
            }
            base.Add(amount);
        }
        public override void Reduce(int amount)
        {
            base.Reduce(amount);
            if (Amount <= 0 && _weakParticleVFX.IsPlaying)
            {
                _weakParticleVFX.Cancel();
            }
        }
        public override void Reset(int value = 0)
        {
            base.Reset(value);
            _weakParticleVFX?.Cancel();
        }
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Vulnerable;
    }
}