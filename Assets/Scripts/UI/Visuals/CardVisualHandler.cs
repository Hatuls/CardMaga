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
#if UNITY_EDITOR
        [Header("Test")]
        [SerializeField] CardMaga.Card.CardData _cardData;



        [Button]
        void OnTryCard()
        {
            SetCardVisuals(_cardData);
        }
#endif
        public void Start()
        {
            //Visuals
            _cardVisualAssignerHandler.CheckValidation();
            //Texts
            _cardTextAssignerHandler.CheckValidation();

            //ResetCard
            _cardZoomHandler.ResetCardType();
        }
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

        public override void SetCardVisuals(CardData cardData)
        {
            //Zoom
            _cardZoomHandler.SetCardType(cardData);
            //Visuals
            _cardVisualAssignerHandler.Init(cardData);
            //Texts
            _cardTextAssignerHandler.Init(cardData);
        }

        public override void OnDestroy()
        {
            _cardVisualAssignerHandler.Dispose();
            _cardTextAssignerHandler.Dispose();
        }
    }

    public abstract class BaseCardVisualHandler : MonoBehaviour
    {
        public abstract CardZoomHandler CardZoomHandler { get; }
        public abstract void SetCardVisuals(CardMaga.Card.CardData cardData);

        public virtual void SetExecutedCardVisuals()
        {
        }
        public virtual void ActivateGlow()
        {
        }
        public virtual void DeactivateGlow()
        {
        }
        public abstract void OnDestroy();
    }
}
