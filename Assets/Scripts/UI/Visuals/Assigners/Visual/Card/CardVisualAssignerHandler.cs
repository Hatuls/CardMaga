using CardMaga.Card;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class CardVisualAssignerHandler : BaseVisualAssignerHandler<BattleCardData>
    {
        [Header("Visuals")]
        [SerializeField] CardBodyPartVisualAssigner _cardBodyPartVisualAssigner;
        [SerializeField] CardTypeVisualAssigner _cardTypeVisualAssigner;
        [SerializeField] CardFrameVisualAssigner _cardFrameVisualAssigner;
        [SerializeField] CardRarityVisualAssigner _cardRarityVisualAssigner;
        [SerializeField] CardStaminaVisualAssigner _cardStaminaVisualAssigner;
        [SerializeField] CardLevelVisualAssigner _cardLevelVisualAssigner;
        [SerializeField] CardImageVisualAssigner _cardImageVisualAssigner;
        [SerializeField] CardGlowVisualAssigner _cardGlowVisualAssigner;

        public override IEnumerable<BaseVisualAssigner<BattleCardData>> VisualAssigners
        {
            get
            {
                yield return _cardBodyPartVisualAssigner;
                yield return _cardTypeVisualAssigner;
                yield return _cardFrameVisualAssigner;
                yield return _cardRarityVisualAssigner;
                yield return _cardStaminaVisualAssigner;
                yield return _cardLevelVisualAssigner;
                yield return _cardImageVisualAssigner;
                yield return _cardGlowVisualAssigner;
            }
        }
    }
}
