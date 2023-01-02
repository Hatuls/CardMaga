using UnityEngine;
using TMPro;
using CardMaga.Card;
using Account.GeneralData;

namespace CardMaga.UI.Text
{
    [System.Serializable]
    public class CardNameTextAssigner : BaseTextAssigner<CardCore>
    {
        [SerializeField] TextMeshProUGUI _cardName;
        public override void CheckValidation()
        {
            if (_cardName == null)
                throw new System.Exception("CardTextAssigner");
        }
        public override void Init(CardCore battleCardData)
        {
            _cardName.AssignText(battleCardData.CardSO.CardName);
        }
        public override void Dispose()
        {
        }
    }
}
