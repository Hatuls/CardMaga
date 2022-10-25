using Keywords;

namespace Characters.Stats
{
    public class StunStat : BaseStat
    {

        //    VFXController bodyPart;
        //    ParticleSystemVFX stunParticle;
        public StunStat(int amount = 0) : base(amount)
        {

        }

        public override KeywordTypeEnum Keyword => KeywordTypeEnum.Stun;


    }
}