using Battle.Combo;
using CardMaga.UI.Text;
using CardMaga.UI.Visuals;
using UnityEngine;

namespace CardMaga.UI
{
    [System.Serializable]
    public abstract class BaseComboVisualHandler : MonoBehaviour,IInitializable<ComboData>
    {
        public abstract BaseTextAssignerHandler<ComboData> ComboTextAssignerHandler { get; }
        public abstract BaseVisualAssignerHandler<ComboData> ComboVisualAssignerHandler { get; }

        public virtual void CheckValidation()
        {
            ComboTextAssignerHandler.CheckValidation();
            ComboVisualAssignerHandler.CheckValidation();
        }
        public virtual void Dispose()
        {
            ComboTextAssignerHandler.Dispose();
            ComboVisualAssignerHandler.Dispose();
        }
        public virtual void Init(ComboData comboDataData)
        {
            ComboTextAssignerHandler.Init(comboDataData);
            ComboVisualAssignerHandler.Init(comboDataData);
        }
    }
}
