using UnityEngine;
using TMPro;

namespace CardMaga.UI.Text
{
    [System.Serializable]
    public class CardNameTextAssigner : BaseTextAssigner<object>
    {
        [SerializeField] TextMeshProUGUI _cardName;
        public override void Init(object none)
        {
            if (_cardName == null)
                throw new System.Exception("CardTextAssigner");
        }
        public void SetCardName(string cardName)
        {
            AssignText(_cardName, cardName);
        }
    }
}
