using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Cards;

namespace Battles.UI
{

    public class CardUI : MonoBehaviour, IPointerClickHandler
    {
        #region Fields
        [Tooltip("Name of Card Text")]
        [SerializeField] TextMeshProUGUI _nameTxt;

        [Tooltip("Description of card Actions Text")]
        [SerializeField] TextMeshProUGUI _descriptionTxt;

        [Tooltip("Description of LCE Action Text")]
        [SerializeField] TextMeshProUGUI _lastCardEffectTxt;

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
        //
        [SerializeField] RectTransform _rectTransform;

        [HideInInspector]
        [SerializeField] CanvasGroup _canvasGroup;

        Cards.Card _cardReferenceInHandDeck;
        #endregion

        #region Events
        [Space]
        [Header("Events")]
        [SerializeField] Unity.Events.CardUIEvent _selectCardEvent;
        [SerializeField] Unity.Events.CardUIEvent _removeCardEvent;
        [SerializeField] Unity.Events.CardUIEvent _onClickedCardEvent;
        #endregion
        #region Properties
        public CanvasGroup GetCanvasGroup => _canvasGroup;
        public TextMeshProUGUI GetNameTxt => _nameTxt;
        public TextMeshProUGUI GetDescriptionTxt => _descriptionTxt;
        public TextMeshProUGUI GetLastCardEffectTxt => _lastCardEffectTxt;
        public Image GetBodyPartImg => _bodyPartImg;
        public Image GetTargetBodyPartImg => _targetBodyPartImg;
        public Image GetCardEarsImg => _cardEarsImg;
        public Image GetCardBackgroundImg => _cardBackGroundImg;
        public Image GetCardStripesImg => _cardStripesImg;
        public Image GetCardKisutimImg => _cardKisutimIg;
        public Image GetCardAreasImg => _cardAreasImg;
        public RectTransform GetRectTransform => _rectTransform;
        public ref Cards.Card GetCardReference { get => ref _cardReferenceInHandDeck; }


        #endregion

        public void MoveCard(Vector3 moveTo)
        {
            if (GetRectTransform == null)
            {
                Debug.LogError("RectTranscorm is Null");
                return;
            }
            LeanTween.move(GetRectTransform, moveTo, Time.deltaTime);
        }
        public void MoveCard(bool withTween, Vector3 moveTo, float seconds, bool? setActiveLater = null)
        {
            if (GetRectTransform == null)
            {
                Debug.LogError("RectTranscorm is Null");
                return;
            }

            if (withTween)
            {
                if (setActiveLater == null)
                    LeanTween.move(GetRectTransform, moveTo, seconds);
                else
                    LeanTween.move(GetRectTransform, moveTo, seconds).setOnComplete(() => gameObject.SetActive(setActiveLater.GetValueOrDefault()));

            }
            else
                GetRectTransform.position = Vector2.Lerp(GetRectTransform.position, moveTo, seconds);
        }

        public void SetActive(bool setActive)
            => this.gameObject.SetActive(setActive);
        public void SetScale(Vector3 toScale, float delay)
        {
            if (GetRectTransform == null)
            {
         //       Debug.LogError("RectTranscorm is Null");
                return;
            }
            LeanTween.scale(GetRectTransform, toScale, delay);
        }
        public void SetPosition(in Vector3 setTo)
        {
            if (GetRectTransform == null)
            {
                //Debug.LogError("RectTranscorm is Null");
                return;
            }
            GetRectTransform.anchoredPosition3D = setTo;
        }
        public void SetRotation(Vector3 rotateTo)
        {
            if (GetRectTransform == null)
            {
             //   Debug.LogError("RectTranscorm is Null");
                return;
            }
            GetRectTransform.localRotation = Quaternion.Euler(rotateTo);
        }
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

        internal void SetCardReference(ref Card cardData , ArtSO artSO )
        {
            // set visual
            SetNameText(cardData.GetSetCard.GetCardName.ToString());
            SetCardDescriptionText(cardData.GetSetCard.GetCardDescription);
            SetLastCardEffectText("");
            SetBodyPartImage(artSO.IconCollection.GetSprite(cardData.GetSetCard.GetBodyPartEnum));
            SetTargetedBodyPartImage(artSO.IconCollection.GetSprite(cardData.GetSetCard.GetBodyPartEnum));
            SetCardColors( cardData.GetSetCard.GetCardTypeEnum , artSO);

            _cardReferenceInHandDeck = cardData;
            //   card.SetLastCardEffectText(cardData.GetSetCard.GetCardLCEDescription);
            //    card.SetRotation(Vector3.zero);
            //image of card
            //color of card
            //icon of body part
            //icon of targeted part

            //rotation?
        }

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
        private void SetCardColors( Cards.CardTypeEnum cardType, ArtSO artSO)
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

        public void OnPointerClick(PointerEventData eventData)
        {
            _onClickedCardEvent?.Raise(this);
        }

        public void BeginDrag()
        {
            _removeCardEvent?.Raise(this);

            _canvasGroup.blocksRaycasts = false;

            _selectCardEvent?.Raise(this);
        }

        public void EndDrag()
        {
            _canvasGroup.blocksRaycasts = true;

            Debug.Log("End Touch");
        }
    }

}