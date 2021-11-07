﻿using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cards;
using Art;
namespace Battles.UI.CardUIAttributes
{
    public enum CardUILevelState { Off = 0 , On =1, Missing = 2};
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



        [SerializeField] CardUILevelHandler _cardUILevelHandler;

        [Tooltip("Card Background Img")]
        [SerializeField] Image _cardFrameIMG;


        [Tooltip("Card Img")]
        [SerializeField] Image _innerCardImage;



        [Tooltip("Card Glow Effect")]
        [SerializeField] Image _glowBackground;

        [Tooltip("Card Rarity Img")]
        [SerializeField] Image _rarityImage;

        [SerializeField] RectTransform _rectTransform;

        Card _cardReferenceInHandDeck;

        [SerializeField] CanvasGroup _canvasGroup;




        #endregion

        #region Properties
        public  Card GetCardReference { get => _cardReferenceInHandDeck; }

        public RectTransform GetRectTransform =>  _rectTransform;
        #endregion


        #region Contructor

        public CardGFX(){}
        #endregion

        #region Methods
        private void SetNameText(in string cardName)
        {
            if (cardName == null)
            {
                //   Debug.LogError("No Name For Card");
                return;
            }
            _titleText.text = cardName;
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
            _descriptionTxt.text = cardDescription;
        }
        internal void SetCardReference(CardSO cardData, ArtSO artSO ,byte lvl = 0)
        {
            if (cardData == null)
                return;
     // set visual
            SetNameText(cardData.CardName);

            SetCardDescriptionText(cardData.CardDescription(lvl));

            SetBodyPartImage(
                artSO.IconCollection.GetSprite(
                    (_cardReferenceInHandDeck == null ? cardData.BodyPartEnum :_cardReferenceInHandDeck.BodyPartEnum)
                    )
                );

            SetCardColors(cardData.CardTypeEnum);

            SetStaminaText(_cardReferenceInHandDeck == null ? cardData.StaminaCost : _cardReferenceInHandDeck.StaminaCost);

            SetCardUIImage(cardData.CardSprite);

            _cardUILevelHandler.SetLevels(lvl,cardData.Rarity);

        }
    
        internal void SetCardReference(Card cardData, ArtSO artSO)
        {
            if (cardData == null)
            {
                Debug.LogError("Card Data is NULL!");
            }
           _cardReferenceInHandDeck = cardData;
            SetCardReference( cardData.CardSO, artSO, cardData.CardLevel);
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
       
        private void SetBodyPartImage(Sprite bodyPartSprite)
        {
            if (bodyPartSprite == null)
            {
                // Debug.LogError("Body Part Sprite Missing");
                return;
            }
            _bodyPartIcon.sprite = bodyPartSprite;
        }

        public void SetAlpha(float amount,float time,LeanTweenType type = LeanTweenType.notUsed, System.Action actionAfterAlpha = null)
        {
            LeanTween.alphaCanvas(_canvasGroup, amount, time).setEase(type).setOnComplete(actionAfterAlpha);
        }
        private void SetCardUIImage(Sprite img)
        => _innerCardImage.sprite = img;
        private void SetCardColors(CardTypeEnum cardType)
        {
            // Body Part:
            var artso = Factory.GameFactory.Instance.ArtBlackBoard;
            var arttypePalleta = artso.GetPallette<CardTypePalette>();
            Color clr = arttypePalleta.GetDecorationColorFromEnum(cardType);

            _bodyPartDecor.color = clr;
            _bodyPartIcon.color = arttypePalleta.GetIconBodyPartColorFromEnum(cardType);

            var carduiPalete = artso.GetPallette<CardUIPalette>();
            // Stamina Part:

            _staminaText.color = carduiPalete.StaminaTextColor;

            ////Background Image
            //_cardFrameIMG.color = carduiPalete.CardDefaultBackgroundColor;


            // Description
            _descriptionTxt.color = carduiPalete.CardInformationDescriptionTextColor;
            _titleText.color = carduiPalete.CardInformationTitleTextColor;

            _cardFrameIMG.sprite = carduiPalete.GetCardUIImage(cardType);
      
        }
        #endregion

    }
}