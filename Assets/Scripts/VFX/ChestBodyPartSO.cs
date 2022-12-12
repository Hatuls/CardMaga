using CardMaga.Battle.Visual;
using UnityEngine;
namespace CardMaga.VFX
{
    [CreateAssetMenu(fileName = "Chest Body Part Position Logic", menuName = "ScriptableObjects/VFX/Battle/Set Position/Chest Body Part")]
    public class ChestBodyPartSO : BaseBattleVisualEffectPosition
    {
        public override void SetPosition(Transform vfxObject, IVisualPlayer sceneData)
        {
            Transform activeBodyPart = sceneData.AvatarHandler.GetBodyPart( BodyPartEnum.Chest);
            SetPoisition(vfxObject, activeBodyPart);
        }
    }
}