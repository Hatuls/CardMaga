using Account.GeneralData;
using CardMaga.Card;
using CardMaga.UI.Text;
using CardMaga.UI.Visuals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardMaga.UI
{
    public class CardVisualHandler : BaseCardVisualHandler
    {
        [Header("General")]
        [SerializeField] CanvasGroup _canvasGroup;

        [Header("Zoom")]
        [SerializeField] CardZoomHandler _cardZoomHandler;

        [Header("Glow")]
        [SerializeField] CardGlowHandler _cardGlowHandler;

        [Header("Visuals")]
        [SerializeField] CardVisualAssignerHandler _cardVisualAssignerHandler;

        [Header("Texts")]
        [SerializeField] CardTextAssignerHandler _cardTextAssignerHandler;


        public CanvasGroup CanvasGroup { get => _canvasGroup; }
        public override CardZoomHandler CardZoomHandler
        {
            get => _cardZoomHandler;
        }

        public override BaseTextAssignerHandler<CardCore> ComboTextAssignerHandler => _cardTextAssignerHandler;

        public override BaseVisualAssignerHandler<CardCore> ComboVisualAssignerHandler => _cardVisualAssignerHandler;
#if UNITY_EDITOR
        [Header("Test")]
        [SerializeField] CardCore CardData;

        [Button]
        void OnTryCard()
        {
            Dispose();
            CheckValidation();
            Init(CardData);
        }
#endif
        [Button]
        public override void ActivateGlow()
        {
            _cardGlowHandler.ChangeGlowState(true);
        }
        [Button]
        public override void DeactivateGlow()
        {
            _cardGlowHandler.ChangeGlowState(false);
        }
        public override void SetExecutedCardVisuals()
        {
            _cardGlowHandler.DiscardGlowAlpha();
        }
        public override void Init(CardCore battleCardData)
        {
            base.Init(battleCardData);
            //Zoom
            _cardZoomHandler.SetCardType(battleCardData);
            _cardZoomHandler.ForceReset();
        }
        public override void CheckValidation()
        {
            base.CheckValidation();
            //ResetCard
            _cardZoomHandler.ResetCardType();
        }
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
