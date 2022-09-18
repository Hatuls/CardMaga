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

        public override BaseTextAssignerHandler<CardData> ComboTextAssignerHandler => _cardTextAssignerHandler;

        public override BaseVisualAssignerHandler<CardData> ComboVisualAssignerHandler => _cardVisualAssignerHandler;
#if UNITY_EDITOR
        [Header("Test")]
        [SerializeField] CardData _cardData;

        [Button]
        void OnTryCard()
        {
            Dispose();
            CheckValidation();
            Init(_cardData);
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
        public override void Init(CardData cardData)
        {
            base.Init(cardData);
            //Zoom
            _cardZoomHandler.SetCardType(cardData);
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
