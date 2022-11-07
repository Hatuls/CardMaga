using Battle.Combo;
using CardMaga.UI.Text;
using CardMaga.UI.Visuals;
using UnityEngine;

namespace CardMaga.UI.Buff
{
    public enum BuffTypeEnum
    {
        None = 0,
        Buff = 1,
        Debuff = 2
    }
    public abstract class BaseBuffVisualHandler : MonoBehaviour, IInitializable<BuffVisualData>
    {
        public abstract BaseTextAssignerHandler<BuffVisualData> BuffTextAssignerHandler { get; }
        public abstract BaseVisualAssignerHandler<BuffVisualData> BuffVisualAssignerHandler { get; }

        public virtual void CheckValidation()
        {
            BuffTextAssignerHandler.CheckValidation();
            BuffVisualAssignerHandler.CheckValidation();
        }

        public virtual void Dispose()
        {
            BuffTextAssignerHandler.Dispose();
            BuffVisualAssignerHandler.Dispose();
        }

        public virtual void Init(BuffVisualData buffData)
        {
            BuffTextAssignerHandler.Init(buffData);
            BuffVisualAssignerHandler.Init(buffData);
        }
    }
}