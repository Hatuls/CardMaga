using CardMaga.Card;
using CardMaga.UI.Text;
using CardMaga.UI.Visuals;
using UnityEngine;

namespace CardMaga.UI
{
    public abstract class BaseCardVisualHandler : MonoBehaviour, IInitializable<CardData>
    {
        public abstract CardZoomHandler CardZoomHandler { get; }
        public abstract BaseTextAssignerHandler<CardData> ComboTextAssignerHandler { get; }
        public abstract BaseVisualAssignerHandler<CardData> ComboVisualAssignerHandler { get; }

        public virtual void SetExecutedCardVisuals()
        {
        }
        public virtual void ActivateGlow()
        {
        }
        public virtual void DeactivateGlow()
        {
        }

        public virtual void Init(CardData cardData)
        {
            ComboTextAssignerHandler.Init(cardData);
            ComboVisualAssignerHandler.Init(cardData);
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