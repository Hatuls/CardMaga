using UnityEngine;
using TMPro;

namespace CardMaga.UI.Text
{
    [System.Serializable]
    public class CardStaminaTextAssigner : BaseTextAssigner<object>
    {
        [SerializeField] TextMeshProUGUI _staminaCost;
        public override void Init(object none)
        {
            if (_staminaCost == null)
                throw new System.Exception("stamina cost is Null");
        }
        public void SetStaminaCost(string cost)
        {
            AssignText(_staminaCost, cost);
        }
    }
}
