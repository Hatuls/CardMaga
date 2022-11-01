using CardMaga.Card;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.Visuals
{
    [Serializable]
    public class CardBodyPartVisualAssigner : BaseVisualAssigner<CardData>
    {
        [SerializeField] BodyPartCardVisualSO _bodyPartCardVisualSO;

        [Tooltip("Attack - 0, Defense - 1, Utility - 2")]
        [SerializeField] GameObject[] _bodyPartActivationObject;

        [SerializeField] Image[] _bGImages;
        [SerializeField] Image[] _innerBGImages;
        [SerializeField] Image[] _bodyPartsImages;

        public override void CheckValidation()
        {
            //in class
            if (_bodyPartActivationObject.Length == 0)
                throw new Exception("CardBodyPartVisualAssigner has no activation object");

            if (_bGImages.Length == 0)
                throw new Exception("CardBodyPartVisualAssigner has no BGImages");

            if (_innerBGImages.Length == 0)
                throw new Exception("CardBodyPartVisualAssigner has no InnerBGImages");

            if (_bodyPartsImages.Length == 0)
                throw new Exception("CardBodyPartVisualAssigner has no BodyPartsImages");

            //in card SO
            _bodyPartCardVisualSO.CheckValidation();

        }

        public override void Dispose()
        {
        }

        public override void Init(CardData cardData)
        {
            var cardType = cardData.CardTypeData.CardType;
            var cardTypeMinusOne = (int)cardData.CardTypeData.CardType - 1;
            var bodyPart = cardData.CardTypeData.BodyPart;


#if UNITY_EDITOR
            if (cardTypeMinusOne == -1)
                Debug.LogError("Card type is -1! " + cardData.CardSO.CardName);
#endif
            //Set Card Type object On
            SetActiveObject(cardTypeMinusOne);

            //Set Card BG Sprites
            _bGImages[cardTypeMinusOne].AssignSprite(_bodyPartCardVisualSO.GetBodyPartBG(cardType));

            //Set Card Inner BG sprites and color
            _innerBGImages[cardTypeMinusOne].AssignSprite(_bodyPartCardVisualSO.GetBodyPartInnerBG(cardType));
            var color = BaseVisualSO.GetColorToAssign((int)cardType, _bodyPartCardVisualSO.BaseSO.InnerBGColor);
            _innerBGImages[cardTypeMinusOne].AssignColor(color);

            //Set body part and color
            _bodyPartsImages[cardTypeMinusOne].AssignSprite(_bodyPartCardVisualSO.BaseSO.GetBodyPartSprite(cardData.BodyPartEnum));

            color = BaseVisualSO.GetColorToAssign((int)cardType, (int)cardType, _bodyPartCardVisualSO.BaseSO.MainColor);

            if (cardData.BodyPartEnum == CardMaga.Card.BodyPartEnum.Empty)
            {
                _bodyPartsImages[cardTypeMinusOne].AssignColor(color.SetColorAlpha(0));
            }
            else
            {
                _bodyPartsImages[cardTypeMinusOne].AssignColor(color.SetColorAlpha(1));
            }
        }

        private void SetActiveObject(int cardType)
        {
            if (_bodyPartActivationObject[cardType].activeSelf)
            {
                //gameobject is already active
                return;
            }
            else
            {
                for (int i = 0; i < _bodyPartActivationObject.Length; i++)
                {
                    if (i == cardType)
                    {
                        _bodyPartActivationObject[i].SetActive(true);
                    }
                    else
                    {
                        _bodyPartActivationObject[i].SetActive(false);
                    }
                }
            }
        }
    }
}
