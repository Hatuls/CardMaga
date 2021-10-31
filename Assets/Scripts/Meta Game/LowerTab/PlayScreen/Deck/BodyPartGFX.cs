using Art;
using Cards;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Meta.PlayScreen
{
    public class BodyPartGFX : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        Image _backgroundImage;
        [SerializeField]
        Image _decorImage;
        [SerializeField]
        Image _bodyPartImage;

        #endregion

        #region Public Methods

        public void Init(CardSO card, CardIconCollectionSO artSOIconCollection, CardTypePalette cardTypePalette)
        {
            _bodyPartImage.sprite = artSOIconCollection.GetSprite(card.BodyPartEnum);
            _bodyPartImage.color = cardTypePalette.GetIconBodyPartColorFromEnum(card.CardTypeEnum);
            _decorImage.color = cardTypePalette.GetDecorationColorFromEnum(card.CardTypeEnum);
        }
        public void Init(CardTypeData cardTypeData, CardIconCollectionSO artSOIconCollection,CardTypePalette cardTypePalette)
        {
            _bodyPartImage.sprite = artSOIconCollection.GetSprite(cardTypeData.BodyPart);
            _bodyPartImage.color = cardTypePalette.GetIconBodyPartColorFromEnum(cardTypeData.CardType);
            _decorImage.color = cardTypePalette.GetDecorationColorFromEnum(cardTypeData.CardType);
        }
        #endregion
    }
}
