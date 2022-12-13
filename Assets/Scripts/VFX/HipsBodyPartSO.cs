using CardMaga.Battle.Visual;
using UnityEngine;
namespace CardMaga.VFX
{
    [CreateAssetMenu(fileName = "Hips Body Part Position Logic", menuName = "ScriptableObjects/VFX/Battle/Set Position/Hips Body Part")]
    public class HipsBodyPartSO : BaseBattleVisualEffectPosition
    {
        public override void SetPosition(Transform vfxObject, IVisualPlayer sceneData)
        {
            Transform activeBodyPart = sceneData.AvatarHandler.GetBodyPart(BodyPartEnum.Hips);
            SetPoisition(vfxObject, activeBodyPart);
        }
    }
}