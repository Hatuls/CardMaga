using CardMaga.UI.Text;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    public abstract class BaseAccountBarVisualHandler : MonoBehaviour, IInitializable<AccountBarVisualData>
    {
        public abstract BaseVisualAssignerHandler<AccountBarVisualData> AccountBarVisualAssignerHandler { get; }
        public abstract BaseTextAssignerHandler<AccountBarVisualData> AccountBarTextAssignerHandler { get; }
        public virtual void CheckValidation()
        {
            AccountBarTextAssignerHandler.CheckValidation();
            AccountBarVisualAssignerHandler.CheckValidation();
        }

        public virtual void Dispose()
        {
            AccountBarTextAssignerHandler.Dispose();
            AccountBarVisualAssignerHandler.Dispose();
        }

        public virtual void Init(AccountBarVisualData comboData)
        {
            AccountBarTextAssignerHandler.Init(comboData);
            AccountBarVisualAssignerHandler.Init(comboData);
        }
    }
}