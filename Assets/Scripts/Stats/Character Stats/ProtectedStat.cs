using Keywords;
namespace Characters.Stats
{
    public class ProtectedStat : BaseStat
    {
        public ProtectedStat(bool isPlayer, int amount) : base(isPlayer, amount)
        {
        }

        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Protected;
    }


    public class WeakStat : BaseStat
    {
        VFXController _vfxController;
        ParticleSystemVFX _vulnerableVFX;
        public WeakStat(bool isPlayer, int amount) : base(isPlayer, amount)
        {
        }

        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Weak;
        public override void Add(int amount)
        {
 
            base.Add(amount);
        }
        public override void Reduce(int amount)
        {
            base.Reduce(amount);
      
        }
        public override void Reset(int value = 0)
        {
            base.Reset(value);
        }
    }
    public class VulnerableKeyword : BaseStat
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
        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Vulnerable;
    }
}