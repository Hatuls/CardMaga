using UnityEngine;
using TMPro;
using CardMaga.Card;

namespace CardMaga.UI.Text
{
    [System.Serializable]
    public class CardStaminaTextAssigner : BaseTextAssigner<BattleCardData>
    {
        [SerializeField] TextMeshProUGUI _staminaCost;

        public override void CheckValidation()
        {
            if (_staminaCost == null)
                throw new System.Exception("stamina cost is Null");
        }
        public override void Init(BattleCardData battleCardData)
        {
            _staminaCost.AssignText(battleCardData.CardSO.StaminaCost.ToString());
        }

        public override void Dispose()
        {
        }

    }
}
