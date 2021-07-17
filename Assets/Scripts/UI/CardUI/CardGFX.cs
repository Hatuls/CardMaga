using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cards;
using Art;
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
        [SerializeField] Image _bodyPartIcon;

        [Tooltip("The Decoration Of The BodyPart Decoration")]
        [SerializeField] Image _bodyPartDecor;

        [Tooltip("The Decoration Of The BodyPart Decoration")]
        [SerializeField] Image _bodyPartBackground;



        [Tooltip("The background Image of the Stamina")]
        [SerializeField] Image _staminaBackground;

        [Tooltip("The Decoration of the stamina icon")]
        [SerializeField] Image _staminaDecor;




        [Tooltip("Card Background Img")]
        [SerializeField] Image _cardBackGroundImg;



        [Tooltip("Card's Background Decoration")]
        [SerializeField] Image _backGroundDecor;

        [Tooltip("Card Areas Img")]
        [SerializeField] Image _cardImage;



        [Tooltip("Card Glow Effect")]
        [SerializeField] Image _glowBackground;




        [SerializeField] RectTransform _rectTransform;

        Card _cardReferenceInHandDeck;

        #endregion

        #region Properties
        public ref Card GetCardReference { get => ref _cardReferenceInHandDeck; }
        public TextMeshProUGUI GetNameTxt => _titleText;
        public TextMeshProUGUI GetDescriptionTxt => _descriptionTxt;
        public TextMeshProUGUI GetStaminaText => _staminaText;

        //public Image GetBodyPartImg => _bodyPartIcon;
        //public Image GetTargetBodyPartImg => _targetBodyPartImg;
        //public Image GetCardEarsImg => _cardEarsImg;
        //public Image GetCardBackgroundImg => _cardBackGroundImg;
        //public Image GetCardStripesImg => _bodyPartDecor;
        //public Image GetCardKisutimImg => _cardKisutimIg;
        //public Image GetCardAreasImg => _cardAreasImg;
        //public Image GetCardGlowImg => _glowBackground;
        public ref RectTransform GetRectTransform => ref _rectTransform;
        #endregion


        #region Contructor

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

        public void GlowCard(bool toGlow)
        {
            if (_rectTransform != null && _glowBackground?.gameObject.activeSelf != toGlow)
            {
                _glowBackground?.gameObject.SetActive(toGlow);
            }
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
        internal void SetCardReference(CardSO cardData, Art.ArtSO artSO)
        {
     // set visual
            SetNameText(cardData.GetCardName.ToString());
            SetCardDescriptionText(cardData.GetCardDescription);
            SetLastCardEffectText("");
            SetBodyPartImage(artSO.IconCollection.GetSprite(cardData.GetBodyPartEnum));
          //  SetTargetedBodyPartImage(artSO.IconCollection.GetSprite(cardData.GetBodyPartEnum));
            SetCardColors(cardData.GetCardTypeEnum, artSO);
            SetStaminaText(cardData.GetStaminaCost);

            //   card.SetLastCardEffectText(cardData.GetSetCard.GetCardLCEDescription);
            //    card.SetRotation(Vector3.zero);
            //image of card
            //color of card
            //icon of body part
            //icon of targeted part

            //rotation?
        }
        internal void SetCardReference(ref Card cardData, ArtSO artSO)
        {
           _cardReferenceInHandDeck = cardData;
            SetCardReference( cardData.GetSetCard, artSO);
        }

        private void SetStaminaText(int stamina)
        {
            if (_staminaText!= null)
            {
                _staminaText.text = stamina.ToString();
            }
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
            GetStaminaText.text = lastCardEffectDescription;
        }
        private void SetBodyPartImage(Sprite bodyPartSprite)
        {
            if (bodyPartSprite == null)
            {
                // Debug.LogError("Body Part Sprite Missing");
                return;
            }
            _bodyPartIcon.sprite = bodyPartSprite;
        }
        //private void SetTargetedBodyPartImage(Sprite targetedBodyPartSprite)
        //{
        //    if (targetedBodyPartSprite == null)
        //    {
        //        //   Debug.LogError("Targeted Body Part Sprite is Missing");
        //        return;
        //    }
        //    GetTargetBodyPartImg.sprite = targetedBodyPartSprite;
        //}
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
            _cardBackGroundImg.color = color;

            color = palette.GetTopColor;
            color.a = uiColorPalette.GetFullOpacity;
            _backGroundDecor.color = color;


            color = palette.GetMiddleColor;

            color.a = uiColorPalette.GetCardAreaOpacity / 100;
            _cardBackGroundImg.color = color;//30

            GetNameTxt.color = palette.GetTopColor;

            GetDescriptionTxt.color = palette.GetTopColor;

            GetStaminaText.color = palette.GetTopColor;

            _bodyPartIcon.color = palette.GetTopColor;

          //  GetTargetBodyPartImg.color = palette.GetTopColor;
        }
        #endregion
    }
}