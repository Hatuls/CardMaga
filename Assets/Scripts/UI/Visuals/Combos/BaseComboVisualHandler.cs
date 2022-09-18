using Battle.Combo;
using CardMaga.UI.Text;
using CardMaga.UI.Visuals;
using UnityEngine;

namespace CardMaga.UI
{
    [System.Serializable]
    public abstract class BaseComboVisualHandler : MonoBehaviour,IInitializable<Combo>
    {
        public abstract BaseTextAssignerHandler<Combo> ComboTextAssignerHandler { get; }
        public abstract BaseVisualAssignerHandler<Combo> ComboVisualAssignerHandler { get; }

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
        public virtual void Init(Combo comboData)
        {
            ComboTextAssignerHandler.Init(comboData);
            ComboVisualAssignerHandler.Init(comboData);
        }
    }
}
