using Account.GeneralData;
using CardMaga.UI.Text;
using CardMaga.UI.Visuals;
using UnityEngine;

namespace CardMaga.UI
{
    public abstract class BaseCardVisualHandler : MonoBehaviour, IInitializable<CardCore>
    {
        public abstract CardZoomHandler CardZoomHandler { get; }
        public abstract BaseTextAssignerHandler<CardCore> ComboTextAssignerHandler { get; }
        public abstract BaseVisualAssignerHandler<CardCore> ComboVisualAssignerHandler { get; }

        public virtual void SetExecutedCardVisuals()
        {
        }
        public virtual void ActivateGlow()
        {
        }
        public virtual void DeactivateGlow()
        {
        }

        public virtual void Init(CardCore comboData)
        {
            ComboTextAssignerHandler.Init(comboData);
            ComboVisualAssignerHandler.Init(comboData);
        }

        public virtual void Dispose()
        {
            ComboTextAssignerHandler.Dispose();
            ComboVisualAssignerHandler.Dispose();
        }

        public virtual void CheckValidation()
        {
            ComboTextAssignerHandler.CheckValidation();
            ComboVisualAssignerHandler.CheckValidation();
        }
    }
}