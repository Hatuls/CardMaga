using UnityEngine;
using TMPro;
using CardMaga.Card;
using Account.GeneralData;

namespace CardMaga.UI.Text
{
    [System.Serializable]
    public class CardStaminaTextAssigner : BaseTextAssigner<CardCore>
    {
        [SerializeField] TextMeshProUGUI _staminaCost;

        public override void CheckValidation()
        {
            if (_staminaCost == null)
                throw new System.Exception("stamina cost is Null");
        }
        public override void Init(CardCore battleCardData)
        {
            _staminaCost.AssignText(battleCardData.CardSO.StaminaCost.ToString());
        }

        public override void Dispose()
        {
        }

    }
}
