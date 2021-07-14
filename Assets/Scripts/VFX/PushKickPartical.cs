
using UnityEngine;

namespace Assets.Scripts.VFX
{
    class PushKickPartical : ParticalEffectBase
    {
        public override ParticleEffectsEnum GetParticalEffect => ParticleEffectsEnum.PushKick;
        public override void SetParticalPosition(Transform ParentLocation)
        {
            transform.SetParent(ParentLocation);
            RotatePartical(ParentLocation);
            transform.localPosition = Vector3.zero;
        }
        protected override void RotatePartical(Transform ParentLocation)
        {
            transform.LookAt(transform.position - ParentLocation.root.right );
        }
    }
}
