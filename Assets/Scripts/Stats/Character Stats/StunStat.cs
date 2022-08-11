using Keywords;
using UnityEngine;

namespace Characters.Stats
{
    public class StunStat : BaseStat
    {

    //    VFXController bodyPart;
    //    ParticleSystemVFX stunParticle;
        public StunStat(bool isPlayer, int amount = 0) : base(isPlayer, amount)
        {
         
        }

        public override KeywordTypeEnum Keyword =>  KeywordTypeEnum.Stun;

        public override void Add(int amount)
        {
            if (Amount ==0)
            {
                //if (stunParticle == null)
                //{
                //    //var tuffle = VFXManager.Instance.RecieveParticleSystemVFX(isPlayer, Factory.GameFactory.Instance.KeywordSOHandler.GetKeywordSO(Keyword).GetVFX());
                //    //stunParticle = tuffle.Item1;
                //    //stunParticle.Cancel();
                //    //bodyPart = tuffle.Item2;
                //}
             //   stunParticle.StartVFX(stunParticle.VFXID, bodyPart.AvatarHandler.GetBodyPart(stunParticle.VFXID.DefaultBodyPart));
            }
            base.Add(amount);
        }


        public override void Reduce(int amount)
        {
            base.Reduce(amount);
          //  if (Amount <=0 && stunParticle.IsPlaying)
            {
     //           stunParticle.Cancel();
            }
        }

        public override void Reset(int value = 0)
        {
            base.Reset(value);
 //           stunParticle?.Cancel();
        }
    }
}