using CardMaga.Battle.Visual;
using UnityEngine;
namespace CardMaga.VFX
{
    [CreateAssetMenu(fileName = " Pivot Forward Position Logic", menuName = "ScriptableObjects/VFX/Battle/Set Position/Character's Pivot position With Forward offset")]
    public class PivotForwardPartSO : BaseBattleVisualEffectPosition
    {
        public Vector3 Offset;

        public override void SetPosition(Transform vfxObject, IVisualPlayer sceneData)
        {
            Transform activeBodyPart = sceneData.AvatarHandler.GetBodyPart(BodyPartEnum.Pivot);

            vfxObject.SetParent(activeBodyPart);
            vfxObject.localPosition = Offset;
            vfxObject.localEulerAngles = Vector3.zero;
            vfxObject.SetParent(null);
        }
    }

}