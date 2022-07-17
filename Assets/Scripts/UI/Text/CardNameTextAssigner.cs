using UnityEngine;
using TMPro;

namespace UI.Text
{
    [System.Serializable]
    public class CardNameTextAssigner : BaseTextAssigner
    {
        [SerializeField] TextMeshProUGUI _cardName;
        public override void Init()
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
