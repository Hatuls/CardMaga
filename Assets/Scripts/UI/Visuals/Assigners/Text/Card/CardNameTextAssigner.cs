using UnityEngine;
using TMPro;
using CardMaga.Card;

namespace CardMaga.UI.Text
{
    [System.Serializable]
    public class CardNameTextAssigner : BaseTextAssigner<BattleCardData>
    {
        [SerializeField] TextMeshProUGUI _cardName;
        public override void CheckValidation()
        {
            if (_cardName == null)
                throw new System.Exception("CardTextAssigner");
        }
        public override void Init(BattleCardData battleCardData)
        {
            _cardName.AssignText(battleCardData.CardSO.CardName);
        }
        public override void Dispose()
        {
        }
    }
}
