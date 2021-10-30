using TMPro;
using UnityEngine;

namespace UI.Meta.PlayScreen
{
    public class CardUIRow: MonoBehaviour
    {
        #region Fields
        [SerializeField]
        TextMeshProUGUI _cardName;
        [SerializeField]
        TextMeshProUGUI _level;
        [SerializeField]
        BodyPartGFX _bodyPartGFX;
        #endregion
        #region Public Methods
        public void Init(Cards.CardSO card,byte cardLevel)
        {
            _cardName.text = card.CardName;
            _level.text = $"LVL {cardLevel}";
            _bodyPartGFX.Init(card);
          
        }
        #endregion
    }
}
