using CardMaga.Card;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class CardImageVisualAssigner : BaseVisualAssigner<CardData>
    {
        [SerializeField] Image _cardSplash;

        public override void CheckValidation()
        {
            if (_cardSplash == null)
                throw new System.Exception("CardImageVisualAssigner Card Spalsh object is null");
        }

        public override void Dispose()
        {
        }

        public override void Init(CardData cardData)
        {
            _cardSplash.AssignSprite(cardData.CardSO.CardSprite);
        }
    }
}
