using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cards;
using Art;
using UI.Meta.PlayScreen;

namespace Battles.UI.CardUIAttributes
{
    public enum CardUILevelState { Off = 0 , On =1, Missing = 2};
    [System.Serializable]

    public class CardGFX 
    {



        #region Fields
        [SerializeField]
        ArtSO _art;

        [Tooltip("Name of Card Text")]
        [SerializeField] TextMeshProUGUI _titleText;

        [Tooltip("Description of card Actions Text")]
        [SerializeField] TextMeshProUGUI _descriptionTxt;

        [Tooltip("Description of LCE Action Text")]
        [SerializeField] TextMeshProUGUI _staminaText;

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


        [SerializeField]
        BodyPartGFX _bodyPartGFX;
        #endregion

        #region Properties
        public  Card GetCardReference { get => _cardReferenceInHandDeck; }

        public RectTransform GetRectTransform =>  _rectTransform;
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
         //   if (_rectTransform != null && _glowBackground?.gameObject.activeSelf != toGlow)
            {
              //  _glowBackground?.gameObject.SetActive(toGlow);
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
        internal void SetCardReference(CardSO cardData,byte lvl = 0)
        {
            if (cardData == null)
                return;
     // set visual
            SetNameText(cardData.CardName);

            SetCardDescriptionText(cardData.CardDescription(lvl));

            //replace with crafting slot data
            _bodyPartGFX.AssignBodyPart(cardData.CardType);
            SetCardColors(cardData.CardTypeEnum,cardData.Rarity);
           
            SetStaminaText(_cardReferenceInHandDeck == null ? cardData.StaminaCost : _cardReferenceInHandDeck.StaminaCost);

            SetCardUIImage(cardData.CardSprite);
            Debug.Log("Level Set: " + cardData.CardName + " Level = " + lvl);
            _cardUILevelHandler.SetLevels(lvl,cardData.Rarity);

        }
    
        internal void SetCardReference(Card cardData)
        {
            if (cardData == null)
            {
                Debug.LogError("Card Data is NULL!");
            }
            _cardReferenceInHandDeck = cardData;
            SetCardReference( cardData.CardSO, cardData.CardLevel);
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
       
       

        public void SetAlpha(float amount,float time,LeanTweenType type = LeanTweenType.notUsed, System.Action actionAfterAlpha = null)
        {
            LeanTween.alphaCanvas(_canvasGroup, amount, time).setEase(type).setOnComplete(actionAfterAlpha);
        }
        private void SetCardUIImage(Sprite img)
        => _innerCardImage.sprite = img;
        private void SetCardColors(CardTypeEnum cardType,RarityEnum rarity)
        {
            // Body Part:
           
            var arttypePalleta = _art.GetPallette<CardTypePalette>();
            Color clr = arttypePalleta.GetDecorationColorFromEnum(cardType);

         //   _bodyPartDecor.color = clr;
         //   _bodyPartIcon.color = arttypePalleta.GetIconBodyPartColorFromEnum(cardType);

            var carduiPalete = _art.GetPallette<CardUIPalette>();
            // Stamina Part:
            _rarityImage.color = carduiPalete.GetRarityColor(rarity);
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