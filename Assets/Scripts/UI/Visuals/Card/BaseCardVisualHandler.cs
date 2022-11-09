using CardMaga.Card;
using CardMaga.UI.Text;
using CardMaga.UI.Visuals;
using UnityEngine;

namespace CardMaga.UI
{
    public abstract class BaseCardVisualHandler : MonoBehaviour, IInitializable<BattleCardData>
    {
        public abstract CardZoomHandler CardZoomHandler { get; }
        public abstract BaseTextAssignerHandler<BattleCardData> ComboTextAssignerHandler { get; }
        public abstract BaseVisualAssignerHandler<BattleCardData> ComboVisualAssignerHandler { get; }

        public virtual void SetExecutedCardVisuals()
        {
        }
        public virtual void ActivateGlow()
        {
        }
        public virtual void DeactivateGlow()
        {
        }

        public virtual void Init(BattleCardData battleCardData)
        {
            ComboTextAssignerHandler.Init(battleCardData);
            ComboVisualAssignerHandler.Init(battleCardData);
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