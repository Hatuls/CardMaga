

using CardMaga.Tools.Pools;
using UnityEngine;
namespace CardMaga.VFX
{
    public class VFXController : MonoBehaviour
    {
        public AvatarHandler AvatarHandler { get; internal set; }
        [SerializeField]
        private VisualEffectSO _hittingVFXEffectSO;
        [SerializeField]
        private VisualEffectSO _defendingVFXEffectSO;

        private IVFXPool _vfxPool;
        public void Init(IVFXPool vfxPool)
        => _vfxPool = vfxPool;




        public void PlayHittingVFX()
        {
            var effect = _vfxPool.Pull(_hittingVFXEffectSO);
            Transform bodyPart = AvatarHandler.GetCurrentActiveBodyPart();
            Transform visualTransform = effect.transform;

            visualTransform.SetParent(bodyPart);
            visualTransform.localPosition = Vector3.zero;
            visualTransform.rotation = Quaternion.identity;
            visualTransform.SetParent(null);

       
            effect.Play();
        }

        public void PlayDefenseVFX() 
        {
            var effect = _vfxPool.Pull(_defendingVFXEffectSO);
            Transform visualTransform = effect.transform;

            visualTransform.SetParent(AvatarHandler.GetBodyPart(BodyPartEnum.Chest));
            visualTransform.localPosition = Vector3.zero;
            visualTransform.rotation = Quaternion.identity;
            effect.transform.SetParent(null);
            effect.Play();
        }
    }

}
public enum BodyPartEnum
{
    None = 0,
    RightArm = 1,
    LeftArm = 2,
    Head = 3,
    LeftLeg = 4,
    RightLeg = 5,
    BottomBody = 6,
    Chest = 7,
    LeftKnee = 8,
    RightKnee = 9,
    LeftElbow = 10,
    RightElbow = 11,
};

