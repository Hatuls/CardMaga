using CardMaga.Battle.Visual;
using UnityEngine;
namespace CardMaga.VFX
{
    [CreateAssetMenu(fileName = "Active Body Part Position Logic", menuName = "ScriptableObjects/VFX/Battle/Set Position/Active Body Part")]
    public class ActiveBodyPartSO : BaseBattleVisualEffectPosition
    {
        public override void SetPosition(Transform vfxObject, IVisualPlayer sceneData)
        {
            Transform activeBodyPart = sceneData.AvatarHandler.GetCurrentActiveBodyPart();
            vfxObject.SetParent(activeBodyPart);
            vfxObject.localPosition = Vector3.zero;
            vfxObject.rotation = Quaternion.identity;
            vfxObject.SetParent(null);
        }
    }

    public abstract class BaseBattleVisualEffectPosition : BaseVisualEffectPosition<IVisualPlayer> {

        protected void SetPoisition(Transform vfxObject, Transform location)
        {
            vfxObject.SetParent(location);
            vfxObject.localPosition = Vector3.zero;
            vfxObject.rotation = Quaternion.identity;
            vfxObject.SetParent(null);
        }
    }
    public abstract class BaseVisualEffectPosition<T> : ScriptableObject
    {
        public abstract void SetPosition(Transform vfxObject, T sceneData);
    }
}