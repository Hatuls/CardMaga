using CardMaga.Battle.Visual;
using UnityEngine;
namespace CardMaga.VFX
{
    [CreateAssetMenu(fileName = "Character Pivot Position Logic", menuName = "ScriptableObjects/VFX/Battle/Set Position/Character's Pivot position")]
    public class CharactersPivotPositionSO : BaseBattleVisualEffectPosition
    {
        public override void SetPosition(Transform vfxObject, IVisualPlayer sceneData)
        {
            Transform activeBodyPart = sceneData.AvatarHandler.GetBodyPart(BodyPartEnum.Pivot);
            SetPoisition(vfxObject, activeBodyPart);
       
        }
    }
}