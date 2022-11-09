using CardMaga.Card;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.UI.Text
{
    [System.Serializable]
    public class CardTextAssignerHandler : BaseTextAssignerHandler<BattleCardData>
    {
        [SerializeField] CardNameTextAssigner _cardNameTextAssigner;
        [SerializeField] CardStaminaTextAssigner _cardStaminaTextAssigner;
        [SerializeField] CardDescriptionAssigner _cardDescriptionAssigner;
        public override IEnumerable<BaseTextAssigner<BattleCardData>> TextAssigners {
            get {
                yield return _cardNameTextAssigner;
                yield return _cardStaminaTextAssigner;
                yield return _cardDescriptionAssigner;
            }
        }
    }
}
