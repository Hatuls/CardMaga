﻿using UnityEngine;
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
        [SerializeField] Image _cardDecor;

        [Tooltip("Card Areas Img")]
        [SerializeField] Image _cardImage;



        [Tooltip("Card Glow Effect")]
        [SerializeField] Image _glowBackground;




        [SerializeField] RectTransform _rectTransform;

        Card _cardReferenceInHandDeck;



        #region Art
        static CardTypePalette _cardTypePalette; 
        static CardUIPalette _cardUIPalette ;
        private static CardUIPalette CardUIPalette
        {
            get
            {
                if (_cardUIPalette== null)
                    _cardUIPalette = ArtSettings.ArtSO.GetPallette<CardUIPalette>();
                return _cardUIPalette;
            }
        }
        private static CardTypePalette CardTypePalette
        {
            get
            {
                if (_cardTypePalette == null)
                    _cardTypePalette = ArtSettings.ArtSO.GetPallette<CardTypePalette>();

                return _cardTypePalette;
            }
        }
        #endregion

        #endregion

        #region Properties
        public ref Card GetCardReference { get => ref _cardReferenceInHandDeck; }
        public TextMeshProUGUI GetNameTxt => _titleText;
        public TextMeshProUGUI GetDescriptionTxt => _descriptionTxt;
        public TextMeshProUGUI GetStaminaText => _staminaText;
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
            SetCardColors(cardData.GetCardTypeEnum);
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
        private void SetCardColors(CardTypeEnum cardType)
        {


            if (CardTypePalette == null)
            {
                //   Debug.LogError("Error in SetCardColors");
                return;
            }






            // Body Part:
            Color clr = CardTypePalette.GetDecorationColorFromEnum(cardType);
            _bodyPartDecor.color = clr;
            _bodyPartBackground.color = CardTypePalette.GetBackgroundColorFromEnum(cardType);
            _bodyPartIcon.color = _cardTypePalette.GetIconBodyPartColorFromEnum(cardType);


            // Stamina Part:
            _staminaBackground.color = CardUIPalette.StaminaBackgroundColor;
            _staminaDecor.color = CardUIPalette.StaminaDecorateColor;
            _staminaText.color = CardUIPalette.StaminaTextColor;

            //Background Image
            _cardDecor.color = clr;

            // Description
            _descriptionTxt.color = CardUIPalette.CardInformationDescriptionTextColor;
            _titleText.color = CardUIPalette.CardInformationTitleTextColor;




        }
        #endregion
    }
}