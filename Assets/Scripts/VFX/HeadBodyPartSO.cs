using CardMaga.Battle.Visual;
using UnityEngine;
namespace CardMaga.VFX
{
    [CreateAssetMenu(fileName = "Head Body Part Position Logic", menuName = "ScriptableObjects/VFX/Battle/Set Position/Head Body Part")]
    public class HeadBodyPartSO : BaseBattleVisualEffectPosition
    {
        public override void SetPosition(Transform vfxObject, IVisualPlayer sceneData)
        {
            Transform activeBodyPart = sceneData.AvatarHandler.GetBodyPart(BodyPartEnum.Head);
            SetPoisition(vfxObject, activeBodyPart);

        }
    }
}