using UnityEngine;
using TMPro;

namespace CardMaga.UI.Text
{
    [System.Serializable]
    public class CardStaminaTextAssigner : BaseTextAssigner
    {
        [SerializeField] TextMeshProUGUI _staminaCost;
        public override void Init()
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
