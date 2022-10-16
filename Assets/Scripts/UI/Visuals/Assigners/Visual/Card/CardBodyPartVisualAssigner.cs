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
            int cardType = (int)cardData.CardTypeData.CardType - 1;
            int bodyPart = (int)cardData.CardTypeData.BodyPart - 1;
            //Set Card Type object On
            SetActiveObject(cardType);

            //Set Card BG Sprites
            var sprite = BaseVisualSO.GetSpriteToAssign(cardType, cardType, _bodyPartCardVisualSO.BodyPartsBG);
            _bGImages[cardType].AssignSprite(sprite);

            //Set Card Inner BG sprites and color
            sprite = BaseVisualSO.GetSpriteToAssign(cardType, cardType, _bodyPartCardVisualSO.BodyPartsInnerBG);
            _innerBGImages[cardType].AssignSprite(sprite);
            var color = BaseVisualSO.GetColorToAssign(cardType, _bodyPartCardVisualSO.BaseSO.InnerBGColor);
            _innerBGImages[cardType].AssignColor(color);

            //Set body part and color
            sprite = BaseVisualSO.GetSpriteToAssign(cardType, bodyPart, _bodyPartCardVisualSO.BaseSO.BodyParts);
            _bodyPartsImages[cardType].AssignSprite(sprite);
            color = BaseVisualSO.GetColorToAssign(cardType, bodyPart, _bodyPartCardVisualSO.BaseSO.MainColor);
            _bodyPartsImages[cardType].AssignColor(color);
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
