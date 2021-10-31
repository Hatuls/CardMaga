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
        [SerializeField] Image _cardDecor;



        [Tooltip("Card Glow Effect")]
        [SerializeField] Image _glowBackground;




        [SerializeField] RectTransform _rectTransform;

        Card _cardReferenceInHandDeck;

        [SerializeField] CanvasGroup _canvasGroup;




        #endregion

        #region Properties
        public  Card GetCardReference { get => _cardReferenceInHandDeck; }
        public TextMeshProUGUI GetNameTxt => _titleText;
        public TextMeshProUGUI GetDescriptionTxt => _descriptionTxt;
        public TextMeshProUGUI GetStaminaText => _staminaText;
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
        internal void SetCardReference(CardSO cardData, ArtSO artSO)
        {
            if (cardData == null)
                return;
     // set visual
            SetNameText(cardData.CardName);

            SetCardDescriptionText(cardData.CardDescription);

            SetLastCardEffectText("");

            SetBodyPartImage(
                artSO.IconCollection.GetSprite(
                    (_cardReferenceInHandDeck == null ? cardData.BodyPartEnum :_cardReferenceInHandDeck.BodyPartEnum)
                    )
                );

            SetCardColors(cardData.CardTypeEnum);

            SetStaminaText(_cardReferenceInHandDeck == null ? cardData.StaminaCost : _cardReferenceInHandDeck.StaminaCost);

            SetCardUIImage(cardData.CardSprite);

            //   card.SetLastCardEffectText(cardData.GetSetCard.GetCardLCEDescription);
            //    card.SetRotation(Vector3.zero);
            //image of card
            //color of card
            //icon of body part
            //icon of targeted part

        }
    
        internal void SetCardReference(Card cardData, ArtSO artSO)
        {
            if (cardData == null)
            {
                Debug.LogError("Card Data is NULL!");
            }
           _cardReferenceInHandDeck = cardData;
            SetCardReference( cardData.CardSO, artSO);
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
        public void SetAlpha(float amount,float time,LeanTweenType type = LeanTweenType.notUsed, System.Action actionAfterAlpha = null)
        {
            LeanTween.alphaCanvas(_canvasGroup, amount, time).setEase(type).setOnComplete(actionAfterAlpha);
        }
        private void SetCardUIImage(Sprite img)
        => _cardBackGroundImg.sprite = img;
        private void SetCardColors(CardTypeEnum cardType)
        {
            // Body Part:
            var artso = Factory.GameFactory.Instance.ArtBlackBoard;
            var arttypePalleta = artso.GetPallette<CardTypePalette>();
            Color clr = arttypePalleta.GetDecorationColorFromEnum(cardType);
            _bodyPartDecor.color = clr;
            _bodyPartBackground.color = arttypePalleta.GetBackgroundColorFromEnum(cardType);
            _bodyPartIcon.color = arttypePalleta.GetIconBodyPartColorFromEnum(cardType);

            var carduiPalete = artso.GetPallette<CardUIPalette>();
            // Stamina Part:
            _staminaBackground.color = carduiPalete.StaminaBackgroundColor;
            _staminaDecor.color = carduiPalete.StaminaDecorateColor;
            _staminaText.color = carduiPalete.StaminaTextColor;

            //Background Image
            _cardDecor.color = clr;
            _cardBackGroundImg.color = carduiPalete.CardDefaultBackgroundColor;


            // Description
            _descriptionTxt.color = carduiPalete.CardInformationDescriptionTextColor;
            _titleText.color = carduiPalete.CardInformationTitleTextColor;




        }
        #endregion
    }
}