using Art;
using CardMaga.Card;
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
        public void Init(CardSO card,int cardLevel, ArtSO artSO)
        {

            _cardName.text = card.CardName;
            _level.text = $"LVL {cardLevel}";
            _bodyPartGFX.AssignBodyPart(card.CardType);
        }
        #endregion
    }
}
