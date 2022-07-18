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

        [Header("Visuals")]
        [SerializeField] CardBodyPartVisualAssigner _cardBodyPartVisualAssigner;
        [SerializeField] CardTypeVisualAssigner _cardTypeVisualAssigner;
        [SerializeField] CardFrameVisualAssigner _cardFrameVisualAssigner;
        [SerializeField] CardRarityVisualAssigner _cardRarityVisualAssigner;
        [SerializeField] CardStaminaVisualAssigner _cardStaminaVisualAssigner;
        [SerializeField] CardLevelVisualAssigner _cardLevelVisualAssigner;
        [SerializeField] CardImageVisualAssigner _cardImageVisualAssigner;
        [SerializeField] CardGlowVisualAssigner _cardGlowVisualAssigner;

        [Header("Texts")]
        [SerializeField] CardNameTextAssigner _cardNameTextAssigner;
        [SerializeField] CardStaminaTextAssigner _cardStaminaTextAssigner;
        [SerializeField] CardDescriptionAssigner _cardDescriptionAssigner;


        public CanvasGroup CanvasGroup { get => _canvasGroup; }
        public override CardZoomHandler CardZoomHandler
        {
            get => _cardZoomHandler;
        }
#if UNITY_EDITOR
        [Header("Test")]
        [SerializeField] Cards.Card _card;



        [Button]
        void OnTryCard()
        {
            SetCardVisuals(_card);
        }
#endif
        public void Start()
        {
            //Visuals
            _cardBodyPartVisualAssigner.Init();
            _cardTypeVisualAssigner.Init();
            _cardFrameVisualAssigner.Init();
            _cardRarityVisualAssigner.Init();
            _cardStaminaVisualAssigner.Init();
            _cardLevelVisualAssigner.Init();
            _cardImageVisualAssigner.Init();
            _cardGlowVisualAssigner.Init();
            //Texts
            _cardNameTextAssigner.Init();
            _cardStaminaTextAssigner.Init();
            _cardDescriptionAssigner.Init();

            //ResetCard
            _cardZoomHandler.SetCardType(Cards.CardTypeEnum.Attack);
        }
        [Button]
        public void ActivateGlow()
        {
            _cardGlowVisualAssigner.SetGlow(true);
        }
        [Button]
        public void DeactivateGlow()
        {
            _cardGlowVisualAssigner.SetGlow(false);
        }
        public override void SetCardVisuals(Cards.Card card)
        {
            var cardTypeEnum = card.CardSO.CardTypeEnum;
            var cardLevel = card.CardLevel;
            //Zoom
            _cardZoomHandler.SetCardType(cardTypeEnum);
            //Visuals
            _cardBodyPartVisualAssigner.SetBodyPart((int)cardTypeEnum, (int)card.CardSO.BodyPartEnum);
            _cardTypeVisualAssigner.SetType((int)cardTypeEnum);
            _cardFrameVisualAssigner.SetFrame(1);
            _cardRarityVisualAssigner.SetRarity((int)card.CardSO.Rarity);
            _cardStaminaVisualAssigner.SetStamina(1);
            _cardLevelVisualAssigner.SetLevel((int)card.CardSO.Rarity, cardLevel, 1);
            _cardImageVisualAssigner.SetSplashImage(card.CardSO.CardSprite);
            //Texts
            _cardNameTextAssigner.SetCardName(card.CardSO.CardName);
            _cardStaminaTextAssigner.SetStaminaCost(card.CardSO.StaminaCost.ToString());

            cardLevel -= 1;//level 1 is level 0 on card
            //_cardDescriptionAssigner.SetCardKeywords(card.CardSO.CardDescription(cardLevel));
        }
    }

    public abstract class BaseCardVisualHandler : MonoBehaviour
    {
        public abstract CardZoomHandler CardZoomHandler { get; }
        public abstract void SetCardVisuals(Cards.Card card);
    }
}
