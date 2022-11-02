using UnityEngine;
using TMPro;
using CardMaga.Card;

namespace CardMaga.UI.Text
{
    [System.Serializable]
    public class CardStaminaTextAssigner : BaseTextAssigner<CardData>
    {
        [SerializeField] TextMeshProUGUI _staminaCost;

        public override void CheckValidation()
        {
            if (_staminaCost == null)
                throw new System.Exception("stamina cost is Null");
        }
        public override void Init(CardData cardData)
        {
            _staminaCost.AssignText(cardData.CardSO.StaminaCost.ToString());
        }

        public override void Dispose()
        {
        }

    }
}
