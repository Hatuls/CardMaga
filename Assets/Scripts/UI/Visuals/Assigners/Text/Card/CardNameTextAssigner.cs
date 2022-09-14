using UnityEngine;
using TMPro;
using CardMaga.Card;

namespace CardMaga.UI.Text
{
    [System.Serializable]
    public class CardNameTextAssigner : BaseTextAssigner<CardData>
    {
        [SerializeField] TextMeshProUGUI _cardName;
        public override void CheckValidation()
        {
            if (_cardName == null)
                throw new System.Exception("CardTextAssigner");
        }
        public override void Init(CardData cardData)
        {
            _cardName.AssignText(cardData.CardSO.CardName);
        }
        public override void Dispose()
        {
        }
    }
}
