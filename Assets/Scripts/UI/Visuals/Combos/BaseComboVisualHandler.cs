using Battle.Combo;
using CardMaga.UI.Text;
using CardMaga.UI.Visuals;
using UnityEngine;

namespace CardMaga.UI
{
    [System.Serializable]
    public abstract class BaseComboVisualHandler : MonoBehaviour,IInitializable<BattleComboData>
    {
        public abstract BaseTextAssignerHandler<BattleComboData> ComboTextAssignerHandler { get; }
        public abstract BaseVisualAssignerHandler<BattleComboData> ComboVisualAssignerHandler { get; }

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
        public virtual void Init(BattleComboData battleComboDataData)
        {
            ComboTextAssignerHandler.Init(battleComboDataData);
            ComboVisualAssignerHandler.Init(battleComboDataData);
        }
    }
}
