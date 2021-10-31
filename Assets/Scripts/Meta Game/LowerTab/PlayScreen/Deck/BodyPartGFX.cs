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
        [SerializeField]
        ArtSO _artSO;

        #endregion

        #region Public Methods

        public void Init(CardSO card)
        {
            var cardTypePalette = _artSO.GetPallette<CardTypePalette>();
            _bodyPartImage.sprite = _artSO.IconCollection.GetSprite(card.BodyPartEnum);
            _bodyPartImage.color = cardTypePalette.GetIconBodyPartColorFromEnum(card.CardTypeEnum);
            _decorImage.color = cardTypePalette.GetDecorationColorFromEnum(card.CardTypeEnum);
        }
        public void Init(CardTypeData cardTypeData)
        {
            _bodyPartImage.sprite = _artSO.IconCollection.GetSprite(cardTypeData.BodyPart);
            _bodyPartImage.color = _artSO.GetPallette<CardTypePalette>().GetIconBodyPartColorFromEnum(cardTypeData.CardType);
            _decorImage.color = _artSO.GetPallette<CardTypePalette>().GetDecorationColorFromEnum(cardTypeData.CardType);
        }
        #endregion
    }
}
