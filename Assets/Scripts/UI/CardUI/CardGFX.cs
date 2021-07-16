using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cards;

namespace Battles.UI.CardUIAttributes
{

    [System.Serializable]

    public class CardGFX
    {

        #region Fields
        [Tooltip("Name of Card Text")]
        [SerializeField] TextMeshProUGUI _titleText;

        [Tooltip("Description of card Actions Text")]
        [SerializeField] TextMeshProUGUI _descriptionTxt;

        [Tooltip("Description of LCE Action Text")]
        [SerializeField] TextMeshProUGUI _staminaText;

        [Tooltip("The Body Part used for Action")]
        [SerializeField] Image _bodyPartImg;

        [Tooltip("The Body Part that is Targeted by Action")]
        [SerializeField] Image _targetBodyPartImg;

        [Tooltip("Card Ear Img")]
        [SerializeField] Image _cardEarsImg;

        [Tooltip("Card Background Img")]
        [SerializeField] Image _cardBackGroundImg;

        [Tooltip("Card Sripes Img")]
        [SerializeField] Image _cardStripesImg;

        [Tooltip("Card Kisutim Img")]
        [SerializeField] Image _cardKisutimIg;

        [Tooltip("Card Areas Img")]
        [SerializeField] Image _cardAreasImg;

        [SerializeField] RectTransform _rectTransform;

        Card _cardReferenceInHandDeck;

        #endregion

        #region Properties
        public ref Card GetCardReference { get => ref _cardReferenceInHandDeck; }
        public TextMeshProUGUI GetNameTxt => _titleText;
        public TextMeshProUGUI GetDescriptionTxt => _descriptionTxt;
        public TextMeshProUGUI GetLastCardEffectTxt => _staminaText;
        public Image GetBodyPartImg => _bodyPartImg;
        public Image GetTargetBodyPartImg => _targetBodyPartImg;
        public Image GetCardEarsImg => _cardEarsImg;
        public Image GetCardBackgroundImg => _cardBackGroundImg;
        public Image GetCardStripesImg => _cardStripesImg;
        public Image GetCardKisutimImg => _cardKisutimIg;
        public Image GetCardAreasImg => _cardAreasImg;

        public ref RectTransform GetRectTransform => ref _rectTransform;
        #endregion


        #region Contructor
        public CardGFX(
            ref TextMeshProUGUI title, ref TextMeshProUGUI description, ref TextMeshProUGUI stamina,
            ref Image bodyPart, ref Image targetBodyPart,
            ref Image cardEars, ref Image cardBackground, ref Image cardStripes,
            ref Image cardkisutim, ref Image cardArea,
            ref RectTransform rectTransform, Card card = null
            )
        {
            _titleText = title;
            _descriptionTxt = description;
            _staminaText = stamina;
            _bodyPartImg = bodyPart;
            _targetBodyPartImg = targetBodyPart;
            _cardEarsImg = cardEars;
            _cardBackGroundImg = cardBackground;
            _cardStripesImg = cardStripes;
            _cardKisutimIg = cardkisutim;
            _cardAreasImg = cardArea;
            _rectTransform = rectTransform;
            _cardReferenceInHandDeck = card;
        }
        public CardGFX() { }
        #endregion

        #region Methods
        private void SetNameText(in string cardName)
        {
            if (cardName == null)
            {
                //   Debug.LogError("No Name For Card");
                return;
            }
            GetNameTxt.text = cardName;
        }
        private void SetCardDescriptionText(in string cardDescription)
        {
            if (cardDescription == null)
            {
                //  Debug.LogError("No Description For Card");
                return;
            }
            GetDescriptionTxt.text = cardDescription;
        }

        internal void SetCardReference(ref Card cardData, ArtSO artSO)
        {
            // set visual
            SetNameText(cardData.GetSetCard.GetCardName.ToString());
            SetCardDescriptionText(cardData.GetSetCard.GetCardDescription);
            SetLastCardEffectText("");
            SetBodyPartImage(artSO.IconCollection.GetSprite(cardData.GetSetCard.GetBodyPartEnum));
            SetTargetedBodyPartImage(artSO.IconCollection.GetSprite(cardData.GetSetCard.GetBodyPartEnum));
            SetCardColors(cardData.GetSetCard.GetCardTypeEnum, artSO);

            _cardReferenceInHandDeck = cardData;
            //   card.SetLastCardEffectText(cardData.GetSetCard.GetCardLCEDescription);
            //    card.SetRotation(Vector3.zero);
            //image of card
            //color of card
            //icon of body part
            //icon of targeted part

            //rotation?
        }
        public void SetActive(bool setActive)
        => this._rectTransform?.gameObject.SetActive(setActive);
        private void SetLastCardEffectText(in string lastCardEffectDescription)
        {
            if (lastCardEffectDescription == null)
            {
                // Debug.LogError("No Description for LCE");
                return;
            }
            GetLastCardEffectTxt.text = lastCardEffectDescription;
        }
        private void SetBodyPartImage(Sprite bodyPartSprite)
        {
            if (bodyPartSprite == null)
            {
                // Debug.LogError("Body Part Sprite Missing");
                return;
            }
            GetBodyPartImg.sprite = bodyPartSprite;
        }
        private void SetTargetedBodyPartImage(Sprite targetedBodyPartSprite)
        {
            if (targetedBodyPartSprite == null)
            {
                //   Debug.LogError("Targeted Body Part Sprite is Missing");
                return;
            }
            GetTargetBodyPartImg.sprite = targetedBodyPartSprite;
        }
        private void SetCardColors(Cards.CardTypeEnum cardType, ArtSO artSO)
        {
            var uiColorPalette = artSO.UIColorPalette;

            if (uiColorPalette == null)
            {
                //   Debug.LogError("Error in SetCardColors");
                return;
            }

            var palette = uiColorPalette.GetCardColorType(cardType);
            var color = uiColorPalette.GetBackgroundColor;
            color.a = uiColorPalette.GetFullOpacity;
            GetCardBackgroundImg.color = color;

            color = palette.GetTopColor;
            color.a = uiColorPalette.GetFullOpacity;
            GetCardKisutimImg.color = color;


            color = palette.GetMiddleColor;

            color.a = uiColorPalette.GetCardEarsOpacity / 100;
            GetCardEarsImg.color = color; //70


            color.a = uiColorPalette.GetCardStripesOpacity / 100;
            GetCardStripesImg.color = color;//50


            color.a = uiColorPalette.GetCardAreaOpacity / 100;
            GetCardAreasImg.color = color;//30

            GetNameTxt.color = palette.GetTopColor;

            GetDescriptionTxt.color = palette.GetTopColor;

            GetLastCardEffectTxt.color = palette.GetTopColor;

            GetBodyPartImg.color = palette.GetTopColor;

            GetTargetBodyPartImg.color = palette.GetTopColor;
        }
        #endregion
    }
}