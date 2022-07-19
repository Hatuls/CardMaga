using Art;
using CardMaga.Card;
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

        //public void Init(CardSO card, CardIconCollectionSO artSOIconCollection, CardTypePalette cardTypePalette)
        //{
        //    _bodyPartImage.sprite = artSOIconCollection.GetSprite(card.BodyPartEnum);
        //    _bodyPartImage.color = cardTypePalette.GetIconBodyPartColorFromEnum(card.CardTypeEnum);
        //    _decorImage.color = cardTypePalette.GetDecorationColorFromEnum(card.CardTypeEnum);
        //}
        ////to replace and use CraftingSlotUI
        //public void Init(CardTypeData cardTypeData, CardIconCollectionSO artSOIconCollection,CardTypePalette cardTypePalette)
        //{
        //    _bodyPartImage.sprite = artSOIconCollection.GetSprite(cardTypeData.BodyPart);
        //    _bodyPartImage.color = cardTypePalette.GetIconBodyPartColorFromEnum(cardTypeData.CardType);
        //    _decorImage.color = cardTypePalette.GetDecorationColorFromEnum(cardTypeData.CardType);
        //}

        public void AssignBodyPart(CardTypeData cardTypeData)
        {
            var art = Factory.GameFactory.Instance.ArtBlackBoard;
            _bodyPartImage.sprite = art.GetSpriteCollections<CardIconCollectionSO>().GetSprite(cardTypeData.BodyPart);
            var artPalett = art.GetPallette<CardTypePalette>();
            _bodyPartImage.color = artPalett.GetIconBodyPartColorFromEnum(cardTypeData.CardType);


            if (_decorImage != null)
                _decorImage.color = artPalett.GetDecorationColorFromEnum(cardTypeData.CardType);
            _bodyPartImage.gameObject.SetActive(cardTypeData.BodyPart != CardMaga.Card.BodyPartEnum.Empty);
        }
        #endregion
    }
}
